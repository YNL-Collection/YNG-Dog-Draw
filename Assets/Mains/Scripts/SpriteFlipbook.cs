using UnityEngine;

public class SpriteFlipbook : MonoBehaviour
{
    public Sprite[] frames;
    public float fps = 8f;

    private SpriteRenderer sr;
    private int index;
    private float timer;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1f / fps)
        {
            timer -= 1f / fps;
            index = (index + 1) % frames.Length;
            sr.sprite = frames[index];
        }
    }
}
