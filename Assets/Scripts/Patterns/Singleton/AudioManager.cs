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
    public AudioClip coinSound;

    void Awake()
    {
        // Simple singleton - one per scene
        if (Instance == null)
        {
            Instance = this;
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

    public void OnPowerUpCollected()
    {
        PlaySFX(powerUpSound);
    }

    public void PlayJumpSound() => PlaySFX(jumpSound);
    public void PlayShootSound() => PlaySFX(shootSound);
    public void PlayCoinSound() => PlaySFX(coinSound);

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}