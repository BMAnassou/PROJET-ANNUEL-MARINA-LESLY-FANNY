using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraController : MonoBehaviour
{
    public Transform unit;

    private void LateUpdate()
    {
        Vector3 newPosition = unit.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
        
        transform.rotation =Quaternion.Euler(90f, unit.eulerAngles.y, 0f);
    }
}
