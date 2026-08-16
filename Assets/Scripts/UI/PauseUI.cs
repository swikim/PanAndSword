using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button countiueButton;
    [SerializeField] private Button SettingButton;
    [SerializeField] private Button RestartButton;
    [SerializeField] private Button ExitButton;

    public void ShowPausePanel()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        //GameManager.Instance.PauseGame();
    }
    public void HidePausePanel()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        //GameManager.Instance.ResumeGame();
        //SettingUI 끌 때 개임 재개 해야함
    }
    public void ShowSettingsPanel()
    {
        pausePanel.SetActive(false);
        //SettingUI.Instance.ShowSettingsPanel();
    }

}
