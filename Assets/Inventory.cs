using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    public static Inventory instance; // Singleton instance

    public TextMeshProUGUI coins; // Reference au texte des pieces

    private void Awake()
    {
        gameObject.SetActive(false);

        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void UpdateCoins(int amount)
    {
        // Mettre a jour le texte des pieces dans l'UI
        coins.text = amount.ToString();
    }
}
