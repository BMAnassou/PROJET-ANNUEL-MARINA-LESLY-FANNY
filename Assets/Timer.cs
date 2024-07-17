using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    private TextMeshProUGUI Time;
    public float time = 20f;
    public bool GameOver;
    public GameObject gameOverUI;

    void OnEnable()
    {
        StopCoroutine(timer());
        Time = GetComponent<TextMeshProUGUI>();
        StartCoroutine(timer());
    }


    void Start()
    {
        StartCoroutine(timer());
         gameOverUI.SetActive(false);



    }

    IEnumerator timer()
    {
        while(time>0){
            time--;
            yield return new WaitForSeconds(1f);
            Time.text = string.Format("{0:0}:{1:00}", Mathf.Floor(time/60),time%60);
        }

        if(time == 0)
        {
            gameOverUI.SetActive(true);
        }
    }




}
