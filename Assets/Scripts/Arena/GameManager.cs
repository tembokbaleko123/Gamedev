using UnityEngine;
using System;

public enum GameState { PreGame, Playing, Won, Lost }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("References")]
    public ArenaManager arenaManager;
    public GameUI gameUI;
    public PlayerAttack playerAttack;  // Reference to ignore first attack click

    private GameState currentState = GameState.PreGame;

    public GameState CurrentState => currentState;

    public event Action<GameState> OnGameStateChanged;

        void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        Debug.Log("[GameManager] Start() called");
        
        if (arenaManager != null)
        {
            Debug.Log("[GameManager] Subscribing to ArenaManager events");
            arenaManager.OnAllWavesCompleted += OnWavesCompleted;
            arenaManager.OnArenaReset += OnArenaReset;
        }
        else
        {
            Debug.LogWarning("[GameManager] ArenaManager is NULL!");
        }

        if (gameUI != null)
        {
            Debug.Log("[GameManager] Subscribing to GameUI events");
            gameUI.OnStartGame += StartGame;
            gameUI.OnRestartGame += RestartGame;
        }
        else
        {
            Debug.LogWarning("[GameManager] GameUI is NULL!");
        }

        currentState = GameState.PreGame;
        
        // PreGame: cursor unlocked for UI
        Time.timeScale = 1f;
        if (FollowCamera.Instance != null)
            FollowCamera.Instance.LockCursorForGameplay(false);
        
        if (gameUI != null)
        {
            gameUI.HideAll();
            gameUI.ShowStartScreen();
        }
        Debug.Log($"[GameManager] Start complete. Current state: {currentState}, timeScale: {Time.timeScale}");
    }

    void OnDestroy()
    {
        if (arenaManager != null)
        {
            arenaManager.OnAllWavesCompleted -= OnWavesCompleted;
            arenaManager.OnArenaReset -= OnArenaReset;
        }

        if (gameUI != null)
        {
            gameUI.OnStartGame -= StartGame;
            gameUI.OnRestartGame -= RestartGame;
        }
    }

    void StartGame()
    {
        Debug.Log("[GameManager] StartGame() CALLED!");
        
        // FULL reset of attack state when game starts (player might have attacked during PreGame)
        if (playerAttack != null)
        {
            playerAttack.ResetAttackState();
            Debug.Log("[GameManager] PlayerAttack state fully reset");
        }
        else
        {
            Debug.LogWarning("[GameManager] playerAttack is NULL! Assign in Inspector.");
        }
        
        if (arenaManager != null)
        {
            Debug.Log("[GameManager] Calling arenaManager.StartArena()");
            arenaManager.StartArena();
        }
        else
        {
            Debug.LogWarning("[GameManager] arenaManager is NULL!");
        }
        
        Debug.Log("[GameManager] Calling SetState(Playing)");
        SetState(GameState.Playing);
        
        // Update UI wave info AFTER SetState so it doesn't get overwritten
        if (gameUI != null)
        {
            int totalWaves = arenaManager != null ? arenaManager.TotalWaves : 0;
            Debug.Log($"[GameManager] Updating UI - Wave: 1/{totalWaves}");
            gameUI.ShowGameplayUI();
            gameUI.UpdateWaveInfo(1, totalWaves);
            // Don't call UpdateEnemiesRemaining(0) here - let SpawnWaveCoroutine update it
        }
        
        Debug.Log($"[GameManager] After SetState: timeScale={Time.timeScale}, state={currentState}");
    }

    void RestartGame()
    {
        Debug.Log("[GameManager] RestartGame() called");
        
        // Reset arena (clears enemies, resets wave index, teleports player back to spawn)
        if (arenaManager != null)
        {
            arenaManager.ResetArena();
            Debug.Log("[GameManager] Arena reset + Player teleported to spawn");
        }
        
        // Show start screen and go to PreGame state
        if (gameUI != null)
        {
            gameUI.HideAll();
            gameUI.ShowStartScreen();
        }
        
        SetState(GameState.PreGame);
        Debug.Log("[GameManager] Restart complete - back to PreGame");
    }

    void OnWavesCompleted()
    {
        Debug.Log("[GameManager] OnWavesCompleted() - setting to Won");
        SetState(GameState.Won);
    }

    void OnArenaReset()
    {
        SetState(GameState.PreGame);
    }

    public void OnPlayerDeath()
    {
        Debug.Log($"[GameManager] OnPlayerDeath() called! currentState={currentState}");
        if (currentState == GameState.Playing)
        {
            Debug.Log("[GameManager] Calling SetState(Lost)");
            SetState(GameState.Lost);
        }
        else
        {
            Debug.Log($"[GameManager] OnPlayerDeath ignored - not in Playing state");
        }
    }

    void SetState(GameState newState)
    {
        if (currentState == newState)
        {
            Debug.Log($"[GameManager] SetState SKIP: sudah di state {newState}");
            return;
        }

        GameState oldState = currentState;
        currentState = newState;
        Debug.Log($"[GameManager] State: {oldState} → {newState}, timeScale: {Time.timeScale} → {(newState == GameState.Playing ? 1 : 0)}");

        switch (newState)
        {
            case GameState.PreGame:
                Time.timeScale = 1f;
                // Unlock cursor untuk UI
                if (FollowCamera.Instance != null)
                    FollowCamera.Instance.LockCursorForGameplay(false);
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                // Lock cursor untuk gameplay
                if (FollowCamera.Instance != null)
                    FollowCamera.Instance.LockCursorForGameplay(true);
                break;
            case GameState.Won:
            case GameState.Lost:
                Time.timeScale = 0f;
                // Unlock cursor untuk UI
                if (FollowCamera.Instance != null)
                    FollowCamera.Instance.LockCursorForGameplay(false);
                break;
        }

        Debug.Log($"[GameManager] After SetState: timeScale = {Time.timeScale}");

        OnGameStateChanged?.Invoke(newState);

        if (gameUI != null)
            UpdateUI(newState);
    }

    void UpdateUI(GameState state)
    {
        Debug.Log($"[GameManager] UpdateUI called with state: {state}, gameUI={gameUI != null}");
        
        if (gameUI == null)
        {
            Debug.LogWarning("[GameManager] gameUI is NULL!");
            return;
        }
        
        switch (state)
        {
            case GameState.PreGame:
                Debug.Log("[GameManager] Showing StartScreen");
                gameUI.HideAll();
                gameUI.ShowStartScreen();
                break;
            case GameState.Playing:
                Debug.Log("[GameManager] Showing GameplayUI");
                gameUI.HideAll();
                gameUI.ShowGameplayUI();
                break;
            case GameState.Won:
                Debug.Log("[GameManager] Showing WinScreen");
                gameUI.HideAll();
                gameUI.ShowWinScreen();
                break;
            case GameState.Lost:
                Debug.Log("[GameManager] Showing GameOverScreen");
                gameUI.HideAll();
                gameUI.ShowGameOverScreen();
                break;
        }
    }
}
