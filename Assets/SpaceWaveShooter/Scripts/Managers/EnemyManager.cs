using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

/// <summary>
/// Spawns the enemies for a wave.
/// </summary>
public class EnemyManager : MonoBehaviour

{
    public float enemySpawnRadius;      // How far from the center of the map can enemies spawn.
    public int remainingEnemies;        // Enemies remaining in the wave.
    
    // Events
    public UnityEvent onEnemyDeath;     // Called when an enemy dies.


    // Instance
    public static EnemyManager inst;

    void Awake ()
    {
        // Set the instance to this script.
        inst = this;
    }

    #region Subscribing to Events

    void OnEnable ()
    {
        onEnemyDeath.AddListener(OnEnemyDeath);
    }

    void OnDisable ()
    {
        onEnemyDeath.RemoveListener(OnEnemyDeath);
    }

    #endregion

    // Spawns a Asteroid
    public void SpawnAsteroidWave(WaveData data, float waitTime = 0.0f)
    {
        int totalAsteroids = 0;

        // Calculate the total amount of enemies to spawn.
        for (int x = 0; x < data.asteroids.Length; ++x)
            totalAsteroids += data.asteroids[x].spawnCount;

        // Calculate time between enemy spawns.
        float spawnRate = data.totalSpawnDuration / (float)totalAsteroids;

        GameObject[] asteroidsToSpawn = new GameObject[totalAsteroids];

        int count = 0;

        for (int x = 0; x < data.asteroids.Length; ++x)
        {
            for (int y = 0; y < data.asteroids[x].spawnCount; ++y)
            {
                asteroidsToSpawn[count] = data.asteroids[x].asteroidPrefab;
                count++;
            }
        }

        // Randomize the order of the enemies.
        asteroidsToSpawn.Shuffle();

        StartCoroutine(SpawnAsteroids(asteroidsToSpawn, spawnRate, waitTime));
    }


    // Spawns a new enemy wave.
    public void SpawnEnemyWave (WaveData data, float waitTime = 0.0f)
    {
        int totalEnemies = 0;

        // Calculate the total amount of enemies to spawn.
        for(int x = 0; x < data.enemies.Length; ++x)
            totalEnemies += data.enemies[x].spawnCount;

        remainingEnemies = totalEnemies;
        
        // Calculate time between enemy spawns.
        float spawnRate = data.totalSpawnDuration / (float)totalEnemies;

        GameObject[] enemiesToSpawn = new GameObject[totalEnemies];

        int count = 0;

        for(int x = 0; x < data.enemies.Length; ++x)
        {
            for(int y = 0; y < data.enemies[x].spawnCount; ++y)
            {
                enemiesToSpawn[count] = data.enemies[x].enemyPrefab;
                count++;
            }
        }
        
        // Randomize the order of the enemies.
        enemiesToSpawn.Shuffle();

        StartCoroutine(SpawnEnemies(enemiesToSpawn, spawnRate, waitTime));
    }

    // Spawns enemies over time.
    IEnumerator SpawnEnemies (GameObject[] enemies, float spawnRate, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        for(int x = 0; x < enemies.Length; ++x)
        {
            GameObject enemyObj = Pool.inst.Spawn(enemies[x], GetEnemySpawnPos(), Quaternion.identity);
            yield return new WaitForSeconds(spawnRate);
        }
    }

    IEnumerator SpawnAsteroids(GameObject[] asteroids, float spawnRate, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        for (int x = 0; x < asteroids.Length; ++x)
        {
            GameObject enemyObj = Pool.inst.Spawn(asteroids[x], GetAsteroidSpawnPos(), Quaternion.identity);
            yield return new WaitForSeconds(spawnRate);
        }
    }
    // Returns a random spawn position for an enemy.
    Vector3 GetEnemySpawnPos ()
    {
        Vector3 dir = Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.up) * Vector3.right;
        Vector3 spawnPos = dir * enemySpawnRadius;

        return spawnPos;
    }

    // Returns a random spawn position for an enemy.
    Vector3 GetAsteroidSpawnPos()
    {
        Vector3 dir = Quaternion.AngleAxis(Random.Range(-360.0f, 360.0f), Vector3.up) * Vector3.right;
        Vector3 spawnPos = dir * Random.Range(10, 100);

        return spawnPos;
    }
    // Called when an enemy dies.
    // Check to see if the wave is over.
    void OnEnemyDeath ()
    {
        remainingEnemies--;
        GameManager.inst.CheckWaveState();
    }
}

public static class IListExtensions
{
    // Shuffles a list.
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;

        for(var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}