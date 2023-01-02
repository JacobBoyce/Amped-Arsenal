using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class PlayerController : Actor
{
    public GameObject mainController;
    public GameObject equippedWeapsObj, equippedRelicsObj;
    public WeaponLib weapLib;
    public RelicLib relLib;
    private GameObject instObj, tempObj, tempRelicObj;

    public List<GameObject> equippedWeapons = new List<GameObject>();
    public List<GameObject> equippedRelics = new List<GameObject>();
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


    void Awake()
    {
        playerObj = this;
        _stats = new Stats();
        _stats.AddStat("hp",       100);    // Max Health
        _stats.AddStat("str",      1,50);    // Multiply this by the damage of weapon being used. (Attk > 1)
        _stats.AddStat("def",        1);    // Multiply by damage taken. (0 > Def < 1)
        _stats.AddStat("spd",    10,50);    // Movement speed
        _stats.AddStat("luck",      10);    // How lucky you are to get different upgrades or drops from enemies.
        _stats.AddStat("pull",    15,30);    // How far to pull object from.
        _stats.AddStat("xp",      1000,100000); // Xp.
        _stats.AddStat("gold",    1000,100000); //Gold
        
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
            HealPlayer(1);
        }
        #region Weapon testing
        /*
        if (Input.GetKeyDown(KeyCode.A))
        {
            //AddWeaponToCache("SwordSwing");
            AddWeaponToCache("Axe");
        }

        if(Input.GetKeyDown(KeyCode.W))
        {
            AddWeaponToCache("Sword");
            //AddWeaponToCache("Axe");
        }

        if(Input.GetKeyDown(KeyCode.Q))
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
        */
        #endregion

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(openShop == true)
            {
                mainController.GetComponent<GameZoneController>().OpenShop();
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
    }

    public void RemoveXP(int xpAmount)
    {
        Set("xp", _stats["xp"].Value - xpAmount);
        xpText.text = _stats["xp"].Value.ToString();
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

        //create weapon object under the library
        tempObj = Instantiate(instObj, equippedWeapsObj.transform.position, equippedWeapsObj.transform.rotation);
        tempObj.transform.SetParent(equippedWeapsObj.transform);
        tempObj.GetComponent<WeaponBase>().playerObj = this;

        //equip weapon to list
        equippedWeapons.Add(tempObj);
    }

    public void AddRelicToCache(RelicBase relic)
    {
        //this takes the physical object on the map and adds it to the equipped relic list
        tempRelicObj = relic.gameObject;
        tempRelicObj.transform.SetParent(equippedRelicsObj.transform);
        tempRelicObj.transform.localPosition = Vector3.zero;
        equippedRelics.Add(tempRelicObj);

        //apply relic
        relic.ApplyRelic(this);
    }

    #region action button

    public void OpenShop(bool openFlag)
    {
        openShop = openFlag;
    }

    #endregion
}
