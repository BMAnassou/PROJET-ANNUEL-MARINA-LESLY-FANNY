using UnityEngine;

public class LoadAndSaveData : MonoBehaviour
{
    public static LoadAndSaveData Instance; // Instance singleton pour faciliter l'acces depuis d'autres scripts

    private void Awake()
    {
        if (Instance == null)
        {
            Debug.LogWarning("Il n'y a plus d'une instance de LaodAndSaveData dans la scene");
            Instance = this;
            DontDestroyOnLoad(gameObject); // Conserver cet objet a travers les scenes
        }
        else
        {
            Destroy(gameObject); // Detruire les doublons
        }
    }

    public void SaveCoins(int coinsAmount)
    {
        PlayerPrefs.SetInt("PlayerCoins", coinsAmount);
        PlayerPrefs.Save();
    }

    public int LoadCoins()
    {
        if (PlayerPrefs.HasKey("PlayerCoins"))
        {
            return PlayerPrefs.GetInt("PlayerCoins");
        }
        return 0; // Par defaut, retourne 0 si aucune valeur n'est trouvee
    }

    // Autres methodes de sauvegarde et de chargement pour d'autres donnees d'inventaire si necessaire
}
