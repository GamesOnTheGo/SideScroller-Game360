using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    public Vector3 spawnPoint = new Vector3(-8f, 0f, 0f);
    public float levelTimeLimit = 120f;

    private int score = 0;
    private float timeRemaining;
    private bool isGameActive = true;
    private bool isPaused = false;

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
        InitializeGame();
    }

    void Update()
    {
        // Debug current state
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("=== DEBUG ===");
            Debug.Log("Time: " + timeRemaining);
            Debug.Log("Active: " + isGameActive);
            Debug.Log("Paused: " + isPaused);
            Debug.Log("TimeScale: " + Time.timeScale);
        }

        // Only update timer when game is active AND not paused
        if (isGameActive && !isPaused)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                GameOver();
            }
        }

        // Restart input
        if (!isGameActive && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    void InitializeGame()
    {
        Debug.Log("=== INITIALIZING GAME ===");

        // Reset all values
        score = 0;
        timeRemaining = levelTimeLimit;
        isGameActive = true;
        isPaused = false;

        // CRITICAL: Force unpause
        Time.timeScale = 1f;

        Debug.Log("Score: " + score);
        Debug.Log("Timer: " + timeRemaining);
        Debug.Log("Active: " + isGameActive);
        Debug.Log("Paused: " + isPaused);
        Debug.Log("TimeScale: " + Time.timeScale);

        EventManager.TriggerEvent("OnGameStart");
    }

    public void AddScore(int points)
    {
        score += points;
        EventManager.TriggerEvent("OnScoreChanged", score);
    }

    public void PlayerDied()
    {
        EventManager.TriggerEvent("OnPlayerDied");
    }

    public void LevelComplete()
    {
        isGameActive = false;
        PauseGame();
        EventManager.TriggerEvent("OnLevelComplete", score);
    }

    void GameOver()
    {
        Debug.Log("💀 GAME OVER");
        isGameActive = false;
        PauseGame();
        EventManager.TriggerEvent("OnGameOver", score);
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        Debug.Log("⏸️ Paused (TimeScale: " + Time.timeScale + ")");
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        Debug.Log("▶️ Resumed (TimeScale: " + Time.timeScale + ")");
    }

    public void RestartGame()
    {
        Debug.Log("=== RESTARTING ===");

        // Clear events
        EventManager.ClearAllEvents();

        // CRITICAL: Reset time scale
        Time.timeScale = 1f;
        isPaused = false;

        Debug.Log("TimeScale reset to: " + Time.timeScale);

        // Reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public int GetScore() => score;
    public float GetTimeRemaining() => timeRemaining;
    public bool IsGameActive() => isGameActive;
    public bool IsPaused() => isPaused;
}