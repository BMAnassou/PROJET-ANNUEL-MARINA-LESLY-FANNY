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
        if (validateButton != null)
        {
            validateButton.onClick.AddListener(OnValidateButtonClick);
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
        Debug.Log("Désactivation de toutes les images");
        airImage.gameObject.SetActive(false);
        earthImage.gameObject.SetActive(false);
        fireImage.gameObject.SetActive(false);
        waterImage.gameObject.SetActive(false);

        // Activer l'image correspondante en fonction de la sélection
        switch ((ChooseElement.Elements)change.value)
        {
            case ChooseElement.Elements.Air:
                Debug.Log("Activation de l'image Air");
                airImage.gameObject.SetActive(true);
                break;
            case ChooseElement.Elements.Earth:
                Debug.Log("Activation de l'image Earth");
                earthImage.gameObject.SetActive(true);
                break;
            case ChooseElement.Elements.Fire:
                Debug.Log("Activation de l'image Fire");
                fireImage.gameObject.SetActive(true);
                break;
            case ChooseElement.Elements.Water:
                Debug.Log("Activation de l'image Water - Avant");
                waterImage.gameObject.SetActive(true);
                Debug.Log("Activation de l'image Water - Après");
                break;
            default:
                Debug.LogWarning("Valeur de dropdown inconnue : " + change.value);
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
