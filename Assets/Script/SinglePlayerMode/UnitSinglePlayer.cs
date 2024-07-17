using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSinglePlayer : MonoBehaviour
{
    public float unitHealth;
    public float unitMaxHealth;
    public VictoryManager victorymanager;
    public CoinsManager coinsmanager;
    public ChooseElement.Elements unitElement;
    public AttackController attackController;
    public Items items;
    public HealthTracker healthTracker;

    void Start()
    {
        UnitSelectionManagerSinglePlayer.Instance.allUnitsList.Add(gameObject);
        unitHealth = unitMaxHealth; // Initialization

        switch (unitElement)
        {
            case ChooseElement.Elements.Air:
                AirProperties();
                break;
            case ChooseElement.Elements.Earth:
                EarthProperties();
                break;
            case ChooseElement.Elements.Fire:
                FireProperties();
                break;
            case ChooseElement.Elements.Water:
                WaterProperties();
                break;
        }

        int selectedElementIndex = PlayerPrefs.GetInt("SelectedElement", 0); 
        AssignElement((ChooseElement.Elements)selectedElementIndex);

        UpdateHealthUI();

        if (gameObject.CompareTag("Enemy"))
        {
            victorymanager?.IncrementEnemyCount();
        }
    }

    private void OnDestroy()
    {
        UnitSelectionManagerSinglePlayer.Instance.allUnitsList.Remove(gameObject);

        if (gameObject.CompareTag("Enemy"))
        {
            victorymanager?.DecrementEnemyCount();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthTracker != null)
        {
            healthTracker.UpdateSliderValue(unitHealth, unitMaxHealth);
        }
        if (unitHealth <= 0)
        {
            HandleDeath();
        }
    }

    internal void TakeDamage(int damageToInflict)
    {
        unitHealth -= damageToInflict;
        UpdateHealthUI();
    }

    private void HandleDeath()
    {
        if (gameObject.CompareTag("Player"))
        {
            Debug.Log("Player died");
        }
        else if (gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy killed");
            if (victorymanager != null)
            {
                victorymanager.isDeadEnemy = true;
            }
            if (coinsmanager != null)
            {
                coinsmanager.EnemyisDead = true;
            }
        }
        Destroy(gameObject);
    }

    private void AssignElement(ChooseElement.Elements element)
    {
        unitElement = element;
        Debug.Log("Unit element assigned: " + unitElement);
    }

    private void AirProperties()
    {
        // Implement specific properties for Air element
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
