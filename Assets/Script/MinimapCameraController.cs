using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraController : MonoBehaviour
{
    public Transform unit;

    private void LateUpdate()
    {
        if (unit != null)
        {
            Vector3 newPosition = unit.position;
            newPosition.y = transform.position.y;
            transform.position = newPosition;
            
            transform.rotation = Quaternion.Euler(90f, unit.eulerAngles.y, 0f);
        }
        else
        {
            Debug.LogWarning("MinimapCameraController: The 'unit' reference is null or has been destroyed.");
        }
    }
}
