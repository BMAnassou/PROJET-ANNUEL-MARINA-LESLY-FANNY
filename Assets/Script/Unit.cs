using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Unit : MonoBehaviour
{
    public float unitHealth;
    public float unitMaxHealth;
    
    public HealthTracker healthTracker;

    void Start()
    {
        Points = 0;
        UnitSelectionManager.Instance.allUnitsList.Add(gameObject);
        unitHealth = unitMaxHealth;
        UpdateHealthUI();
    }

    public int Points { get; private set; }

    public void AddPoints(int points)
    {
        Points += points;
        Debug.Log($"Unit gained {points} points. Total points: {Points}");
    }

    void Update()
    {
    }

    private void onDestroy()
    {
        UnitSelectionManager.Instance.allUnitsList.Remove(gameObject);
    }

    private void UpdateHealthUI()
    {
        if (healthTracker != null)
        {
            healthTracker.UpdateSliderValue(unitHealth, unitMaxHealth);
        }
        if (unitHealth <= 0 )
        {
            Destroy(gameObject);
        }
    }
    internal void TakeDamage(int damageToInflict)
    {
        unitHealth -= damageToInflict;
        UpdateHealthUI();
    }
}
