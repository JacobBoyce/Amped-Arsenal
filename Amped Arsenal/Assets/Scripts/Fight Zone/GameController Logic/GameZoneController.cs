using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.IO;
using TMPro;
using UnityEngine.UI;

public class GameZoneController : MonoBehaviour
{
    public static GameZoneController Instance{get; private set;}
    public PlayerController p1;
    public GameObject statsPanel, uiController, upgradePanel, shopPanel;
    public TextMeshProUGUI statsTxt;
    public ShopMenuController shopController;


    public bool isPaused, statsVisible;


    private void Awake() 
    { 
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }


    // Start is called before the first frame update
    void Start()
    {
        statsVisible = false;
        ShowStats(statsVisible);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            statsVisible = statsVisible ? false : true;
            ShowStats(statsVisible);
        }

        statsTxt.text = "HP: " + p1._stats["hp"].Value + " / " + p1._stats["hp"].Max
                        + "\nSTR: " + p1._stats["str"].Value
                        + "\nDEF: " + p1._stats["def"].Value
                        + "\nSPD: " + p1._stats["spd"].Value
                        + "\nLUCK: " + p1._stats["luck"].Value
                        + "\nPULL: " + p1._stats["pull"].Value
                        + "\nXP: " + p1._stats["xp"].Value + " / " + p1._stats["xp"].Max;
    }

    public void ShowStats(bool onoff)
    {
        statsPanel.SetActive(onoff);
    }

    public void PauseGame()
    {
        isPaused = isPaused == true ? false : true;
        if(isPaused == true)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

    }

    public void OpenShop()
    {
        PauseGame();
        uiController.SetActive(false);
        shopPanel.SetActive(true);
        shopController.InitShop();
    }

    public void TurnOffShop()
    {
        PauseGame();
        uiController.SetActive(true);
        shopPanel.SetActive(false);
    }
}
