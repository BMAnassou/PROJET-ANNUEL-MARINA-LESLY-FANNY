using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Unit : MonoBehaviour
{
    public bool EnemyisDead;
    public float unitHealth;
    public float unitMaxHealth;
    

    public HealthTracker healthTracker;
    
    public int Coins;
    public int NombreEnemy;
    public TextMeshProUGUI coins;

    public GameObject victoryUI;
    public TextMeshProUGUI victoryCoins;
    void Start()
    {
        NombreEnemy = 4;
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
        if(EnemyisDead == true)
        {
            Debug.Log("Tu recois 50 coins");
            Coins += 50;
            coins.text = Coins.ToString();
            victoryCoins.text = Coins.ToString();
            EnemyisDead = false;
        }

        if(NombreEnemy == 0)
        {
            victoryUI.SetActive(true);
        }
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
