using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryManager : MonoBehaviour

{
    public GameObject victoryScreen;
    public int NombreEnemy;

    public bool isDeadEnemy;

    //public CoinsManager coinsmanager;

    // Start is called before the first frame update
    void Start()
    {
        NombreEnemy = 0;
        NombreEnemy = GameObject.FindGameObjectsWithTag("Enemy").Length;
        Debug.Log("Initial number of enemies: " + NombreEnemy);
        UpdateEnemyCount();

        //coinsmanager = FindObjectOfType<CoinsManager>();
        
    }


    // Methode pour mettre a jour le nombre d'ennemis restants
    public void UpdateEnemyCount()
    {
       /* if(NombreEnemy <= 0)
        {
            victoryScreen.SetActive(true);
            Debug.Log("Victory! All enemies defeated.");
        }*/
    }
    
    // Methode pour decrementer le nombre d'ennemis
    public void DecrementEnemyCount()
    {
        NombreEnemy --;
        Debug.Log("Enemy destroyed. Remaining enemies: " + NombreEnemy);
        UpdateEnemyCount();
    }

    // Methode pour incrementer le nombre d'ennemi
    public void IncrementEnemyCount()
    {
        NombreEnemy++;
        Debug.Log("New enemy spawned. Total enemies: " + NombreEnemy);
    }

    /*void Update()
    {
        if(isDeadEnemy == true)
        {
            NombreEnemy -= 1;
            Debug.Log("isDeadEnemy");

            isDeadEnemy = false;

        }

        if(NombreEnemy == 0)
        {
            victoryScreen.SetActive(true);
            Debug.Log("Victory! All enemies defeated.");
        }
        
    }*/
}
