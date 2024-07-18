using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class WaveContent
    {
        [SerializeField][NonReorderable] private GameObject[] enemySpawn;

        public GameObject[] GetEnemySpawnList()
        {
            return enemySpawn;
        }
    }

    [SerializeField][NonReorderable] private WaveContent[] waves;
    public int currentWave = 0;
    public float spawnRange = 10f;
    public List<GameObject> currentEnemy = new List<GameObject>();
    public Enemy enemy;

    void Start()
    {
        if (waves == null || waves.Length == 0)
        {
            Debug.LogWarning("Waves array is not initialized or empty.");
            return;
        }

        foreach (var wave in waves)
        {
            if (wave == null || wave.GetEnemySpawnList() == null)
            {
                Debug.LogWarning("Wave or enemy spawn list is null.");
                return;
            }
        }

        SpawnWave();
    }

    void Update()
    {
        if (currentEnemy.Count == 0)
        {
            Debug.Log("current Enemy == 0, Current wave += 1");
            currentWave++;
            if (currentWave < waves.Length)
            {
                SpawnWave();
            }
        }
    }

    void SpawnWave()
    {
        if (currentWave >= waves.Length)
        {
            Debug.LogWarning("No more waves to spawn.");
            return;
        }

        foreach (GameObject enemyPrefab in waves[currentWave].GetEnemySpawnList())
        {
            if (enemyPrefab == null)
            {
                Debug.LogWarning("Enemy prefab is null.");
                continue;
            }

            Vector3 spawnLocation = FindSpawnLoc();
            GameObject newEnemy = Instantiate(enemyPrefab, spawnLocation, Quaternion.identity);
            currentEnemy.Add(newEnemy);

            enemy = newEnemy.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.SetSpawner(this);
            }
            else
            {
                Debug.LogWarning("Enemy component not found on instantiated enemy prefab!");
            }

            Debug.Log($"Spawned enemy at {spawnLocation}, currentEnemy count: {currentEnemy.Count}");
        }
    }

    Vector3 FindSpawnLoc()
    {
        Vector3 spawnPos;
        int maxAttempts = 10;
        int attempts = 0;

        do
        {
            float xLoc = Random.Range(-spawnRange, spawnRange) + transform.position.x;
            float zLoc = Random.Range(-spawnRange, spawnRange) + transform.position.z;
            float yLoc = transform.position.y;

            spawnPos = new Vector3(xLoc, yLoc, zLoc);

            if (Physics.Raycast(spawnPos, Vector3.down, 5))
            {
                return spawnPos;
            }

            attempts++;
        } while (attempts < maxAttempts);

        Debug.LogWarning("Failed to find a valid spawn location after maximum attempts. Returning default position.");
        return transform.position;
    }

    public void OnEnemyDestroyed(GameObject enemy)
    {
        Debug.Log($"OnEnemyDestroyed called for {enemy}");
        if (currentEnemy.Contains(enemy))
        {
            currentEnemy.Remove(enemy);
            Debug.Log($"Enemy removed from list, currentEnemy count: {currentEnemy.Count}");
        }
        else
        {
            Debug.LogWarning("Attempted to remove enemy that is not in the list.");
        }
    }
}
