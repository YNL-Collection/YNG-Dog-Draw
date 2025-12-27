using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [Header("Time")]
    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI LevelText;
    public float TimeRemain = 6f;

    [Header("Level")]
    public List<GameObject> levels;
    public Transform levelParent;

    private int currentLevelIndex;
    private GameObject currentLevel;
    private bool isCounting;
    private bool isGameOver;

    private const string LEVEL_KEY = "CURRENT_LEVEL";

    private void Start()
    {
        LoadLevelIndex();
        SpawnLevel(currentLevelIndex);
    }

    private void Update()
    {
        if (!isCounting || isGameOver) return;

        TimeRemain -= Time.deltaTime;
        TimeRemain = Mathf.Max(TimeRemain, 0f);

        UpdateTimeUI();

        if (TimeRemain <= 0f)
        {
            Win();
        }
    }

    #region LEVEL LOGIC

    private void SpawnLevel(int index)
    {
        if (currentLevel != null)
            Destroy(currentLevel);

        ClearAllLines();

        DrawManager draw = FindObjectOfType<DrawManager>();
        if (draw != null)
            draw.ResetDrawUI();

        index = Mathf.Clamp(index, 0, levels.Count - 1);

        currentLevel = Instantiate(levels[index], levelParent);

        TimeRemain = 6f;
        isCounting = false;
        isGameOver = false;

        UpdateTimeUI();
        UpdateLevelUI();
    }

    public void StartCountTime()
    {
        if (isGameOver) return;
        isCounting = true;
    }

    public void Win()
    {
        if (isGameOver) return;

        isGameOver = true;
        isCounting = false;

        Debug.Log("WIN!");

        currentLevelIndex++;
        SaveLevelIndex();

        PopupManager.Instance.ShowPopup_Win();
    }


    public void Loss()
    {
        if (isGameOver) return;

        isGameOver = true;
        isCounting = false;

        Debug.Log("LOSS!");

        Invoke(nameof(ReloadLevel), 1.5f);
    }

    private void LoadNextLevel()
    {
        SpawnLevel(currentLevelIndex);
    }

    private void ReloadLevel()
    {
        SpawnLevel(currentLevelIndex);
    }

    #endregion

    #region UI

    private void UpdateTimeUI()
    {
        if (TimeText != null)
            TimeText.text = Mathf.CeilToInt(TimeRemain).ToString();
    }

    private void UpdateLevelUI()
    {
        if (LevelText != null)
            LevelText.text = "LEVEL " + (currentLevelIndex + 1);
    }

    #endregion

    #region SAVE / LOAD

    private void SaveLevelIndex()
    {
        PlayerPrefs.SetInt(LEVEL_KEY, currentLevelIndex);
        PlayerPrefs.Save();
    }

    public void LoadNextLevelByPopup()
    {
        SpawnLevel(currentLevelIndex);
    }

    private void LoadLevelIndex()
    {
        currentLevelIndex = PlayerPrefs.GetInt(LEVEL_KEY, 0);
    }

    #endregion

    #region LINE CLEANUP

    private void ClearAllLines()
    {
        Line[] lines = FindObjectsOfType<Line>();
        foreach (var line in lines)
        {
            Destroy(line.gameObject);
        }
    }

    #endregion
}
