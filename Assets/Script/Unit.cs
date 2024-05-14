using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float unitHealth;
    public float unitMaxHealth;

    public HealthTracker healthTracker;
    void Start()
    {
        UnitSelectionManager.Instance.allUnitsList.Add(gameObject);
        unitHealth = unitMaxHealth;
    }

   

    private void onDestroy()
    {
        UnitSelectionManager.Instance.allUnitsList.Remove(gameObject);
    }

    private void UpdateHealthUI()
    {
        healthTracker.UpdateSliderValue(unitHealth, unitMaxHealth);
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
