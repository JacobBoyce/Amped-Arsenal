using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.IO;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.InputSystem;

public class GameZoneController : MonoBehaviour
{
    public static GameZoneController Instance{get; private set;}
    public GameObject joystickController, upgradeButton;
    public PlayerController p1;
    public GameObject statsPanel, uiController, currencyUI, upgradePanel, shopPanel, chooseWeapApplyEffect;
    public TextMeshProUGUI statsTxt;
    public ShopMenuController shopController;
    public ShopMenuAnimController shopAnimeController;
    public List<GameObject> mainUIComponents = new();
    public List<GameObject> gamePlayUIComponents = new();
    public bool isUpgrading;
    GameObject focusedUI;
    public GameObject notifyGamesOfUpgrade;
    public bool isPaused, statsVisible;
    public TextMeshProUGUI gameTimerUIText;

    public GameObject thingToShoot, whereToShoot;

    public float gameTimer, gameTimerMax;
    public int minutes, seconds;
    public int Min
    {
        get { return minutes; }
        set
        {
            if (minutes == value) return;
            minutes = value;
            if (OnMinutesChanged != null)
                OnMinutesChanged(minutes);
        }
    }

    public delegate void MinutesChangedDelegate(int minutes);
    public event MinutesChangedDelegate OnMinutesChanged;


    public void FocusUI(GameObject focus, bool needPause)
    {
        focusedUI = focus;
        foreach (GameObject go in mainUIComponents)
        {
            if(focus == go)
            {
                go.SetActive(true);
            }
            else
            {
                go.SetActive(false);
            }
        }

        if (needPause == true)
        {
            PauseGame();
        }
    }

    public void ResumeGamePlay()
    {
        //ADD JOYSTICK BACK HERE FOR UI ACTIVATION
        foreach (GameObject go in gamePlayUIComponents)
        {
            focusedUI.SetActive(false);
            go.SetActive(true);
            joystickController.SetActive(true);
        }
        PauseGame();
    }


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
        /*if (Input.GetKeyDown(KeyCode.Escape))
        {
            //turn on upgrade screen and turn off other ui
            if(isUpgrading == false)
            {
                FocusUI(upgradePanel, true);
                upgradePanel.GetComponent<UpgradeMenuController>().PopulateUI();

            }
            else
            {
                //display gameplay ui
                ResumeGamePlay();
            }
            isUpgrading = !isUpgrading;
        }*/

        if (Input.GetKeyDown(KeyCode.I))
        {
            statsVisible = !statsVisible;
            ShowStats(statsVisible);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            
            //GameObject tempObj = Instantiate(thingToShoot, whereToShoot.transform.position, Quaternion.identity);
            //GetComponent<ShootReward>().ShootObject(whereToShoot, tempObj, ShootReward.ShootType.Facing);
            
        }

        statsTxt.text = "HP: " + p1._stats["hp"].Value + " / " + p1._stats["hp"].Max
                        + "\nSTR: " + p1._stats["str"].Value
                        + "\nDEF: " + p1._stats["def"].Value
                        + "\nSPD: " + p1._stats["spd"].Value
                        + "\nLUCK: " + p1._stats["luck"].Value
                        + "\nPULL: " + p1._stats["pull"].Value
                        + "\nXP: " + p1._stats["xp"].Value + " / " + p1._stats["xp"].Max;
    }

    public void OpenUpgrades()
    {
        
        //turn on upgrade screen and turn off other ui
            if(isUpgrading == false)
            {
                FocusUI(upgradePanel, true);
                upgradePanel.GetComponent<UpgradeMenuController>().PopulateUI();
                joystickController.SetActive(false);
            }
            else
            {
                //display gameplay ui
                ResumeGamePlay();
                joystickController.SetActive(true);
            }
            isUpgrading = !isUpgrading;
    }

    public void ShowStats(bool onoff)
    {
        statsPanel.SetActive(onoff);
    }

    public void PauseGame()
    {
        isPaused = !isPaused;
        
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
        p1.GetComponent<ThirdPersonMovement>().movementEnabled = false;
        PauseGame();
        
        //p1.GetComponent<PlayerInput>().defaultActionMap = "UI";
        joystickController.SetActive(false);
        shopPanel.SetActive(true);
        shopController.InitShop();
        upgradeButton.SetActive(false);
        //shopAnimeController.ToggleDots();
    }

    public void TurnOffShop()
    {
        PauseGame();
        p1.GetComponent<ThirdPersonMovement>().movementEnabled = true;
        joystickController.SetActive(true);
        currencyUI.SetActive(true);
        shopPanel.SetActive(false);
        upgradeButton.SetActive(true);
        //shopAnimeController.ToggleDots();
    }

    public void OpenWeapSelectEffect(RelicBase relic)
    {
        PauseGame();
        chooseWeapApplyEffect.SetActive(true);
        joystickController.SetActive(false);
        chooseWeapApplyEffect.GetComponent<ApplyToWeapMenu>().PopulateWeapChoiceList(p1, relic);
    }

    public void CloseWeapSelectEffect()
    {
        PauseGame();
        joystickController.SetActive(true);
        chooseWeapApplyEffect.SetActive(false);
        
    }

    public void ToggleUpgradeNotification(bool toggleVal)
    {
        notifyGamesOfUpgrade.GetComponent<NotifyPlayerOfUpgrade>().ChangeButtonImage(toggleVal);
    }
}
