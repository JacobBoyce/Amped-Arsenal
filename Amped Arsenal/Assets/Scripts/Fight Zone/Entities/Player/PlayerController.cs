using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class PlayerController : Actor
{
    public GameObject mainController;
    public GameObject equippedWeapsObj;
    public WeaponLib weapLib;
    private GameObject instObj, tempObj;

    public List<GameObject> equippedWeapons = new List<GameObject>();
    public List<GameObject> spawnPoints = new();// List<GameObject>();
    public List<GameObject> rotatingSpawnPoints = new();// List<GameObject>();
    public static PlayerController playerObj;
    public event Action<Stat> UpdateHPBar;


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
        _stats.AddStat("str",      1,2);    // Multiply this by the damage of weapon being used. (Attk > 1)
        _stats.AddStat("def",        1);    // Multiply by damage taken. (0 > Def < 1)
        _stats.AddStat("spd",    10,50);    // Movement speed
        _stats.AddStat("luck",      10);    // How lucky you are to get different upgrades or drops from enemies.
        _stats.AddStat("pull",    4,15);    // How far to pull object from.
        _stats.AddStat("xp",      0,10000); // Xp.
        _stats.AddStat("gold",    25,10000); //Gold
        goldText.text = "Gold: " + _stats["gold"].Value;
        
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
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
    }
    

    public void TakeDamage(float damage)
    {
        Set("hp", _stats["hp"].Value - Mathf.FloorToInt(damage * _stats["def"].Value));
        UpdateHPBar(_stats["hp"]);
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
        xpText.text = "XP: " + _stats["xp"].Value;
    }

    public void AddGold(int goldAmount)
    {
        Set("gold", _stats["gold"].Value + goldAmount);
        goldText.text = "Gold: " + _stats["gold"].Value;
    }

    public void LevelUp()
    {
        //call pause instance for timescale pausing
        //GameZoneController.Instance.PauseGame(true);

        //algorithm for leveling up
        _stats["xp"].IncreaseMaxBy(10);
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
}
