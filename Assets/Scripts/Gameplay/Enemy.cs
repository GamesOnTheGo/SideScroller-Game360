using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int scoreValue = 10;
    public float moveSpeed = 2f;
    private Vector3 startPos;
    private int direction = -1;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // NEW: Don't move when paused
        if (GameManager.Instance != null && GameManager.Instance.IsPaused())
        {
            return;
        }

        // Move enemy
        transform.position += Vector3.right * direction * moveSpeed * Time.deltaTime;

        if (Mathf.Abs(transform.position.x - startPos.x) > 3f)
        {
            direction *= -1;
        }
    }

    public void TakeDamage()
    {
        GameManager.Instance.AddScore(scoreValue);
        EventManager.TriggerEvent("OnEnemyDefeated");
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage();
            }
        }
    }
}