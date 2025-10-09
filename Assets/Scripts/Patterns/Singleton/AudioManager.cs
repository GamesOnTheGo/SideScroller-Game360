using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Sound Effects")]
    public AudioClip jumpSound;
    public AudioClip shootSound;
    public AudioClip enemyDefeatedSound;
    public AudioClip powerUpSound;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        EventManager.Subscribe("OnEnemyDefeated", OnEnemyDefeated);
        EventManager.Subscribe("OnPowerUpCollected", OnPowerUpCollected);
    }

    void OnDestroy()
    {
        EventManager.Unsubscribe("OnEnemyDefeated", OnEnemyDefeated);
        EventManager.Unsubscribe("OnPowerUpCollected", OnPowerUpCollected);
    }

    void OnEnemyDefeated()
    {
        PlaySFX(enemyDefeatedSound);
    }

    void OnPowerUpCollected()
    {
        PlaySFX(powerUpSound);
    }

    public void PlayJumpSound() => PlaySFX(jumpSound);
    public void PlayShootSound() => PlaySFX(shootSound);

    void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}