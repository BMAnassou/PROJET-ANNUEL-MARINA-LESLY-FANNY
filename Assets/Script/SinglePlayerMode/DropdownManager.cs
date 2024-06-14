using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropdownManager : MonoBehaviour
{
    public TMP_Dropdown elementDropdown;
    public Image airImage;
    public Image earthImage;
    public Image fireImage;
    public Image waterImage;
    public Button validateButton;

    void Start()
    {
        if (elementDropdown != null)
        {
            elementDropdown.onValueChanged.AddListener(delegate {
                DropdownValueChanged(elementDropdown);
            });
        }
        // Désactiver toutes les images et le bouton de validation par défaut
        airImage.gameObject.SetActive(false);
        earthImage.gameObject.SetActive(false);
        fireImage.gameObject.SetActive(false);
        waterImage.gameObject.SetActive(false);
        
    }

    void DropdownValueChanged(TMP_Dropdown change)
    {
        // Désactiver toutes les images
        airImage.gameObject.SetActive(false);
        earthImage.gameObject.SetActive(false);
        fireImage.gameObject.SetActive(false);
        waterImage.gameObject.SetActive(false);

        // Activer l'image correspondante en fonction de la sélection
        switch ((ChooseElement.Elements)change.value)
        {
            case ChooseElement.Elements.Air:
                airImage.gameObject.SetActive(true);
                break;
            case ChooseElement.Elements.Earth:
                earthImage.gameObject.SetActive(true);
                break;
            case ChooseElement.Elements.Fire:
                fireImage.gameObject.SetActive(true);
                break;
            case ChooseElement.Elements.Water:
                waterImage.gameObject.SetActive(true);
                break;
        }

        

        // Stocker la sélection dans PlayerPrefs
        PlayerPrefs.SetInt("SelectedElement", change.value);
        Debug.Log("Element selected: " + (ChooseElement.Elements)change.value);
    }

    public void OnValidateButtonClick()
    {
        // Logique de validation ou chargement de la scène de jeu
        Debug.Log("Validation Button Clicked. Element selected: " + (ChooseElement.Elements)PlayerPrefs.GetInt("SelectedElement", 0));
        // Charger la scène de jeu par exemple
        // SceneManager.LoadScene("GameScene"); // Remplacez "GameScene" par le nom de votre scène de jeu
    }
}
