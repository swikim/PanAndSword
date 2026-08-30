using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    public static SettingUI Instance { get; private set; }

    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider soundEffectSlider;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Boot 없으니 지금은 여기서 유지
    }

    void Start()
    {
        volumeSlider.value = AudioListener.volume;
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void ShowSettingsPanel()
    {
        settingsPanel.SetActive(true);
    }

    public void HideSettingsPanel()
    {
        settingsPanel.SetActive(false);
        PauseUI.Instance.ShowPausePanel();
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
    }
    public void SetSoundEffectVolume(float value)
    {
        // sound effect volume 조절 로직 구현 필요
    }

    
}