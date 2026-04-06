using UnityEngine;
using System;
using System.Collections.Generic;

public class ArenaManager : MonoBehaviour
{
    public static ArenaManager Instance { get; private set; }

    [Header("Wave Configuration")]
    public WaveData[] waves;
    public Transform[] spawnPoints;

    [Header("Player Reset")]
    public Transform playerSpawnPoint;

    [Header("References")]
    public PlayerHealth playerHealth;

    private int currentWaveIndex = -1;
    private int enemiesRemaining = 0;
    private bool isWaveActive = false;
    private bool isArenaActive = false;

    private List<GameObject> activeEnemies = new List<GameObject>();

    public event Action<int> OnWaveStarted;
    public event Action<int> OnWaveCompleted;
    public event Action OnAllWavesCompleted;
    public event Action OnArenaReset;
    public event Action<int> OnEnemiesRemainingChanged;

    public int CurrentWave => currentWaveIndex + 1;
    public int TotalWaves => waves != null ? waves.Length : 0;
    public bool IsArenaActive => isArenaActive;

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
        if (playerHealth != null)
        {
            playerHealth.OnPlayerDeath += OnPlayerDied;
        }
    }

    void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnPlayerDeath -= OnPlayerDied;
        }
    }

    public void StartArena()
    {
        Debug.Log($"[ArenaManager] StartArena() called. waves={waves?.Length}, spawnPoints={spawnPoints?.Length}");
        
        if (waves == null || waves.Length == 0)
        {
            Debug.LogWarning("[ArenaManager] No waves configured! Assign WaveData in Inspector.");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("[ArenaManager] No spawn points configured! Assign spawn points in Inspector.");
            return;
        }

        isArenaActive = true;
        currentWaveIndex = -1;
        
        Debug.Log("[ArenaManager] Starting first wave...");
        StartNextWave();
    }

    public void ResetArena()
    {
        StopAllCoroutines();
        
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null)
                Destroy(enemy);
        }
        activeEnemies.Clear();

        currentWaveIndex = -1;
        isWaveActive = false;
        isArenaActive = false;
        enemiesRemaining = 0;

        // Reset player position and health
        if (playerHealth != null)
        {
            playerHealth.ResetHealth();
            
            // Teleport player back to spawn point
            if (playerSpawnPoint != null)
            {
                playerHealth.transform.position = playerSpawnPoint.position;
                playerHealth.transform.rotation = playerSpawnPoint.rotation;
                Debug.Log($"[ArenaManager] Player teleported to spawn point: {playerSpawnPoint.position}");
            }
            else
            {
                Debug.LogWarning("[ArenaManager] playerSpawnPoint is NULL! Assign in Inspector.");
            }
        }
        else
        {
            Debug.LogWarning("[ArenaManager] playerHealth is NULL! Assign in Inspector.");
        }

        OnArenaReset?.Invoke();
    }

    void StartNextWave()
    {
        currentWaveIndex++;
        Debug.Log($"[ArenaManager] StartNextWave() - index={currentWaveIndex}/{waves.Length}");

        if (currentWaveIndex >= waves.Length)
        {
            Debug.Log("[ArenaManager] All waves completed!");
            isArenaActive = false;
            OnAllWavesCompleted?.Invoke();
            return;
        }

        isWaveActive = true;
        OnWaveStarted?.Invoke(CurrentWave);

        WaveData wave = waves[currentWaveIndex];
        Debug.Log($"[ArenaManager] Spawning wave with {wave.enemies.Length} enemy types");
        StartCoroutine(SpawnWaveCoroutine(wave));
    }

    System.Collections.IEnumerator SpawnWaveCoroutine(WaveData wave)
    {
        int totalEnemies = 0;
        foreach (var entry in wave.enemies)
        {
            totalEnemies += entry.count;
            Debug.Log($"[ArenaManager] Wave entry: {entry.enemyPrefab?.name} x {entry.count}");
        }
        enemiesRemaining = totalEnemies;
        OnEnemiesRemainingChanged?.Invoke(enemiesRemaining);
        Debug.Log($"[ArenaManager] Total enemies to spawn: {totalEnemies}");

        foreach (var entry in wave.enemies)
        {
            for (int i = 0; i < entry.count; i++)
            {
                Debug.Log($"[ArenaManager] Spawning enemy {i+1}/{entry.count}: {entry.enemyPrefab?.name}");
                SpawnEnemy(entry.enemyPrefab);
                yield return new WaitForSeconds(wave.timeBetweenSpawns);
            }
        }
        
        Debug.Log($"[ArenaManager] All enemies spawned. Waiting for them to die...");
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("[ArenaManager] enemyPrefab is NULL!");
            return;
        }
        
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("[ArenaManager] No spawn points assigned!");
            return;
        }

        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        Debug.Log($"[ArenaManager] Spawning at {spawnPoint.position}");
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        
        Health health = enemy.GetComponent<Health>();
        if (health != null)
        {
            health.OnDamageTaken += OnEnemyDied;
            health.OnDeath += HandleEnemyDeath;
        }
        else
        {
            Debug.LogWarning($"[ArenaManager] Spawned enemy has no Health component!");
        }

        activeEnemies.Add(enemy);
    }

    void OnEnemyDied(int damage)
    {
        Debug.Log($"[ArenaManager] Enemy took {damage} damage");
    }

    void HandleEnemyDeath()
    {
        enemiesRemaining--;
        Debug.Log($"[ArenaManager] Enemy died. Remaining: {enemiesRemaining}");
        OnEnemiesRemainingChanged?.Invoke(enemiesRemaining);

        if (enemiesRemaining <= 0 && isWaveActive)
        {
            isWaveActive = false;
            OnWaveCompleted?.Invoke(CurrentWave);

            WaveData wave = waves[currentWaveIndex];
            float delay = wave.timeBetweenWaves;

            Debug.Log($"[ArenaManager] Wave {CurrentWave} completed! Next wave in {delay}s");
            Invoke(nameof(StartNextWave), delay);
        }
    }

    void OnPlayerDied()
    {
        Debug.Log("[ArenaManager] Player died!");
        isArenaActive = false;
        StopAllCoroutines();
    }

    public int GetEnemiesRemaining()
    {
        return enemiesRemaining;
    }
}
