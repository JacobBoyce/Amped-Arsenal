using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : Actor
{
    public GameObject weapLibObj;
    public WeaponLib weapLib;
    public GameObject instObj, tempObj;

    public List<GameObject> equippedWeapons = new List<GameObject>();
    public List<GameObject> spawnPoints = new List<GameObject>();
    public static PlayerController playerObj;
    public event Action<Stat> UpdateHPBar;

    //List of equipped weapons List<Weapon>

    public void UpdateBar(Stat stat)
    {
        if(UpdateHPBar != null)
        {
            UpdateHPBar(stat);
        }
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
        _stats.AddStat("pull",      50);    // How far to pull object from.
        
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            AddWeaponToCache("SwordSwing");
        }
    }
    

    public void TakeDamage(float damage)
    {
        Set("hp", _stats["hp"].Value - Mathf.FloorToInt(damage * _stats["def"].Value));
        UpdateHPBar(_stats["hp"]);
    }

    public void SpawnWeapon(GameObject weapon, int spawnPoint, bool setParent)
    {
        //check weapon if it needs to be made a child of the spawn point
        GameObject createdWeapon = Instantiate(weapon, spawnPoints[spawnPoint].transform.position, spawnPoints[spawnPoint].transform.rotation);
        if(setParent)
        {
            createdWeapon.transform.SetParent(spawnPoints[spawnPoint].transform);
        }
    }

    public void AddWeaponToCache(string weapName)
    {
        //Get weapon to spawn from library
        instObj = weapLib.FindWeaponFromLib(weapName);
        //create weapon object under the library
        tempObj = Instantiate(instObj, weapLibObj.transform.position, weapLibObj.transform.rotation);
        tempObj.transform.SetParent(weapLibObj.transform);
        //equip weapon to list
        equippedWeapons.Add(tempObj);
    }
}
