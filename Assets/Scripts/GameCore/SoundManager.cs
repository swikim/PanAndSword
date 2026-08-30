using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static  SoundManager Instance { get; private set;}
    [SerializeField]private AudioSource bgmSource;
    [SerializeField]private AudioClip lobbyBGM;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        if (Instance != this) return;
        PlayBGM(lobbyBGM);
    }
    public void PlayBGM(AudioClip clip)
    {
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }
}
