using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    public static PauseUI Instance { get; private set; }
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button countiueButton;
    [SerializeField] private Button SettingButton;
    [SerializeField] private Button RestartButton;
    [SerializeField] private Button ExitButton;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void ShowPausePanel()
    {
        pausePanel.SetActive(true);
        GameManager.Instance.SetTimeScale(0f);
    }

    public void HidePausePanel()
    {
        pausePanel.SetActive(false);
        GameManager.Instance.SetTimeScale(1f); 
    }
    public void ShowSettingsPanel()
    {
        pausePanel.SetActive(false);
        //SettingUI.Instance.ShowSettingsPanel();
        //SettingUI 끌 때 개임 재개 해야함
    }

}
