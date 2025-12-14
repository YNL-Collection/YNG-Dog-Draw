using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LoadingScene : Singleton<LoadingScene>
{
    public List<GameObject> uiGameplay;
    public List<GameObject> uiMenu;

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
