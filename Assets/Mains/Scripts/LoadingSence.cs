using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LoadingScene : Singleton<LoadingScene>
{
    [Header("UI Groups")]
    public List<GameObject> uiGameplay;
    public List<GameObject> uiMenu;

    [Header("Canvas (Screen Space - Camera)")]
    public Canvas mainCanvas;   

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        if (mainCanvas == null)
            mainCanvas = GetComponentInChildren<Canvas>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnAnySceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnAnySceneLoaded;
    }

    void OnAnySceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignCameraForCanvas();
    }

    void AssignCameraForCanvas()
    {
        if (mainCanvas == null) return;

        Camera cam = Camera.main;

        if (cam == null)
        {
            Debug.LogWarning("⚠ Không tìm thấy MainCamera trong scene");
            return;
        }

        mainCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        mainCanvas.worldCamera = cam;
        mainCanvas.planeDistance = 1f;
    }

    public void LoadMainGame()
    {
        StartCoroutine(LoadMainGameAsync());
    }

    private IEnumerator LoadMainGameAsync()
    {
        SceneManager.sceneLoaded += OnMainGameLoaded;

        AsyncOperation operation = SceneManager.LoadSceneAsync("MainGame");
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        operation.allowSceneActivation = true;
    }

    private void OnMainGameLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "MainGame") return;

        foreach (GameObject ui in uiGameplay)
            if (ui != null) ui.SetActive(true);

        foreach (GameObject ui in uiMenu)
            if (ui != null) ui.SetActive(false);

        SceneManager.sceneLoaded -= OnMainGameLoaded;
    }

    public void LoadMenu()
    {
        StartCoroutine(LoadMenuAsync());
    }

    private IEnumerator LoadMenuAsync()
    {
        SceneManager.sceneLoaded += OnMenuLoaded;

        AsyncOperation operation = SceneManager.LoadSceneAsync("Menu");
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        operation.allowSceneActivation = true;
    }

    private void OnMenuLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Menu") return;

        foreach (GameObject ui in uiMenu)
            if (ui != null) ui.SetActive(true);

        foreach (GameObject ui in uiGameplay)
            if (ui != null) ui.SetActive(false);

        SceneManager.sceneLoaded -= OnMenuLoaded;
    }
}
