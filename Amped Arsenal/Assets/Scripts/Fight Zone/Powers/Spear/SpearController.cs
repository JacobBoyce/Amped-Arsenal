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

        /*if(currentlySpinning)
        {
            if(activeSpinCD < activeSpinCDMax)
            {
                activeSpinCD += Time.deltaTime;
            }
            else if(activeSpinCD >= activeSpinCDMax)
            {
                activeSpinCD = 0;
                curCooldown = 0;
                currentlySpinning = false;
                ////// turn off axes HERE \\\\\\
                axeParent.GetComponent<AxeParent>().TurnOffAxes();
                /*for(int i = 0; i < spawnDetails.Count; i++)
                {
                    axes[i].SetActive(false);
                }

            }
        }*/
        
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
        enemy.TakeDamage(damage * playerObj._stats["str"].Value);
    }

    public override void UpgradeWeapon()
    {
        level++;
        if(level == 2)
        {
            tickMaxCD -= 1;
            damage++;
        }
        else if(level == 3)
        {
            tickMaxCD -= 1;
        }
        else if(level == 4)
        {
            tickMaxCD -= 1;
            damage++;
        }
        else if(level == 5)
        {
            tickMaxCD -= 1;
            damage++;
        }
    }
}
