using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoldGenerator : MonoBehaviour
{

    
    private int gold = 0;
    public float goldGenerationInterval = 5f;
    public int goldPerInterval = 3;
    private float elapsedTime = 0f;
    public TMP_Text goldText;
    
    void Start()
    {
        elapsedTime = 0f;

        UpdateGoldText();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= goldGenerationInterval)
        {
            gold += goldPerInterval;
            UpdateGoldText();
            elapsedTime = 0f;
        }
    }

    public int GetGold()
    {
        return gold;
    }

    
    void UpdateGoldText()
    {
        if (goldText != null)
        {
            goldText.text = gold.ToString();
        }
    }

}
