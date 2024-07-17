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
            Vector3 spawnLocation = FindSpawnLoc();
            GameObject newEnemy = Instantiate(enemyPrefab, spawnLocation, Quaternion.identity);
            currentEnemy.Add(newEnemy);

            enemy = newEnemy.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.SetSpawner(this);
            }
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
}