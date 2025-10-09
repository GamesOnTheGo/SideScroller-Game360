using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public TMP_Text stateText;
    public TMP_Text coinText;
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    private int coinCount = 0;

    void Start()
    {
        Debug.Log("✅ UIManager subscribing to events...");

        // Subscribe to events (OBSERVER PATTERN)
        EventManager.Subscribe("OnScoreChanged", UpdateScore);
        EventManager.Subscribe("OnPlayerStateChanged", UpdateStateDisplay);
        EventManager.Subscribe("OnGameOver", ShowGameOver); // GAME OVER
        EventManager.Subscribe("OnLevelComplete", ShowVictory); // VICTORY
        EventManager.Subscribe("OnCoinCollected", UpdateCoinCount);

        // Initialize
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
            Debug.Log("Game Over Panel disabled");
        }

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
            Debug.Log("Victory Panel disabled");
        }

        if (coinText != null)
        {
            coinText.text = "Coins: 0";
        }
    }

    void Update()
    {
        // Update timer
        if (timerText && GameManager.Instance != null)
        {
            float time = GameManager.Instance.GetTimeRemaining();
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    void OnDestroy()
    {
        // Unsubscribe to prevent errors on scene change
        EventManager.Unsubscribe("OnScoreChanged", UpdateScore);
        EventManager.Unsubscribe("OnPlayerStateChanged", UpdateStateDisplay);
        EventManager.Unsubscribe("OnGameOver", ShowGameOver);
        EventManager.Unsubscribe("OnLevelComplete", ShowVictory);
        EventManager.Unsubscribe("OnCoinCollected", UpdateCoinCount);
    }

    void UpdateScore(object scoreData)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + scoreData.ToString();
        }
    }

    void UpdateStateDisplay(object stateData)
    {
        if (stateText != null)
        {
            stateText.text = "State: " + stateData.ToString();
        }
    }

    void UpdateCoinCount(object coinValue)
    {
        coinCount++;
        if (coinText != null)
        {
            coinText.text = "Coins: " + coinCount;
        }
    }

    void ShowGameOver(object finalScore)
    {
        Debug.Log("🔴 SHOWING GAME OVER PANEL");

        // Make sure victory panel is hidden
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }

        // Show game over panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Debug.Log("Game Over Panel activated");
        }
        else
        {
            Debug.LogError("❌ Game Over Panel is NULL!");
        }
    }

    void ShowVictory(object finalScore)
    {
        Debug.Log("🟢 SHOWING VICTORY PANEL");

        // Make sure game over panel is hidden
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // Show victory panel
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            Debug.Log("Victory Panel activated");
        }
        else
        {
            Debug.LogError("❌ Victory Panel is NULL!");
        }
    }

    public void OnRestartButton()
    {
        if (GameManager.Instance != null)
        {
            coinCount = 0;
            GameManager.Instance.RestartGame();
        }
    }

    public int GetCoinCount()
    {
        return coinCount;
    }
}