using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingAni : MonoBehaviour
{
    public GameObject[] Wing;
    public float delay = 0.1f;

    private void Start()
    {
        StartCoroutine(LoopCanh());
    }

    IEnumerator LoopCanh()
    {
        while (true)
        {
            for (int i = 0; i < Wing.Length; i++)
            {
                foreach (GameObject c in Wing)
                    c.SetActive(false);

                Wing[i].SetActive(true);

                yield return new WaitForSeconds(delay);
            }
        }
    }
}
