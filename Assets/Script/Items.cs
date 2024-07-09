using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Items : MonoBehaviour
{
    private int itemsNb = 0;
    public int newItems = 0;
   /* public float itemGenerationInterval = 5f;
    private float elapsedTime = 0f;*/
    public TMP_Text itemText;
    public GameObject itemPrefab; // Prefab for the item to be generated
    public float itemSpawnInterval = 15f; // Interval for spawning items in the scene
    public Terrain terrain; // Reference to the terrain

    void Start()
    {
        //elapsedTime = 0f;
        UpdateItemText();
        
        InvokeRepeating(nameof(GenerateItems), itemSpawnInterval, itemSpawnInterval);
    }

    void Update()
    {
        //elapsedTime += Time.deltaTime;

        if (newItems > 0)
        {
            itemsNb += newItems;
            UpdateItemText();
            newItems = 0;
        }
    }

    public int GetItem()
    {
        return itemsNb;
    }

    void UpdateItemText()
    {
        if (itemText != null)
        {
            itemText.text = itemsNb.ToString();
        }
    }

    /*public void TakeItems(int amount)
    {
        if (itemsNb >= amount)
        {
            itemsNb -= amount;
            UpdateItemText();
            Debug.Log(amount + " items taken. Remaining items: " + itemsNb);
        }
        else
        {
            Debug.Log("Not enough items to take. Available items: " + itemsNb);
        }
    }*/

    public void GenerateItems()
    {
        for (int i = 0; i < itemSpawnInterval; i++)
        {
            // Generate a random position on the terrain
            Vector3 randomPosition = GetRandomPositionOnTerrain();
            // Instantiate the item prefab at the random position
            Instantiate(itemPrefab, randomPosition, Quaternion.identity);
            Debug.Log("Items génerés : " + itemSpawnInterval + "Position : " + randomPosition);
        }
    }

    Vector3 GetRandomPositionOnTerrain()
    {
        // Get the terrain dimensions
        float terrainWidth = terrain.terrainData.size.x;
        float terrainLength = terrain.terrainData.size.z;
        float terrainHeight = terrain.terrainData.size.y;

        // Generate random x and z coordinates
        float randomX = Random.Range(0, terrainWidth);
        float randomZ = Random.Range(0, terrainLength);

        // Get the terrain height at the random coordinates
        float randomY = terrain.SampleHeight(new Vector3(randomX, 0, randomZ)) + terrain.transform.position.y;

        // Return the random position
        return new Vector3(randomX, randomY, randomZ);
    }
}
