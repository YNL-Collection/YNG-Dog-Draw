using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingAni : MonoBehaviour
{
    public GameObject[] Wing;
    public float delay = 0.1f;

    private Coroutine wingCoroutine;

    private void OnEnable()
    {
        if (wingCoroutine != null)
        {
            StopCoroutine(wingCoroutine);
        }

        wingCoroutine = StartCoroutine(LoopCanh());
    }

    private void OnDisable()
    {
        if (wingCoroutine != null)
        {
            StopCoroutine(wingCoroutine);
            wingCoroutine = null;
        }
    }

    IEnumerator LoopCanh()
    {
        while (true)
        {
            for (int i = 0; i < Wing.Length; i++)
            {
                if (Wing[i] == null) continue;

                foreach (GameObject c in Wing)
                {
                    if (c != null) c.SetActive(false);
                }

                Wing[i].SetActive(true);

                yield return new WaitForSeconds(delay);
            }
        }
    }
}