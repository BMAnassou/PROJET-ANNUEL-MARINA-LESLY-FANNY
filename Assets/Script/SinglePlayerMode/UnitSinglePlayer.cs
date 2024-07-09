using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSinglePlayer : MonoBehaviour
{
    public float unitHealth;
    public float unitMaxHealth;

    public ChooseElement.Elements unitElement;
    public AttackController attackController;
    public Items items;

    

    public HealthTracker healthTracker;

    void Start()
    {
        
        UnitSelectionManagerSinglePlayer.Instance.allUnitsList.Add(gameObject);
        switch (unitElement)
        {
            case ChooseElement.Elements.Air:
                unitHealth = unitMaxHealth;
                AirProperties();
                break;
            case ChooseElement.Elements.Earth:
                unitHealth = unitMaxHealth;
                EarthProperties();
                break;
            case ChooseElement.Elements.Fire:
                unitHealth = unitMaxHealth;
                FireProperties();
                break;
            case ChooseElement.Elements.Water:
                WaterProperties();
                break;
        }

       
        int selectedElementIndex = PlayerPrefs.GetInt("SelectedElement", 0); 
        AssignElement((ChooseElement.Elements)selectedElementIndex);

        UpdateHealthUI();
    }

    private void OnDestroy()
    {
        UnitSelectionManagerSinglePlayer.Instance.allUnitsList.Remove(gameObject);
    }

    private void UpdateHealthUI()
    {
        if (healthTracker != null)
        {
            healthTracker.UpdateSliderValue(unitHealth, unitMaxHealth);
        }
        if (unitHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    internal void TakeDamage(int damageToInflict)
    {
        unitHealth -= damageToInflict;
        UpdateHealthUI();
    }

    private void AssignElement(ChooseElement.Elements element)
    {
        unitElement = element;
        Debug.Log("Unit element assigned: " + unitElement);
    }

    private void AirProperties()
    {
        
    }
    
    private void EarthProperties()
    {
        items.itemSpawnInterval = items.itemSpawnInterval * 2;
    }
    
    private void FireProperties()
    {
        attackController.unitDamage = 20;
    }
    
    private void WaterProperties()
    {
        unitMaxHealth = 125;
    }

    private void TakeItems()
    {
        items.newItems += 10;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            TakeItems();
            Destroy(other.gameObject);
        }
    }
}