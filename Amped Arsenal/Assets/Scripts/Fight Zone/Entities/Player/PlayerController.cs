using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class PlayerController : Actor
{
    public GameZoneController mainController;
    public GameObject equippedWeapsObj, equippedRelicsObj;
    public WeaponLib weapLib;
    public RelicLib relLib;
    public EffectLib effectLib;
    private GameObject instObj, tempObj, tempRelicObj;

    public List<GameObject> equippedWeapons = new();
    public List<GameObject> equippedRelics = new();
    public List<GameObject> spawnPoints = new();// List<GameObject>();
    public List<GameObject> rotatingSpawnPoints = new();// List<GameObject>();
    public static PlayerController playerObj;
    public event Action<Stat> UpdateHPBar;
    public event Action OnDamaged;
    public event Action OnHealed;
    public bool openShop = false;


    [Header("UI")]
    public TextMeshProUGUI xpText;
    public TextMeshProUGUI goldText;

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


    public void Awake()
    {
        playerObj = this;
        _stats = new Stats();
        _stats.AddStat("hp",       100);    // Max Health
        _stats.AddStat("str",     1,50);    // Multiply this by the damage of weapon being used. (Attk > 1)
        _stats.AddStat("def",        1);    // Multiply by damage taken. (0 > Def < 1)
        _stats.AddStat("spd",    10,50);    // Movement speed
        _stats.AddStat("luck",    9,10);    // How lucky you are to get different upgrades or drops from enemies.
        _stats.AddStat("pull",   5,30);    // How far to pull object from.
        _stats.AddStat("xp",      0,100000); // Xp.
        _stats.AddStat("gold",    50,100000); //Gold
        
        //mod testing
        //_stats["hp"].AddMod("main", .1f, Modifier.ChangeType.PERCENT, true);
        //_stats["str"].AddMod("main", .1f, Modifier.ChangeType.INT, false);
        

        goldText.text = _stats["gold"].Value.ToString();
        xpText.text = _stats["xp"].Value.ToString();
        UpdateBar(_stats["hp"]);
    }

    public void Start()
    {
        UpdateBar(_stats["hp"]);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //_stats["hp"].RemoveMod("main");
            //UpdateBar(_stats["hp"]);
            //HealPlayer(1);
            foreach(GameObject weap in equippedWeapons)
            {
                if(weap.GetComponent<WeaponBase>().wName.Equals("Axe"))
                {
                    //weap.GetComponent<WeaponBase>().AddEffectToWeapon(FindRelic("Poison").);
                }
            }
        }
        #region Weapon testing
        
        if (Input.GetKeyDown(KeyCode.Y))
        {
            //AddWeaponToCache("SwordSwing");
            AddWeaponToCache("Bomb");
        }

        if(Input.GetKeyDown(KeyCode.G))
        {
            AddWeaponToCache("Sword");
            //AddWeaponToCache("Axe");
        }

        if(Input.GetKeyDown(KeyCode.U))
        {
            AddWeaponToCache("Spear");
            //AddWeaponToCache("Axe");
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            foreach(GameObject go in equippedWeapons)
            {
                go.GetComponent<WeaponBase>().UpgradeWeapon();
            }
        }
        
        #endregion

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(openShop == true)
            {
                mainController.OpenShop();
            }
        }
    }


    public void TakeDamage(float damage)
    {
        Set("hp", _stats["hp"].Value - Mathf.FloorToInt(damage * _stats["def"].Value));
        UpdateBar(_stats["hp"]);

        //trigger damage event list
        if(OnDamaged != null)
        {
            OnDamaged();
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

    public void MovePlayerToField(GameObject moveToPos)
    {
        transform.position = moveToPos.transform.position;
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
                if (weap.weapUpgrades.costValues[weap.level - 1] <= _stats["xp"].Value)
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
}