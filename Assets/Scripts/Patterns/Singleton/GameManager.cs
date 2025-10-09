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
    private bool isPaused = false; // NEW: Pause state

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
        InitializeGame();
    }

    void Update()
    {
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

        // Restart input (works when game is NOT active)
        if (!isGameActive && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    void InitializeGame()
    {
        score = 0;
        timeRemaining = levelTimeLimit;
        isGameActive = true;
        isPaused = false;

        // Make sure game is unpaused
        ResumeGame();

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
        PauseGame(); // NEW: Pause when level complete
        EventManager.TriggerEvent("OnLevelComplete", score);
    }

    void GameOver()
    {
        isGameActive = false;
        PauseGame(); // NEW: Pause when game over
        EventManager.TriggerEvent("OnGameOver", score);
    }

    // NEW: Pause the entire game
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Freezes all time-based updates
        Debug.Log("⏸️ Game Paused");
    }

    // NEW: Resume the game
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Normal time flow
        Debug.Log("▶️ Game Resumed");
    }

    public void RestartGame()
    {
        // Clear all events to prevent errors
        EventManager.ClearAllEvents();

        // Resume game before reloading
        ResumeGame();

        // Reset game state
        isGameActive = true;

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Getters
    public int GetScore() => score;
    public float GetTimeRemaining() => timeRemaining;
    public bool IsGameActive() => isGameActive;
    public bool IsPaused() => isPaused; // NEW
}