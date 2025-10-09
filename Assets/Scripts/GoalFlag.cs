using UnityEngine;

public class GoalFlag : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.LevelComplete();
        }
    }
}