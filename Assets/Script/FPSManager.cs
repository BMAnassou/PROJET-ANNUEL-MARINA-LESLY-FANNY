using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSManager : MonoBehaviour
{
    public enum limits
    {
        noLimit = 0,
        limit30 = 30,
        limit50 = 50, 
        limit120 = 120,
        limit240 = 240,
    }

    public limits limit;

    private void Awake()
    {
        Application.targetFrameRate = (int)limit;
    }
}
