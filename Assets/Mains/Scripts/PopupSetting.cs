using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupSetting : PopupBase
{
    [Header("Sprites")]
    public Sprite On;
    public Sprite Off;

    [Header("Texts")]
    public TextMeshProUGUI Sound_OnOrOff_Text;
    public TextMeshProUGUI Music_OnOrOff_Text;

    [Header("Buttons")]
    public Button Sound;
    public Button Music;

    private bool isSoundOn;
    private bool isMusicOn;

    private const string SOUND_KEY = "SOUND_ON";
    private const string MUSIC_KEY = "MUSIC_ON";

    private void OnEnable()
    {
        LoadSetting();
        UpdateUI();
    }

    public void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        PlayerPrefs.SetInt(SOUND_KEY, isSoundOn ? 1 : 0);
        PlayerPrefs.Save();

        UpdateUI();
    }

    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn;
        PlayerPrefs.SetInt(MUSIC_KEY, isMusicOn ? 1 : 0);
        PlayerPrefs.Save();

        UpdateUI();
    }

    private void LoadSetting()
    {
        isSoundOn = PlayerPrefs.GetInt(SOUND_KEY, 1) == 1;
        isMusicOn = PlayerPrefs.GetInt(MUSIC_KEY, 1) == 1;
    }

    private void UpdateUI()
    {
        Sound.image.sprite = isSoundOn ? On : Off;
        Sound_OnOrOff_Text.text = isSoundOn ? "ON" : "OFF";
        AudioManager.Instance.SetSound(isSoundOn);

        Music.image.sprite = isMusicOn ? On : Off;
        Music_OnOrOff_Text.text = isMusicOn ? "ON" : "OFF";
        AudioManager.Instance.SetMusic(isMusicOn);
    }

}
