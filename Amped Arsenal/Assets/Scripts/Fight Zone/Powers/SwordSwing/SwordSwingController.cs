using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSwingController : WeaponBase
{
    //one prefab all references set, when spoawned move to proper position deacitvate and activate on cooldown stuff
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

            tempWeapSpawn.GetComponent<SwordSwingLogic>().InitSword(this, swingNumber);

            swordObjs.Add(tempWeapSpawn);
            swordlogics.Add(tempWeapSpawn.GetComponent<SwordSwingLogic>());
        }
    }

    public void SendDamage(EnemyController enemy)
    {
        enemy.TakeDamage(damage * playerObj._stats["str"].Value);
    }

    public override void UpgradeWeapon()
    {
        level++;
        if(level == 2)
        {
            tickMaxCD -= 1;
            damage++;
            swingNumber++;
        }
        else if(level == 3)
        {
            tickMaxCD -= 1;
            damage++;
        }
        else if(level == 4)
        {
            tickMaxCD -= 1;
            damage++;
            swingNumber++;
        }
        else if(level == 5)
        {
            tickMaxCD -= 1;
            damage++;
        }
    }
}
