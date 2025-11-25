using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static Action OnDrawEnd;   // Other scripts call this
    public static Action OnDogBit;    // Bees call this when hit the dog
    public static Action OnLevelWin;
    public static Action OnLevelLose;

    [SerializeField] private List<Level> _levels = new();

    private bool dogIsBit = false;
    private Coroutine survivalRoutine;

    private void OnEnable()
    {
        OnDrawEnd += HandleDrawEnd;
        OnDogBit += HandleDogBit;
    }

    private void OnDisable()
    {
        OnDrawEnd -= HandleDrawEnd;
        OnDogBit -= HandleDogBit;
    }

    private void HandleDrawEnd()
    {
        // Reset state
        dogIsBit = false;

        // Start survival countdown
        if (survivalRoutine != null)
            StopCoroutine(survivalRoutine);

        survivalRoutine = StartCoroutine(SurvivalTimer());
    }

    private void HandleDogBit()
    {
        if (dogIsBit) return;
        dogIsBit = true;

        if (survivalRoutine != null)
        {
            StopCoroutine(survivalRoutine);
        }

        OnLevelLose?.Invoke();
        Debug.Log("Level Lose!");
    }

    private IEnumerator SurvivalTimer()
    {
        yield return new WaitForSeconds(5f);

        if (!dogIsBit)
        {
            OnLevelWin?.Invoke();

            Debug.Log("Level Won!");
        }
    }
}
