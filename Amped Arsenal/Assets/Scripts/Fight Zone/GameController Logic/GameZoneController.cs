using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameZoneController : MonoBehaviour
{
    public static GameZoneController Instance{get; private set;}
    public Image fadeImage;
    public GameObject joystickController, upgradeButton;
    public RelicLib relicLibrary;
    public PlayerController p1;
    public float exfilPercentAmount;
    public GameObject statsPanel, uiController, currencyUI, upgradePanel, pauseMenu, shopPanel, chooseWeapApplyEffect;
    public TextMeshProUGUI statsTxt;
    public ShopMenuController shopController;
    public ShopMenuAnimController shopAnimeController;
    public LobbyController lobCont;
    public List<GameObject> mainUIComponents = new();
    public List<GameObject> gamePlayUIComponents = new();
    public bool isUpgrading;
    GameObject focusedUI;
    public GameObject notifyGamesOfUpgrade;
    public bool isPaused, statsVisible;
    public TextMeshProUGUI gameTimerUIText;

    public List<GameObject> lightsToToggle;

    [Header("Fade Stuff"), Space(10)]
    [Range(0.1f, 10f), SerializeField] private float _fadeOutSpeed = 5f;
    [Range(0.1f, 10f), SerializeField] private float _fadeInSpeed = 5f;
    [SerializeField] public Color _fadeOutStartColor;

    public bool IsFadingOut {get; private set;}
    public bool IsFadingIn {get; private set;}

    [Space(10)]
    public GameObject thingToShoot;
    public GameObject whereToShoot;

    public float gameTimer, gameTimerMax;
    public int minutes, seconds;
    public int Min
    {
        get { return minutes; }
        set
        {
            if (minutes == value) return;
            minutes = value;
            OnMinutesChanged?.Invoke(minutes);
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
        p1.inflationAmount = PlayerPrefs.GetInt("Inflation");
        fadeImage.color = Color.black;
        //MainMenuController.Instance._fadeOutStartColor.a = 0f;
        StartFadeIn();
        
    }


    // Start is called before the first frame update
    void Start()
    {
        statsVisible = false;
        ShowStats(statsVisible);

        //Apply base stats from main menu
        //p1._stats["str"].Value += PlayerPrefs.GetInt("Strength");
        
        ToggleFightZoneLights(false);
    }

    // Update is called once per frame
    void Update()
    {
        #region Fade Stuff

            if(IsFadingOut)
        {
            if(fadeImage.color.a < 1f)
            {
                _fadeOutStartColor.a += Time.unscaledDeltaTime * _fadeOutSpeed;
                fadeImage.color = _fadeOutStartColor;
            }
            else
            {
                IsFadingOut = false;

            }
        }

        if(IsFadingIn)
        {
            if(fadeImage.color.a > 0f)
            {
                _fadeOutStartColor.a -= Time.unscaledDeltaTime * _fadeInSpeed;
                fadeImage.color = _fadeOutStartColor;
            }
            else
            {
                IsFadingIn = false;
            }
        }

        #endregion
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
                        + "\nInf: " + PlayerPrefs.GetInt("Inflation")
                        + "\nXP: " + p1._stats["xp"].Value + " / " + p1._stats["xp"].Max;
    }
    
    public void StartFadeOut()
    {
        fadeImage.color = _fadeOutStartColor;
        IsFadingOut = true;
    }

    public void StartFadeIn()
    {
        if(fadeImage.color.a >= 1f)
        {
            fadeImage.color = _fadeOutStartColor;
            IsFadingIn = true;
        }
    }
    public void ToggleFightZoneLights(bool toggle)
    {
        foreach(GameObject go in lightsToToggle)
        {
            go.SetActive(toggle);
        }
    }
    public void OpenUpgrades(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(!isPaused)
            {
                //turn on upgrade screen and turn off other ui
                if(isUpgrading == false)
                {
                    FocusUI(upgradePanel, true);
                    upgradePanel.GetComponent<UpgradeMenuController>().PopulateUI();
                    joystickController.SetActive(false);
                    upgradeButton.SetActive(false);
                    isUpgrading = true;
                }
                else
                {
                    //display gameplay ui
                    ResumeGamePlay();
                    joystickController.SetActive(true);
                    upgradeButton.SetActive(true);
                    isUpgrading = false;
                    p1.CheckIfCanUpgradeWeapons();
                }
            }
            else if(isPaused && isUpgrading)
            {
                //display gameplay ui
                ResumeGamePlay();
                joystickController.SetActive(true);
                upgradeButton.SetActive(true);
                isUpgrading = false;
                p1.CheckIfCanUpgradeWeapons();
            }
        }
    }

    public void OpenPauseMenu(InputAction.CallbackContext context)
    {
        //if button was pressed (this makes it so the method doesnt get called twice, NEEDED)
        if (context.performed)
        {
            if(isUpgrading == false)
            {
                //if game is not paused
                if(!isPaused)
                {
                    FocusUI(pauseMenu, true);
                    pauseMenu.GetComponent<PauseMenuController>().CloseOtherMenus();
                }
                else
                {
                    ResumeGamePlay();
                }
            }            
        }
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
        p1.CheckIfCanUpgradeWeapons();
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

    public void EndGame()
    {
        //end level stuff
        SceneManager.LoadScene("MainMenu");
    }

    public void EndGameChoice(int choice)
    {
        //1 = continue
        if(choice == 1)
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }
        else
        {
            PlayerPrefs.SetInt("Gold", Mathf.RoundToInt(p1._stats["gold"].Value * ((PlayerPrefs.GetInt("Inflation") / 10) + exfilPercentAmount)));
            PlayerPrefs.SetInt("Returned", 1);
            EndGame();
        }
        
    }
}
