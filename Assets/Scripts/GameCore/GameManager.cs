using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}
    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    void Start()
    {
        
    }

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }
    public void RestartDungeon()
    {
        AdManager.Instance.TryShowInterstitialAd(() =>
        {
            SetTimeScale(1f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
        
    }

    public void GoToLobby()
    {
        AdManager.Instance.TryShowInterstitialAd(() =>
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Lobby");
        });
    }
}
