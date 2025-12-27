using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupManager : Singleton<PopupManager>
{
    [Header("Popup Prefabs")]
    public GameObject popup_Setting;
    public GameObject popup_Win;

    [Header("Popup Parent (Canvas)")]
    public Canvas popupCanvas; 
    public Transform popupParent;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        if (popupCanvas == null)
            popupCanvas = GetComponentInChildren<Canvas>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignCameraToCanvas();
    }

    void AssignCameraToCanvas()
    {
        if (popupCanvas == null) return;

        Camera mainCam = Camera.main;

        if (mainCam != null)
        {
            popupCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            popupCanvas.worldCamera = mainCam;
            popupCanvas.planeDistance = 1f;

        }
        else
        {
        }
    }

    public void ShowPopup_Setting()
    {
        if (popup_Setting == null) return;

        GameObject popup = Instantiate(popup_Setting, popupParent);

        RectTransform rect = popup.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.localPosition = Vector3.zero; 
            rect.localScale = Vector3.one;    
        }

        popup.SetActive(true);
    }
    public void ShowPopup_Win()
    {
        if (popup_Win == null) return;

        GameObject popup = Instantiate(popup_Win, popupParent);

        RectTransform rect = popup.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.localPosition = Vector3.zero; 
            rect.localScale = Vector3.one;    
        }

        popup.SetActive(true);
    }
}