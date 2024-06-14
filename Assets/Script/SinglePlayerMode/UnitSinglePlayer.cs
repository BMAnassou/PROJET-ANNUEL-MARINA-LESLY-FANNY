using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSinglePlayer : MonoBehaviour
{
    public float unitHealth;
    public float unitMaxHealth;

    public ChooseElement.Elements unitElement;

    public float UnitMaxHealth
    {
        get => unitMaxHealth;
        set => unitMaxHealth = value;
    }

    public HealthTracker healthTracker;

    void Start()
    {
        UnitSelectionManagerSinglePlayer.Instance.allUnitsList.Add(gameObject);
        unitHealth = unitMaxHealth;

        // Récupérer la sélection de l'élément à partir de PlayerPrefs
        int selectedElementIndex = PlayerPrefs.GetInt("SelectedElement", 0); // 0 est la valeur par défaut
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

    public void AssignElement(ChooseElement.Elements element)
    {
        unitElement = element;
        Debug.Log("Unit element assigned: " + unitElement);
    }
}