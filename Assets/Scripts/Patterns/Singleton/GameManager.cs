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
        if (!isGameActive) return;

        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            GameOver();
        }
    }

    void InitializeGame()
    {
        score = 0;
        timeRemaining = levelTimeLimit;
        isGameActive = true;
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
        EventManager.TriggerEvent("OnLevelComplete", score);
    }

    void GameOver()
    {
        isGameActive = false;
        EventManager.TriggerEvent("OnGameOver", score);
    }

    public void RestartGame()
    {
        isGameActive = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public int GetScore() => score;
    public float GetTimeRemaining() => timeRemaining;
    public bool IsGameActive() => isGameActive;
}