using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearController : WeaponBase
{
    public int numSpears, pierceNum, speed;
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
        if(curCooldown == tickMaxCD)
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
        for(int i = 0; i < numSpears; i++)
        {
            GameObject tempSpear = Instantiate(weapPrefab, playerObj.rotatingSpawnPoints[i].transform.position, playerObj.rotatingSpawnPoints[i].transform.rotation);
            tempSpear.GetComponentInChildren<SpearLogic>().InitSpear(this, weapMod);
        }
    }

    /*public override void SetSpawnDetails()
    {
        for(int i = 0; i < spawnDetails.Count; i++)
        {    
            //axeParent = Instantiate(weapPrefab);
            if(spawnDetails[i].needsParent)
            {
                //axeParent.gameObject.transform.SetParent(playerObj.spawnPoints[spawnDetails[i].spawnpoint].transform);
                //axeParent.transform.position = axeParent.transform.parent.transform.position;
            }
            
            //axeParent.transform.position = playerObj.spawnPoints[spawnDetails[i].spawnpoint].transform.position;
            //axeParent.GetComponent<FollowObject>().target = playerObj.spawnPoints[spawnDetails[i].spawnpoint].gameObject;
            //axeParent.GetComponentInChildren<AxeParent>().InitAxes(this, rotSpeed, distanceFromPlayer, speedOut);           
        }        
    }*/

    public void SendDamage(EnemyController enemy)
    {
        //round up
        enemy.TakeDamage(Mathf.CeilToInt(damage * playerObj._stats["str"].Value));
    }

    public override void UpgradeWeapon()
    {
        level++;
        UpdateValues();
    }

    public void UpdateValues()
    {
        numSpears = (int)weapUpgrades.UpgradeList.Find(x => x.weapUpType == WeapUpgrade.WeaponUpgrade.AMOUNT).upValues[level - 1];
        tickMaxCD = (int)weapUpgrades.UpgradeList.Find(x => x.weapUpType == WeapUpgrade.WeaponUpgrade.COOLDOWN).upValues[level - 1];
        damage = (int)weapUpgrades.UpgradeList.Find(x => x.weapUpType == WeapUpgrade.WeaponUpgrade.DAMAGE).upValues[level - 1];
        pierceNum = (int)weapUpgrades.UpgradeList.Find(x => x.weapUpType == WeapUpgrade.WeaponUpgrade.PIERCE).upValues[level - 1];
        speed = (int)weapUpgrades.UpgradeList.Find(x => x.weapUpType == WeapUpgrade.WeaponUpgrade.SPEED).upValues[level - 1];
    }
}
