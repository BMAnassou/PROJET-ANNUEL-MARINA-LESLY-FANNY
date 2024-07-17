using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DropdownManager : MonoBehaviour
{
    public TMP_Dropdown elementDropdown;
    public Image airImage;
    public Image earthImage;
    public Image fireImage;
    public Image waterImage;

    public GameObject airPlayer;
    public GameObject earthPlayer;
    public GameObject firePlayer;
    public GameObject waterPlayer;

    public Button validateButton;

    public enum Elements { Air, Earth, Fire, Water}

    public GameObject AccessMenuFromValidateButton;
    public GameObject PanelElements;

    void Start()
    {
        if (elementDropdown != null)
        {
            elementDropdown.onValueChanged.AddListener(delegate {
                DropdownValueChanged(elementDropdown);
            });

            // Initialiser la dropdown avec des options si ce n'est pas fait via l'inspecteur
            InitializeDropdownOptions();
        } 


        if (validateButton != null)
        {
            validateButton.onClick.AddListener(OnValidateButtonClick);
        }

        // Désactiver toutes les images et le bouton de validation par défaut
        DisableAllImages();
        DisableAllPlayers();

        // Charger le choix précédemment sauvegardé
        int selectedElement = PlayerPrefs.GetInt("SelectedElement", 0);
        setElementPlayer((Elements)selectedElement);
        
    }

    void InitializeDropdownOptions()
    {
        elementDropdown.ClearOptions();
        elementDropdown.AddOptions(new System.Collections.Generic.List<string>{ "Air", "Earth", "Fire", "Water"});

    }

    void DisableAllImages()
    {
        Debug.Log("Désactivation de toutes les images");

        airImage.gameObject.SetActive(false);
        earthImage.gameObject.SetActive(false);
        fireImage.gameObject.SetActive(false);
        waterImage.gameObject.SetActive(false);

    }


    void DropdownValueChanged(TMP_Dropdown change)
    {
        // Désactiver toutes les images
        Debug.Log("Désactivation de toutes les images");
        DisableAllImages();

        // Activer l'image correspondante en fonction de la sélection

        Elements selectedElement = (Elements)change.value;// A REVOIR
        int selectedValue = change.value;
        setElementPlayer(selectedElement);

        PlayerPrefs.SetInt("SelectedElement", selectedValue);
        PlayerPrefs.Save(); // Sauvegarde immédiate dans PlayerPrefs



        switch (selectedElement)
        {
            case Elements.Air:
                Debug.Log("Activation de l'image Air");
                airImage.gameObject.SetActive(true);
                break;
            case Elements.Earth:
                Debug.Log("Activation de l'image Earth");
                earthImage.gameObject.SetActive(true);
                break;
            case Elements.Fire:
                Debug.Log("Activation de l'image Fire");
                fireImage.gameObject.SetActive(true);
                break;
            case Elements.Water:
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
        Debug.Log("Element selected: " + selectedElement);
    }


    void DisableAllPlayers()
    {
        airPlayer.SetActive(false);
        earthPlayer.SetActive(false);
        firePlayer.SetActive(false);
        waterPlayer.SetActive(false);
    }

    void setElementPlayer(Elements selectedElement)
    {
        DisableAllPlayers();
        switch (selectedElement)
        {
            case Elements.Air:
                Debug.Log("Activation du joueur Air");
                airPlayer.SetActive(true);
                break;
            case Elements.Earth:
                Debug.Log("Activation du joueur Terre");
                earthPlayer.SetActive(true);
                break;
            case Elements.Fire:
                Debug.Log("Activation du joueur Feu");
                firePlayer.SetActive(true);
                break;
            case Elements.Water:
                Debug.Log("Activation du joueur Eau");
                waterPlayer.SetActive(true);
                break;
            default:
                Debug.LogWarning("Valeur de dropdown inconnue: " + selectedElement);
                break;
        }
    }




    public void OnValidateButtonClick()
    {
        // Logique de validation ou chargement de la scène de jeu
        int selectedValue = PlayerPrefs.GetInt("SelectedElement", 0);
        Debug.Log("Validation Button Clicked. Element selected: " + (Elements)selectedValue);
        // Charger la scène de jeu par exemple
        enabled = false;
        DontDestroyOnLoad(gameObject);
        AccessMenuFromValidateButton.SetActive(true);
        PanelElements.SetActive(false);
    }
}
