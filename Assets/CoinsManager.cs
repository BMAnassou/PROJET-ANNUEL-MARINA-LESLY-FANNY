using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinsManager : MonoBehaviour
{

    public bool EnemyisDead;
    public int Coins = 0;
    public TextMeshProUGUI coins;
    public TextMeshProUGUI victoryCoins;

    // Start is called before the first frame update
    void Start()
    {
        Coins = LoadAndSaveData.Instance.LoadCoins();// Charger les pieces sauvegardees au demarrage
        UpdateCoinText();// Mettre a jour l'affichage du texte
        
    }

    // Update is called once per frame
    void Update()
    {
        if(EnemyisDead)
        {
            Debug.Log("Tu recois 50 coins");
            Coins += 50;
            UpdateCoinText();
            EnemyisDead = false;
            Debug.Log($"Coins: {Coins}");
        }
    }

    private void UpdateCoinText()
    {
        coins.text = Coins.ToString(); // Mettre a jour le texte avec le nombre de pieces
        victoryCoins.text = Coins.ToString();

        //if (Inventory.instance != null)
        //{
            Inventory.instance.UpdateCoins(Coins);
        //}
    }

    public void UpdateCoins(int amount)
    {
        Coins += amount;// Ajouter le montant specifie aux pieces
        UpdateCoinText();// Mettre a jour l'affichage du texte
        SaveCoins();// Sauvegarder les pieces apres modification
    }

    public void SaveCoins()
    {
        LoadAndSaveData.Instance.SaveCoins(Coins);// Appeler la methode de sauvegarde dans LoadAndSaveData
    
    }
}


