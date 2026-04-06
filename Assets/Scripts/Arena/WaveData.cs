using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "Game/Wave Data")]
public class WaveData : ScriptableObject
{
    [System.Serializable]
    public class EnemyEntry
    {
        public GameObject enemyPrefab;
        public int count = 1;
    }

    [Header("Wave Settings")]
    public EnemyEntry[] enemies;
    public float timeBetweenSpawns = 1f;
    public float timeBetweenWaves = 3f;
}
