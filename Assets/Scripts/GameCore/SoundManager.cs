using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static  SoundManager Instance { get; private set;}
    [Header("BGM")]
    [SerializeField]private AudioSource bgmSource;
    [SerializeField]private AudioClip lobbyBGM;

    [Header("SFX")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip hitSfx;
    [SerializeField] private AudioClip touchSfx;
    [SerializeField] private AudioClip materialGetSfx;
    [SerializeField] private AudioClip dungeonClearSfx;
    [SerializeField] private AudioClip gameoverSfx;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
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
    public void PlayHit()
    {
        sfxSource.PlayOneShot(hitSfx);
    }

    public void PlayMaterialGet()
    {
        sfxSource.PlayOneShot(materialGetSfx);
    }

    public void PlayDungeonClear()
    {
        sfxSource.PlayOneShot(dungeonClearSfx);
    }
    public void PlayTouchSfx()
    {
        sfxSource.PlayOneShot(touchSfx);
    }
    public void PlayGameOver()
    {
        sfxSource.PlayOneShot(gameoverSfx);
    }
}
