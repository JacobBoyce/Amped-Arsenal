using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearController : WeaponBase
{
    public int pierceNum, speed, range;
    //public List<GameObject> axes = new List<GameObject>();
    void Start()
    {
        TickSystem.OnSubTick += delegate (object sender, TickSystem.OnTickEventArgs e) 
        {
            curCooldown++;
        };
        //SetSpawnDetails();
    }
    public void Update()
    {
        if(curCooldown >= tickMaxCD)
        {
            ActivateAbility();
        }        
    }

    public void OnEquipped()
    {
        TickSystem.OnSubTick += delegate (object sender, TickSystem.OnTickEventArgs e) 
        {
            curCooldown++;
        };
        UpdateValues();
    }
    
    public override void ActivateAbility()
    {
        //spawn object and call its init method 
        curCooldown = 0;
        for(int i = 0; i < playerObj.rotatingSpawnPoints.Count; i++)
        {
            GameObject tempSpear = Instantiate(weapPrefab, playerObj.rotatingSpawnPoints[i].transform.position, playerObj.rotatingSpawnPoints[i].transform.rotation);
            tempSpear.GetComponentInChildren<SpearLogic>().InitSpear(this, weapMod);
        }
    }


    public override void UpgradeWeapon()
    {
        level++;
        UpdateValues();
    }

    //This updates the values of the weapon each time it is activated. so after you upgrade the weapon
    //then it will give it the new stats right before the next time it activates
    public void UpdateValues()
    {
        //numSpears = (int)weapUpgrades.UpgradeList.Find(x => x.weapUpType == WeapUpgrade.WeaponUpgrade.AMOUNT).upValues[level - 1];
        tickMaxCD = (int)weapUpgrades.UpgradeList.Find(x => x.weapUpType == WeapUpgrade.WeaponUpgrade.COOLDOWN).upValues[level - 1];
        damage = (int)weapUpgrades.UpgradeList.Find(x => x.weapUpType == WeapUpgrade.WeaponUpgrade.DAMAGE).upValues[level - 1];
        pierceNum = (int)weapUpgrades.UpgradeList.Find(x => x.weapUpType == WeapUpgrade.WeaponUpgrade.PIERCE).upValues[level - 1];
        range = (int)weapUpgrades.UpgradeList.Find(x => x.weapUpType == WeapUpgrade.WeaponUpgrade.RANGE).upValues[level - 1];
        //speed = (int)weapUpgrades.UpgradeList.Find(x => x.weapUpType == WeapUpgrade.WeaponUpgrade.SPEED).upValues[level - 1];
        
    }
}
