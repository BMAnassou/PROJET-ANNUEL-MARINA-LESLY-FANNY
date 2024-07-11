using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Items : MonoBehaviour
{
    private int itemsNb = 0;
    public int newItems = 0;
    private int itemsInScene = 0;
    public int maxItems = 15;
   /* public float itemGenerationInterval = 5f;
    private float elapsedTime = 0f;*/
    public TMP_Text itemText;
    public GameObject itemPrefab; 
    public float itemSpawnInterval = 15f; 
    public Terrain terrain;

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
        while (itemsInScene < maxItems)
        {
            Vector3 randomPosition = GetRandomPositionOnTerrain();
            Instantiate(itemPrefab, randomPosition, Quaternion.identity);
            itemsInScene++;
            Debug.Log("Items generated. Position: " + randomPosition);
        }
    }

    Vector3 GetRandomPositionOnTerrain()
    {
        
        float terrainWidth = terrain.terrainData.size.x;
        float terrainLength = terrain.terrainData.size.z;
        float terrainHeight = terrain.terrainData.size.y;

        
        float randomX = Random.Range(0, terrainWidth);
        float randomZ = Random.Range(0, terrainLength);

        
        float randomY = terrain.SampleHeight(new Vector3(randomX, 0, randomZ)) + terrain.transform.position.y;

        
        return new Vector3(randomX, randomY, randomZ);
    }
}