using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameUI : MonoBehaviour
{
    [Header("Screens")]
    public GameObject startScreen;
    public GameObject gameplayUI;
    public GameObject gameOverScreen;
    public GameObject winScreen;

    [Header("Gameplay UI")]
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI enemiesText;

    [Header("Game Over UI")]
    public TextMeshProUGUI gameOverTitle;
    public Button restartButton;

    [Header("Win UI")]
    public TextMeshProUGUI winTitle;
    public Button playAgainButton;

    [Header("Start UI")]
    public Button startButton;

    public event Action OnStartGame;
    public event Action OnRestartGame;

    // Public method for UI Button click (bisa dipanggil dari Inspector)
    public void OnStartGamePressed()
    {
        Debug.Log("[GameUI] OnStartGamePressed() CALLED! Invoking event...");
        OnStartGame?.Invoke();
        Debug.Log("[GameUI] OnStartGame event invoked");
    }

    public void OnRestartGamePressed()
    {
        Debug.Log("[GameUI] OnRestartGamePressed() called!");
        OnRestartGame?.Invoke();
    }

    void Awake()
    {
        Debug.Log("[GameUI] Awake()");
        
        if (restartButton != null)
        {
            Debug.Log("[GameUI] RestartButton found, adding listener");
            restartButton.onClick.AddListener(OnRestartGamePressed);
        }
        else
        {
            Debug.LogWarning("[GameUI] RestartButton is NULL!");
        }

        if (playAgainButton != null)
        {
            Debug.Log("[GameUI] PlayAgainButton found, adding listener");
            playAgainButton.onClick.AddListener(OnRestartGamePressed);
        }
        else
        {
            Debug.LogWarning("[GameUI] PlayAgainButton is NULL!");
        }

        if (startButton != null)
        {
            Debug.Log($"[GameUI] StartButton found: {startButton.gameObject.name}, adding listener");
            // Only add listener here - do NOT wire OnStartGamePressed in Inspector to avoid double invoke
            startButton.onClick.AddListener(OnStartGamePressed);
        }
        else
        {
            Debug.LogWarning("[GameUI] StartButton is NULL! UI buttons will not work!");
        }
    }

    void Start()
    {
        Debug.Log("[GameUI] Start()");
        
        // subscribe ke ArenaManager
        if (ArenaManager.Instance != null)
        {
            ArenaManager.Instance.OnWaveStarted += OnWaveStarted;
            ArenaManager.Instance.OnEnemiesRemainingChanged += OnEnemiesRemainingChanged;
        }
        else
        {
            Debug.LogWarning("[GameUI] ArenaManager.Instance is NULL!");
        }

        HideAll();
        ShowStartScreen();
    }

    void OnDestroy()
    {
        if (restartButton != null)
            restartButton.onClick.RemoveListener(OnRestartGamePressed);

        if (playAgainButton != null)
            playAgainButton.onClick.RemoveListener(OnRestartGamePressed);

        if (startButton != null)
            startButton.onClick.RemoveListener(OnStartGamePressed);

        if (ArenaManager.Instance != null)
        {
            ArenaManager.Instance.OnWaveStarted -= OnWaveStarted;
            ArenaManager.Instance.OnEnemiesRemainingChanged -= OnEnemiesRemainingChanged;
        }
    }

    public void HideAll()
    {
        if (startScreen != null) startScreen.SetActive(false);
        if (gameplayUI != null) gameplayUI.SetActive(false);
        if (gameOverScreen != null) gameOverScreen.SetActive(false);
        if (winScreen != null) winScreen.SetActive(false);
    }

    public void ShowStartScreen()
    {
        HideAll();
        if (startScreen != null) startScreen.SetActive(true);
    }

    public void ShowGameplayUI()
    {
        HideAll();
        if (gameplayUI != null)
        {
            gameplayUI.SetActive(true);
        }
        else
        {
            Debug.LogWarning("[GameUI] GameplayUI is NULL! Assign in Inspector.");
        }
        UpdateWaveInfo(0, 0);
    }

    public void ShowGameOverScreen()
    {
        Debug.Log("[GameUI] ShowGameOverScreen() called");
        HideAll();
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
            Debug.Log($"[GameUI] GameOverScreen activated: {gameOverScreen.name}");
        }
        else
        {
            Debug.LogWarning("[GameUI] GameOverScreen is NULL!");
        }
    }

    public void ShowWinScreen()
    {
        Debug.Log("[GameUI] ShowWinScreen() called");
        HideAll();
        if (winScreen != null)
        {
            winScreen.SetActive(true);
            Debug.Log($"[GameUI] WinScreen activated: {winScreen.name}");
        }
        else
        {
            Debug.LogWarning("[GameUI] WinScreen is NULL!");
        }
    }

    public void UpdateWaveInfo(int currentWave, int totalWaves)
    {
        if (waveText != null)
        {
            waveText.text = $"Wave {currentWave} / {totalWaves}";
            Debug.Log($"[GameUI] UpdateWaveInfo: Wave {currentWave} / {totalWaves}");
        }
        else
        {
            Debug.LogWarning("[GameUI] waveText is NULL! Assign in Inspector.");
        }
    }

    public void UpdateEnemiesRemaining(int count)
    {
        if (enemiesText != null)
        {
            enemiesText.text = $"Enemies: {count}";
        }
    }

    public void OnWaveStarted(int waveNumber)
    {
        if (ArenaManager.Instance != null)
        {
            UpdateWaveInfo(waveNumber, ArenaManager.Instance.TotalWaves);
        }
    }

    public void OnEnemiesRemainingChanged(int count)
    {
        UpdateEnemiesRemaining(count);
    }
}
