using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void WinGame()
    {
        Debug.Log("You have won the game!");
        // Ajoutez votre logique de victoire ici (par exemple, afficher un écran de victoire)
    }

    public void LoseGame()
    {
        Debug.Log("You have lost the game!");
        // Ajoutez votre logique de défaite ici (par exemple, afficher un écran de défaite)
    }
}
