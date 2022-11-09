using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSwingController : WeaponBase
{
    //one prefab all references set, when spawned move to proper position deacitvate and activate on cooldown stuff
    public int swingNumber;

    public List<GameObject> swordObjs = new List<GameObject>();
    public List<SwordSwingLogic> swordlogics = new List<SwordSwingLogic>();

    void Start()
    {
        TickSystem.OnSubTick += delegate (object sender, TickSystem.OnTickEventArgs e) 
        {
            curCooldown++;
        };
        SetSpawnDetails();
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
    }
    
    public override void ActivateAbility()
    {
        curCooldown = 0;
        for(int i = 0; i < spawnDetails.Count; i++)
        {
            swordObjs[i].SetActive(true);
            swordlogics[i].TurnOn();
            

            //playerObj.SpawnWeapon(weapPrefab, spawnDetails[i].spawnpoint, spawnDetails[i].needsParent);
            //

        }
    }

    public override void SetSpawnDetails()
    {
        for(int i = 0; i < spawnDetails.Count; i++)
        {
            GameObject tempWeapSpawn = Instantiate(weapPrefab);

            //set parent if it says to
            if(spawnDetails[i].needsParent)
            {
                //set to spawn point
                tempWeapSpawn.gameObject.transform.SetParent(playerObj.spawnPoints[spawnDetails[i].spawnpoint].transform);
            }

            tempWeapSpawn.GetComponent<SwordSwingLogic>().InitSword(this, weapMod, swingNumber);

            swordObjs.Add(tempWeapSpawn);
            swordlogics.Add(tempWeapSpawn.GetComponent<SwordSwingLogic>());
        }
        UpdateValues();
    }

    public void SendDamage(EnemyController enemy)
    {
        enemy.TakeDamage(Mathf.CeilToInt(damage * playerObj._stats["str"].Value));
    }

    public override void UpgradeWeapon()
    {
        level++;
        UpdateValues();
    }

    public void UpdateValues()
    {
        swingNumber = (int)weapUpgrades.UpgradeList.Find(x => x.weapUpType == WeapUpgrade.WeaponUpgrade.AMOUNT).upValues[level - 1];
        tickMaxCD = (int)weapUpgrades.UpgradeList.Find(x => x.weapUpType == WeapUpgrade.WeaponUpgrade.COOLDOWN).upValues[level - 1];
        damage = (int)weapUpgrades.UpgradeList.Find(x => x.weapUpType == WeapUpgrade.WeaponUpgrade.DAMAGE).upValues[level - 1];
    }
}
