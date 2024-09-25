using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.Linq;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEngine.InputSystem.Utilities;
using Den.Tools;

public class PlayerController : Actor
{
    public GameZoneController mainController;
    public GameObject equippedWeapsObj, equippedRelicsObj;
    public WeaponLib weapLib;
    public RelicLib relLib;
    public EffectLib effectLib;
    public EquippedHUD eHUD;
    private GameObject instObj, tempObj, tempRelicObj;

    public List<GameObject> equippedWeapons = new();
    public List<GameObject> equippedRelics = new();
    public List<GameObject> spawnPoints = new();// List<GameObject>();
    public List<GameObject> rotatingSpawnPoints = new();// List<GameObject>();
    public static PlayerController playerObj;
    public event Action<Stat> UpdateHPBar;
    public event Action OnDamaged;
    public event Action OnHealed;
    public event Action TriggerInteractEvent;
    public GameObject keyboardImg, gamepadImg;
    public bool iskeyboard;
    public bool openShop = false;
    public float inflationAmount = 0;

    public ParticleSystem dropGetPartSys;
    public GameObject intObj;

    [Header("Visual Damage")]
    public MeshRenderer quispySprite;
    public Color damageColor;
    public bool tookDamage;
    public float blinkIntesity, blinkDuration, blinkTimer;
    


    [Header("UI")]
    public TextMeshProUGUI xpText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI interactText;

    public Light aoeLight;
    public QuispyLightAOE aoeLightElse;
    public int lightMax = 60;

    [Header("Audio")]
    public PlayerAudioController audioController;

    //List of equipped weapons List<Weapon>

    public void UpdateBar(Stat stat)
    {
        if(UpdateHPBar != null)
        {
            UpdateHPBar(stat);
        }
    }

    public bool FindWeapon(string weapName)
    {
        foreach(GameObject weap in equippedWeapons)
        {
            if(weap.GetComponent<WeaponBase>().wName.Equals(weapName))
            {
                return true;
            }
        }
        return false;
    }

    public RelicBase FindRelic(string relicName)
    {
        RelicBase tempRelic = null;
        foreach(GameObject relic in equippedRelics)
        {
            if(relic.GetComponent<RelicBase>().relicName.Equals(relicName))
            {
                tempRelic = relic.GetComponent<RelicBase>();
            }
        }
        return tempRelic;
    }

    public void OnEnable()
    {
        GameSceneManager.OnInputModeChanged += ChangeInteractButtonImage;
    }

    public void OnDisable()
    {
        GameSceneManager.OnInputModeChanged -= ChangeInteractButtonImage;
    }

    public void ChangeInteractButtonImage(GameSceneManager.InputMode mode)
    {
        if(mode == GameSceneManager.InputMode.Keyboard)
        {
            iskeyboard = true;
        }
        else
        {
            iskeyboard = false;
        }
    }

    public void ChangeInteractButtonImage()
    {
        if(GameSceneManager.instance._currentInputMode == GameSceneManager.InputMode.Keyboard)
        {
            iskeyboard = true;
            
        }
        else
        {
            iskeyboard = false;
        }
        if(intObj != null)
        {
            ActivateUIForInteract();
        }
    }

    public void Awake()
    {

        playerObj = this;
        _stats = new Stats();
        _stats.AddStat("hp",       1000,1000);    // Max Health
        _stats.AddStat("str",     1,50);    // Multiply this by the damage of weapon being used. (Attk > 1)
        _stats.AddStat("def",        1);    // Multiply by damage taken. (0 > Def < 1)
        _stats.AddStat("spd",    9,50);    // Movement speed
        _stats.AddStat("luck",   65,100);    // How lucky you are to get different upgrades or drops from enemies.
        _stats.AddStat("pull",   5,50);    // How far to pull object from.
        _stats.AddStat("xp",      0,100000); // Xp.
        _stats.AddStat("gold",    0,100000); //Gold
        
        //mod testing
        //_stats["hp"].AddMod("main", .1f, Modifier.ChangeType.PERCENT, true);
        //_stats["str"].AddMod("main", .1f, Modifier.ChangeType.INT, false);
        
        _stats["hp"].IncreaseMaxByAmount(PlayerPrefs.GetInt("HP"),true,false);
        //_stats["hp"].Value = _stats["hp"].Max;
        float tempstr = (float)PlayerPrefs.GetInt("Strength") / 100;
        //Debug.Log((float)PlayerPrefs.GetInt("Strength")/100);
        _stats["str"].Value += tempstr;

        float tempDef = (float)PlayerPrefs.GetInt("Armor") / 100;
        _stats["def"].Value -= tempDef;

        float tempSpd = PlayerPrefs.GetInt("Speed") / 2;
        _stats["spd"].Value += tempSpd;

        _stats["pull"].Value += PlayerPrefs.GetInt("Magnet");

        _stats["luck"].Value = PlayerPrefs.GetInt("Luck");

        aoeLight.range = PlayerPrefs.GetInt("Light");
        aoeLightElse.AdjustMyLight((int)aoeLight.range);

        goldText.text = _stats["gold"].Value.ToString();
        xpText.text = _stats["xp"].Value.ToString();
        interactText.text = "";
        UpdateBar(_stats["hp"]);
    }

    public void Start()
    {
        UpdateBar(_stats["hp"]);

        if(PlayerPrefs.GetInt("FinishedLampLevel") >= 1)
        {
            goldText.text = PlayerPrefs.GetInt("CurrentGold").ToString();
        }
    }

    public void Update()
    {
        if(tookDamage)
        {
            if(!(_stats["hp"].Value <= 0))
            {
                VisualDamage();
            }
        }


        if (Input.GetKeyDown(KeyCode.Z))
        {
            //_stats["hp"].RemoveMod("main");
            //UpdateBar(_stats["hp"]);
            //HealPlayer(1);
            // foreach(GameObject weap in equippedWeapons)
            // {
            //     if(weap.GetComponent<WeaponBase>().wName.Equals("Sword"))
            //     {
            //         //weap.GetComponent<WeaponBase>().AddEffectToWeapon(FindRelic("Poison").);
            //     }
            // }
        }
        #region Cheats 
        
        if (Input.GetKeyDown(KeyCode.Y))
        {
            aoeLight.range += 5;
            aoeLightElse.AdjustMyLight((int)aoeLight.range);
        }

        if(Input.GetKey(KeyCode.X))
        {
            AddXP(10);
        }

        if(Input.GetKey(KeyCode.G))
        {
            AddGold(10);
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            _stats["pull"].Value++;
        }

        if(Input.GetKeyDown(KeyCode.L))
        {
            GameZoneController.Instance.wvController.enemyVisuals = !GameZoneController.Instance.wvController.enemyVisuals;
        }

        // if(Input.GetKeyDown(KeyCode.Z))
        // {
        //     foreach(GameObject go in equippedWeapons)
        //     {
        //         go.GetComponent<WeaponBase>().UpgradeWeapon();
        //     }
        // }
        
        #endregion
       // DetectInputDevice();
        ChangeInteractButtonImage();
        // if (Input.GetKeyDown(KeyCode.E))
        // {
        //     if(openShop == true)
        //     {
        //         mainController.OpenShop();
        //         //invoke interacted delegate
        //     }
        //     TriggerInteractStuff();
        // }

        // if(Gamepad.current.buttonWest.wasPressedThisFrame)
        // {
        //     if(openShop == true)
        //     {
        //         mainController.OpenShop();
        //         //invoke interacted delegate
        //     }
        //     TriggerInteractStuff();
        // }
    }
    
    public void TriggerInteractStuff(InputAction.CallbackContext context)
    {
        //Debug.Log("pressed");
        if (context.performed)
        {
            TriggerInteractEvent?.Invoke();
        }
    }
    

    
    public void TakeDamage(float damage)
    {
        blinkTimer = blinkDuration;
        tookDamage = true;

        
        float dmg = _stats["def"].Value * damage;
        Set("hp", _stats["hp"].Value - Mathf.CeilToInt(dmg));
        UpdateBar(_stats["hp"]);


        //trigger damage event list
        OnDamaged?.Invoke();
        
        if (_stats["hp"].Value == 0)
        {
            // Debug.Log("GameOver");
            // //---------------------add player prefs inflation to equation here
            // Debug.Log("inflation percentage: " + (inflationAmount / 10) 
            // + "\ncurrent gold: " + _stats["gold"].Value
            // + "\n amount to take back: " + (_stats["gold"].Value * (inflationAmount / 10)));
            if(!audioController.deathSound.isPlaying)
            {
                audioController.StopAllCoroutines();
                audioController.damageSound.Stop();
                audioController.deathSound.time = .5f;
                audioController.deathSound.Play();
            }
            mainController.StartEndGamePhase();
            
            // PlayerPrefs.SetInt("Gold", Mathf.RoundToInt(_stats["gold"].Value * (inflationAmount / 10)));
            // PlayerPrefs.SetInt("Returned", 1);
            // mainController.EndGame();
        }
    }

    public void HealPlayer(float healAmt)
    {
        Set("hp", _stats["hp"].Value + Mathf.FloorToInt(healAmt));
        UpdateBar(_stats["hp"]);

        if(OnHealed != null)
        {
            OnHealed();
        }
    }

    public void AddXP(int xpAmount)
    {
        /*int overflow = 0;
        if((_stats["xp"].Value + xpAmount) >= _stats["xp"].Max)
        {
            overflow = ((int)_stats["xp"].Value + xpAmount) - _stats["xp"].Max;
            Set("xp", 0 + overflow);
            //LevelUp();
        }
        else
        {
            Set("xp", _stats["xp"].Value + xpAmount);
        }
        */
        Set("xp", _stats["xp"].Value + xpAmount);
        xpText.text = _stats["xp"].Value.ToString();
        audioController.PlayXPSound();
        //dropGetPartSys.Play();
        CheckIfCanUpgradeWeapons();
    }

    public void RemoveXP(int xpAmount)
    {
        Set("xp", _stats["xp"].Value - xpAmount);
        xpText.text = _stats["xp"].Value.ToString();

        CheckIfCanUpgradeWeapons();
    }

    public void AddGold(int goldAmount)
    {
        Set("gold", _stats["gold"].Value + goldAmount);
        goldText.text = _stats["gold"].Value.ToString();
        //dropGetPartSys.Play();
        audioController.PlayGoldSound();
    }

    public void RemoveGold(int amt)
    {
        // add bool to perameters for taking all gold that is left.
        Set("gold", _stats["gold"].Value - amt);
        goldText.text = _stats["gold"].Value.ToString();
    }

    public void AddWeaponToCache(string weapName)
    {
        //Get weapon to spawn from library
        instObj = weapLib.FindWeaponFromLib(weapName);

        if(FindWeapon(instObj.GetComponent<WeaponBase>().shopItemInfo.weapName) == false)
        {
            tempObj = Instantiate(instObj, equippedWeapsObj.transform.position, equippedWeapsObj.transform.rotation);
            tempObj.transform.SetParent(equippedWeapsObj.transform);
            tempObj.GetComponent<WeaponBase>().playerObj = this;
            //equip weapon to list
            equippedWeapons.Add(tempObj);
            eHUD.ApplyImage(tempObj.GetComponent<WeaponBase>().shopItemInfo.splashImg);
        }
        else
        {
            Debug.Log("Weapon already exsists");
        }
        //create weapon object under the library
    }

    public void AddRelicToCache(RelicBase relic)
    {
        //this takes the physical object on the map and adds it to the equipped relic list
        tempRelicObj = relic.gameObject;
        tempRelicObj.transform.SetParent(equippedRelicsObj.transform);
        tempRelicObj.transform.localPosition = Vector3.zero;
        

        //apply relic
        //open up Choice UI thru main controller
        //Then after choice has been made send the name of the weapon to the relic so it can be applied
        if(tempRelicObj.GetComponent<RelicBase>().rType == RelicBase.RelicType.ONHIT || tempRelicObj.GetComponent<RelicBase>().rType == RelicBase.RelicType.ONKILL)
        {
            mainController.OpenWeapSelectEffect(relic);
        }
        else
        {
            tempRelicObj.GetComponent<RelicBase>().ApplyRelic(this);
        }
        equippedRelics.Add(tempRelicObj);
        //relic.ApplyRelic(this, weapname);
    }

    public void MovePlayerToField(GameObject moveToPos, bool toggleWeaponsAndRelics)
    {
        transform.position = moveToPos.transform.position;

        ToggleEquipables(toggleWeaponsAndRelics);
    }

    public void ToggleEquipables(bool toggle)
    {
        if(toggle)
        {
            //activate
            equippedWeapsObj.SetActive(true);
            equippedRelicsObj.SetActive(true);
        }
        else
        {
            //deactivate
            equippedWeapsObj.SetActive(false);
            equippedRelicsObj.SetActive(false);
        }
    }

    #region action button

    public void OpenShop(bool openFlag)
    {
        openShop = openFlag;
    }

    #endregion

    public void CheckIfCanUpgradeWeapons()
    {
        int counter = 0;
        foreach(GameObject go in equippedWeapons)
        {
            WeaponBase weap = go.GetComponent<WeaponBase>();
            if (weap != null && !weap.IsMaxLvl())
            {
                //HERE
                if (weap.weapUpgrades.costValues[weap.level] <= _stats["xp"].Value)
                {
                    counter++;
                }
            }
        }

        if(counter > 0)
        {
            mainController.ToggleUpgradeNotification(true);
        }
        else
        {
            mainController.ToggleUpgradeNotification(false);
        }
        //if all equipped weapons upgrade cost is less than current xp turn on notify
        
    }

    public void VisualDamage()
    {
        if(blinkTimer > 0)
        {
            blinkTimer -= Time.deltaTime;
            float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
            float intensity = (lerp * blinkIntesity) + 1.0f;
            if(quispySprite != null)
            {
                quispySprite.material.color = Color.white * intensity;
            }
        }
        else
        {
            tookDamage = false;
            //curBlinkIntensity = blinkIntesity;
            //curDamageColor = baseDamageColor;
        }
    }

    public void ActivateUIForInteract()
    {
        if(iskeyboard)
        {
            keyboardImg.SetActive(true);
            gamepadImg.SetActive(false);
        }
        else
        {
            keyboardImg.SetActive(false);
            gamepadImg.SetActive(true);
        }
    }

    public void DeactivateUIForInteract()
    {
        keyboardImg.SetActive(false);
        gamepadImg.SetActive(false);
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Interactable")
        {
            //turn on UI
            ActivateUIForInteract();
            //interactText.text = "!";
            intObj = other.gameObject;
            intObj.GetComponent<InteractableObject>()?.InRange(this); 
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.tag == "Interactable")
        {
            //turn on UI
            DeactivateUIForInteract();
            //interactText.text = "";
            intObj.GetComponent<InteractableObject>()?.NotInRange(this);
            intObj = null; 
        }
    }

    public void CleanInteractableObj()
    {
        DeactivateUIForInteract();
        //interactText.text = "";
        intObj?.GetComponent<InteractableObject>()?.NotInRange(this);
        intObj = null; 
    }

    public void ReactivateInteractableAfterMenuClose()
    {
        if(intObj != null)
        {
            intObj.GetComponent<InteractableObject>().EventTriggered = false;
        }
    }

}