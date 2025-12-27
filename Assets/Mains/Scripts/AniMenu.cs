using UnityEngine;
using System.Collections;

public class AniMenu : MonoBehaviour
{
    [Header("References")]
    public Transform Dog;
    public GameObject Pine;
    public GameObject Beehive;
    public GameObject Bee1;
    public GameObject Bee2;

    [Header("Positions")]
    public Vector2 PoA = new Vector2(1.6f, -1f);
    public Vector2 PoB = new Vector2(10f, -1f);

    [Header("Jump / Rotate / Squash")]
    public float jumpHeight = 3f;
    public float jumpDuration = 0.7f;
    public float rotateAngle = -35f;
    public float rotateDuration = 0.25f;
    public float throwDelay = 0.2f;
    public float dogThrowDuration = 0.2f;
    public float pineThrowDuration = 0.3f;
    public float squashSpeed = 12f;
    public float squashIntensity = 0.02f;
    public float squashDuration = 1f;

    [Header("Beehive Swing")]
    public float firstSwingAngle = -20f;
    public float firstSwingDuration = 0.1f;
    public float swingAngle = 15f;
    public float swingDuration = 0.2f;
    public int swingLoops = 3;

    [Header("Chase Settings")]
    public float beeScaleDuration = 0.025f;
    public float dogRunDuration = 1f;
    public float beeRunDuration = 1.3f;
    public float dogRunDelay = 0.1f;

    [Header("Dog Run Animation")]
    public float runBobHeight = 0.08f;
    public float runBobSpeed = 20f;
    public float runSquash = 0.03f;

    Vector3 pineStartPos;
    Vector3 dogInitialScale;
    Vector3 bee1InitialPos, bee2InitialPos;

    void Awake()
    {
        pineStartPos = Pine.transform.position;
        dogInitialScale = Dog.localScale;
        bee1InitialPos = Bee1.transform.position;
        bee2InitialPos = Bee2.transform.position;
    }

    void OnEnable()
    {
        StopAllCoroutines();
        ResetState();
        StartCoroutine(MainLoop());
    }

    IEnumerator MainLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(MainSequence());
            ResetState();
        }
    }

    void ResetState()
    {
        Dog.position = PoB;
        Dog.rotation = Quaternion.identity;
        Dog.localScale = dogInitialScale;

        Pine.SetActive(false);
        Pine.transform.position = pineStartPos;
        Pine.transform.rotation = Quaternion.identity;

        Beehive.transform.rotation = Quaternion.identity;

        Bee1.SetActive(false);
        Bee2.SetActive(false);
        Bee1.transform.localScale = Vector3.zero;
        Bee2.transform.localScale = Vector3.zero;
        Bee1.transform.position = bee1InitialPos;
        Bee2.transform.position = bee2InitialPos;
    }

    IEnumerator MainSequence()
    {
        yield return StartCoroutine(JumpParabola(PoB, PoA));
        yield return StartCoroutine(SquashEffect(Dog, squashDuration));

        Pine.SetActive(true);
        yield return new WaitForSeconds(0.2f);

        StartCoroutine(RotateSmooth(Dog, rotateAngle, rotateDuration));
        StartCoroutine(RotateSmooth(Pine.transform, rotateAngle, rotateDuration));
        yield return new WaitForSeconds(rotateDuration + throwDelay);

        StartCoroutine(RotateSmooth(Dog, 0f, dogThrowDuration));
        yield return StartCoroutine(MoveTo(Pine.transform, Beehive.transform.position, pineThrowDuration));
        Pine.SetActive(false);

        yield return StartCoroutine(SwingBeehive());

        Dog.localScale = new Vector3(-dogInitialScale.x, dogInitialScale.y, dogInitialScale.z);

        Bee1.SetActive(true);
        Bee2.SetActive(true);
        StartCoroutine(ScaleOverTime(Bee1.transform, Vector3.one, beeScaleDuration));
        StartCoroutine(ScaleOverTime(Bee2.transform, Vector3.one, beeScaleDuration));

        Vector3 beeTarget = PoB;

        StartCoroutine(MoveTo(Bee1.transform, beeTarget, beeRunDuration));
        StartCoroutine(MoveTo(Bee2.transform, beeTarget + new Vector3(0.3f, 0.4f, 0), beeRunDuration));

        yield return new WaitForSeconds(dogRunDelay);

        Coroutine runAnim = StartCoroutine(DogRunAnim(dogRunDuration, 2f));
        yield return StartCoroutine(MoveTo(Dog, PoB, dogRunDuration));
        StopCoroutine(runAnim);

        float remain = beeRunDuration - (dogRunDuration + dogRunDelay);
        if (remain > 0)
            yield return new WaitForSeconds(remain);
    }

    IEnumerator MoveTo(Transform target, Vector3 endPos, float duration)
    {
        float t = 0;
        Vector3 start = target.position;
        while (t < duration)
        {
            target.position = Vector3.Lerp(start, endPos, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        target.position = endPos;
    }

    IEnumerator ScaleOverTime(Transform target, Vector3 targetScale, float duration)
    {
        float t = 0;
        while (t < duration)
        {
            target.localScale = Vector3.Lerp(Vector3.zero, targetScale, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        target.localScale = targetScale;
    }

    IEnumerator JumpParabola(Vector2 start, Vector2 end)
    {
        float t = 0;
        while (t < jumpDuration)
        {
            float p = t / jumpDuration;
            float x = Mathf.Lerp(start.x, end.x, p);
            float y = Mathf.Lerp(start.y, end.y, p) + jumpHeight * 4f * p * (1f - p);
            Dog.position = new Vector2(x, y);
            t += Time.deltaTime;
            yield return null;
        }
        Dog.position = end;
    }

    IEnumerator RotateSmooth(Transform target, float angle, float duration)
    {
        float t = 0;
        float start = target.eulerAngles.z;
        if (start > 180) start -= 360;

        while (t < duration)
        {
            float z = Mathf.Lerp(start, angle, t / duration);
            target.rotation = Quaternion.Euler(0, 0, z);
            t += Time.deltaTime;
            yield return null;
        }
        target.rotation = Quaternion.Euler(0, 0, angle);
    }

    IEnumerator SwingBeehive()
    {
        Transform h = Beehive.transform;
        yield return RotateSmooth(h, firstSwingAngle, firstSwingDuration);

        for (int i = 0; i < swingLoops; i++)
        {
            yield return RotateSmooth(h, swingAngle, swingDuration * 0.5f);
            yield return RotateSmooth(h, -swingAngle, swingDuration * 0.5f);
        }

        yield return RotateSmooth(h, 0f, swingDuration);
    }

    IEnumerator SquashEffect(Transform target, float duration)
    {
        float t = 0;
        while (t < duration)
        {
            float wave = Mathf.Sin(t * squashSpeed);
            float x = target.localScale.x > 0
                ? dogInitialScale.x + wave * squashIntensity
                : -dogInitialScale.x - wave * squashIntensity;

            target.localScale = new Vector3(
                x,
                dogInitialScale.y - wave * squashIntensity,
                dogInitialScale.z
            );

            t += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator DogRunAnim(float duration, float power)
    {
        float t = 0f;
        while (t < duration)
        {
            float wave = Mathf.Sin(t * runBobSpeed);

            float xScale = Mathf.Sign(Dog.localScale.x) *
                (Mathf.Abs(dogInitialScale.x) + wave * runSquash * power);

            Dog.localScale = new Vector3(
                xScale,
                dogInitialScale.y - Mathf.Abs(wave) * runSquash * power,
                dogInitialScale.z
            );

            Dog.position += Vector3.up * wave * runBobHeight * power * Time.deltaTime;

            t += Time.deltaTime;
            yield return null;
        }

        Dog.localScale = new Vector3(
            Mathf.Sign(Dog.localScale.x) * Mathf.Abs(dogInitialScale.x),
            dogInitialScale.y,
            dogInitialScale.z
        );
    }
}
