using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelDifficulty
{
    Easy,
    Medium,
    Hard
}

[CreateAssetMenu(fileName ="Level Data", menuName = "Level Data")]
public class LevelDataScriptableObject : ScriptableObject
{
    public LevelDifficulty difficulty;      // Level difficulty.
    public Color cameraBackgroundColor;     // Color to have as a background for the camera.
    public WaveData[] waves;                // All the waves for this level.
}

// Data class for a wave.
[System.Serializable]
public class WaveData
{
    public WaveAsteroidData[] asteroids;
    public WaveEnemyData[] enemies;             // Each type of enemy to spawn this wave.
    public float totalSpawnDuration = 5.0f;     // How long does it take to spawn all of the enemies for this wave?
}

// Data class for each type of enemy to be spawned in a wave.
[System.Serializable]
public class WaveEnemyData
{
    public GameObject enemyPrefab;      // Prefab to spawn.
    public int spawnCount;              // How many of this enemy do we spawn?
}
[System.Serializable]
public class WaveAsteroidData
{
    public GameObject asteroidPrefab;
    public int spawnCount;
}