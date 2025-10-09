using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Collectible Settings")]
    public CollectibleType type = CollectibleType.Coin;
    public int scoreValue = 5;
    public bool rotates = true;
    public float rotationSpeed = 100f;

    [Header("Effects (Optional)")]
    public GameObject collectEffect;
    public AudioClip collectSound;

    void Update()
    {
        // NEW: Don't rotate when paused
        if (GameManager.Instance != null && GameManager.Instance.IsPaused())
        {
            return;
        }

        // Rotate the collectible for visual appeal
        if (rotates)
        {
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // NEW: Can't collect when paused
        if (GameManager.Instance != null && GameManager.Instance.IsPaused())
        {
            return;
        }

        if (collision.CompareTag("Player"))
        {
            Collect();
        }
    }

    void Collect()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreValue);
        }

        EventManager.TriggerEvent("OnCoinCollected", scoreValue);

        if (AudioManager.Instance != null && collectSound != null)
        {
           // AudioManager.Instance.PlaySFX(collectSound);
        }
        else if (AudioManager.Instance != null)
        {
            //AudioManager.Instance.OnPowerUpCollected();
        }

        if (collectEffect != null)
        {
            Instantiate(collectEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}

public enum CollectibleType
{
    Coin,
    Gem,
    PowerUp,
    HealthPack
}