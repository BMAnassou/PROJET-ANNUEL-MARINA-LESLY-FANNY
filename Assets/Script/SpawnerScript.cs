using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject unitPrefab;
    public float xOffset = 1.0f;
    private int unitSpawned = 0;
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 spawnPosition = transform.position + Vector3.right * (unitSpawned * xOffset);
            Instantiate(unitPrefab, spawnPosition, Quaternion.identity);
            unitSpawned++;
        }
    }
}
