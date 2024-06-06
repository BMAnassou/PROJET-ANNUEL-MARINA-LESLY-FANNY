using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCollisionDetector : MonoBehaviour
{
    public bool isColliding = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Building"))
        {
            isColliding = true;
            Debug.Log("Collision detected: Enter");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Building"))
        {
            isColliding = false;
            Debug.Log("Collision detected: Exit");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Building"))
        {
            isColliding = true;
            Debug.Log("Collision detected: Stay");
        }
    }
}
