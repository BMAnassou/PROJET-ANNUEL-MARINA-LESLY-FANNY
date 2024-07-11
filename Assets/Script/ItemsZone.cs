using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsZone : MonoBehaviour
{
    public int itemsToCollect = 10; 
    public float collectionRadius = 5f; 
    public float collectionTime = 3f; 
    public string playerTag = "Player";

    private Items itemsScript; 
    private Transform playerTransform; 
    private bool isPlayerInZone = false;
    private float timeSpentInZone = 0f;
    private SphereCollider sphereCollider;

    void Start()
    {
        // Chercher automatiquement le script Items dans la scène
        itemsScript = FindObjectOfType<Items>();

        if (itemsScript == null)
        {
            Debug.LogError("Items script not found in the scene!");
            return;
        }

        
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);

        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player object with tag " + playerTag + " not found in the scene!");
            return;
        }

        sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = collectionRadius;
    }

    void Update()
    {
        if (itemsScript == null || playerTransform == null)
        {
            return; 
        }

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        Debug.Log("Distance to player: " + distanceToPlayer);

        if (distanceToPlayer <= collectionRadius)
        {
            if (!isPlayerInZone)
            {
                isPlayerInZone = true;
                timeSpentInZone = 0f;
                Debug.Log("Player entered the zone.");
            }

            timeSpentInZone += Time.deltaTime;
            Debug.Log("Time spent in zone: " + timeSpentInZone);

            if (timeSpentInZone >= collectionTime)
            {
                CollectItems();
            }
        }
        else
        {
            if (isPlayerInZone)
            {
                Debug.Log("Player left the zone.");
            }

            isPlayerInZone = false;
            timeSpentInZone = 0f;
        }
    }

    void CollectItems()
    {
        itemsScript.newItems += itemsToCollect;
        Debug.Log(itemsToCollect + " items collected!");
        
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, collectionRadius);
    }
}
