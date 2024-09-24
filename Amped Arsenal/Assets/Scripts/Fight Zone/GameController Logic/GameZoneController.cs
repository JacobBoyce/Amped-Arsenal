using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;

public class GameZoneController : MonoBehaviour
{
    public static GameZoneController Instance{get; private set;}
    public CamFollow camf;
    public WaveController wvController;
    public SelectedSoundMaker soundMaker;
    public Image fadeImage;
    public GameObject joystickController, upgradeButton;
    public RelicLib relicLibrary;
    public PlayerController p1;
    public float exfilPercentAmount;
    public GameObject statsPanel, uiController, currencyUI, upgradePanel, pauseMenu, returnMenu, shopPanel, chooseWeapApplyEffect;
    public ReturnToMainMenuInLobby rtmmilObj;
    public TextMeshProUGUI statsTxt;
    public ShopMenuController shopController;
    public ShopMenuAnimController shopAnimeController;
    public LobbyController lobCont;
    public GameObject EndGameUI;
    public List<GameObject> mainUIComponents = new();
    public List<GameObject> gamePlayUIComponents = new();
    public bool isUpgrading, isEndGame = false, startGoldUICountDown = false, inLobby;
    private float maxGCD = 4f, goldCD = 0;
    public GameObject focusedUI;
    public GameObject notifyGamesOfUpgrade;
    public bool isPaused, statsVisible, isShopping;
    public TextMeshProUGUI gameTimerUIText, endGameGoldText;
    public int endGameGold = 0, endGameGoldHalved = 0;
    public List<GameObject> lightsToToggle;

    public GameObject quispyDPoof, quispyDeathAnim;

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
            //if in lobby dont turn on certain things
            focusedUI.SetActive(false);
            if(go.name != "GameTimeUI")
            {
                go.SetActive(true);
            }
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
        GameSceneManager.instance.musicMaker.AquireSoundMaker();
        
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
                    if(isEndGame)
                    {
                        //play death poof effect
                        quispyDPoof.SetActive(true);
                        quispyDeathAnim.SetActive(true);
                    }
                    else
                    {
                        p1.GetComponent<PlayerInput>().DeactivateInput();
                    }
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
                p1.GetComponent<PlayerInput>().ActivateInput();
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

        #region Show Stats
        if (Input.GetKeyDown(KeyCode.I))
        {
            statsVisible = !statsVisible;
            ShowStats(statsVisible);
        }


        statsTxt.text = "HP: " + p1._stats["hp"].Value + " / " + p1._stats["hp"].Max
                        + "\nSTR: " + p1._stats["str"].Value
                        + "\nDEF: " + p1._stats["def"].Value
                        + "\nSPD: " + p1._stats["spd"].Value
                        + "\nLUCK: " + p1._stats["luck"].Value
                        + "\nPULL: " + p1._stats["pull"].Value
                        + "\nInf: " + PlayerPrefs.GetInt("Inflation")
                        + "\nXP: " + p1._stats["xp"].Value + " / " + p1._stats["xp"].Max;
        #endregion

        #region endgame gold ui countdown
        if(startGoldUICountDown)
        {
            float currentGold;
            if (goldCD < maxGCD)
            {
                goldCD += Time.unscaledDeltaTime;
                float percentageComplete = goldCD / maxGCD;

                // Update current gold based on percentage
                currentGold = Mathf.FloorToInt(endGameGold * (1 - percentageComplete));

                // Ensure currentGold doesn't go below half of endGameGold
                if (currentGold < endGameGoldHalved)
                {
                    currentGold = endGameGoldHalved;
                    startGoldUICountDown = false; // Stop countdown
                }

                // Update the UI text
                endGameGoldText.text = currentGold.ToString();
            }
            else
            {
                currentGold = endGameGoldHalved; // Set currentGold to half when done
                endGameGoldText.text = currentGold.ToString();
                startGoldUICountDown = false; // Ensure countdown stops
            }
            
            
        }
        #endregion
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
        if (context.performed && !isEndGame)
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
        if (context.performed && !isEndGame)
        {
            if(isUpgrading == false)
            {
                //if game is not paused
                if(!isPaused && !pauseMenu.GetComponent<PauseMenuController>().IsPausedMenuUp)
                {
                    FocusUI(pauseMenu, true);
                    pauseMenu.GetComponent<PauseMenuController>().pauseMainMenu.SetActive(true);
                    pauseMenu.GetComponent<PauseMenuController>().CloseOtherMenus();
                    pauseMenu.GetComponent<PauseMenuController>().IsPausedMenuUp = true;
                }
                else
                {
                    if(isShopping)
                    {
                        TurnOffShop();
                        p1.ReactivateInteractableAfterMenuClose();
                    }
                    else if(pauseMenu.GetComponent<PauseMenuController>().IsPausedMenuUp)
                    {
                        pauseMenu.GetComponent<PauseMenuController>().pauseMainMenu.SetActive(false);
                        pauseMenu.GetComponent<PauseMenuController>().CloseOtherMenus();
                        p1.ReactivateInteractableAfterMenuClose();
                        PauseGame();
                        pauseMenu.GetComponent<PauseMenuController>().IsPausedMenuUp = false;   
                    }
                }
            }        
            else
            {
                isUpgrading = false;
                //pauseMenu.GetComponent<PauseMenuController>().CloseOtherMenus();
                ResumeGamePlay();
                //deactivate all menus and turn
            }    
        }
    }

    public void ReturnToMainMenuMenuOpen()
    {
        FocusUI(returnMenu,true);
        p1.GetComponent<ThirdPersonMovement>().movementEnabled = false;
    }

    public void TriggerReturnToMainMenu()
    {
        MainMenuController.Instance._playerGold += (int)p1._stats["gold"].Value;
        MainMenuController.Instance.SaveGoldData();
        GameSceneManager.instance.LoadMainMenu();
    }

    public void ChooseNoToReturnToMainMenu()
    {
        ResumeGamePlay();
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
            // if(p1.audioController.damageSound.isPlaying)
            // {
            //     StartCoroutine(p1.audioController.HurtSFXFade(false));

            // }
        }
        else
        {
            Time.timeScale = 1;
            // if(p1.audioController.beingDamaged)
            // {
            //     StartCoroutine(p1.audioController.HurtSFXFade(true));

            // }
        }

    }

    public void OpenShop()
    {
        if(!isShopping)
        {
            p1.GetComponent<ThirdPersonMovement>().movementEnabled = false;
            PauseGame();
            
            //p1.GetComponent<PlayerInput>().defaultActionMap = "UI";
            joystickController.SetActive(false);
            shopPanel.SetActive(true);
            shopController.InitShop();
            upgradeButton.SetActive(false);
            isShopping = true;
            //shopAnimeController.ToggleDots();
        }
        
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
        isShopping = false;
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

    public void StartEndGamePhase()
    {
        isEndGame = true;
        GameSceneManager.instance.musicMaker.StopMusic();
        
        //turn off UI
        foreach (GameObject go in gamePlayUIComponents)
        {
            go.SetActive(false);
        }
        camf.AdjustToTargetOffset(1.5f);
        StartCoroutine(DecreaseTimeScaleAndVignette(1f));
        //GameSceneManager.instance.LoadMainMenu();
    }

    private IEnumerator DecreaseTimeScaleAndVignette(float duration)
    {
        float startTime = Time.timeScale; // Starting time scale (1)
        float elapsedTime = 0f;
        

        // Get the current vignette intensity
        float startIntensity = GetComponent<GlobalVolumeController>().vignette.intensity.value;

        while (elapsedTime < duration)
        {
            // Interpolate time scale
            Time.timeScale = Mathf.Lerp(startTime, 0f, elapsedTime / duration);
            // Interpolate vignette intensity
            if (GetComponent<GlobalVolumeController>().vignette != null)
            {
                GetComponent<GlobalVolumeController>().vignette.intensity.value = Mathf.Lerp(startIntensity, 1f, elapsedTime / duration);
            }
            elapsedTime += Time.unscaledDeltaTime; // Increase elapsed time
            yield return null; // Wait for the next frame
        }
        StartFadeOut();
        
        // Ensure final values are set
        Time.timeScale = 0f; // Set time scale to 0
        if (GetComponent<GlobalVolumeController>().vignette != null)
        {
            GetComponent<GlobalVolumeController>().vignette.intensity.value = 1f; // Ensure vignette intensity is set to 1
        }
        
    }
    
    public void OpenEndGameUI()
    {
        endGameGold = (int)p1._stats["gold"].Value;
        endGameGoldHalved = Mathf.RoundToInt(endGameGold / 2);

        MainMenuController.Instance._playerGold += endGameGold;
        MainMenuController.Instance.SaveGoldData();
        
        endGameGoldText.text = endGameGold.ToString();
        EndGameUI.SetActive(true);
        startGoldUICountDown = true;
    }
    public void EndGame()
    {
        GameSceneManager.instance.LoadMainMenu();
    }

    // public void EndGameChoice(int choice)
    // {
    //     //1 = continue
    //     if(choice == 1)
    //     {
    //         Debug.Log("end game called");
    //         SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    //     }
    //     else
    //     {
    //         Debug.Log("end game called2");
    //         PlayerPrefs.SetInt("Gold", Mathf.RoundToInt(p1._stats["gold"].Value * ((PlayerPrefs.GetInt("Inflation") / 10) + exfilPercentAmount)));
    //         PlayerPrefs.SetInt("Returned", 1);
    //         EndGame();
    //     }
        
    // }
}
