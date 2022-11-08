using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : WeaponBase
{
    public int numAxes;
    public bool currentlySpinning;
    public float rotSpeed, speedOut, distanceFromPlayer;
    public float activeSpinCD, activeSpinCDMax;
    public float spawnDelay;
    public GameObject axeParent;
    //public List<GameObject> axes = new List<GameObject>();
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

        if(currentlySpinning)
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
                }*/

            }
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
        if(!currentlySpinning)
        {
            //Choose which axes are activated and send to the parent to spawn the axes or turn them on
            axeParent.GetComponent<AxeParent>().UpdateAndActivateAxes(rotSpeed, speedOut, distanceFromPlayer, numAxes);
            currentlySpinning = true;
        }
    }

    public override void SetSpawnDetails()
    {
        for(int i = 0; i < spawnDetails.Count; i++)
        {    
            axeParent = Instantiate(weapPrefab);
            if(spawnDetails[i].needsParent)
            {
                axeParent.gameObject.transform.SetParent(playerObj.spawnPoints[spawnDetails[i].spawnpoint].transform);
                axeParent.transform.position = axeParent.transform.parent.transform.position;
            }
            
            axeParent.transform.position = playerObj.spawnPoints[spawnDetails[i].spawnpoint].transform.position;
            axeParent.GetComponent<FollowObject>().target = playerObj.spawnPoints[spawnDetails[i].spawnpoint].gameObject;
            axeParent.GetComponentInChildren<AxeParent>().InitAxes(this, weapMod, rotSpeed, distanceFromPlayer, speedOut);           
        }

        UpdateValues();
    }

    public void SendDamage(EnemyController enemy)
    {
        enemy.TakeDamage(damage * playerObj._stats["str"].Value);
    }

    public override void UpgradeWeapon()
    {
        level++;
        UpdateValues();
    }

    public void UpdateValues()
    {
        numAxes = (int)weapUpgrades.UpgradeList.Find(x => x.weapUpType == WeapUpgrade.WeaponUpgrade.AMOUNT).upValues[level-1];
        tickMaxCD = (int)weapUpgrades.UpgradeList.Find(x => x.weapUpType == WeapUpgrade.WeaponUpgrade.COOLDOWN).upValues[level-1];
        damage = (int)weapUpgrades.UpgradeList.Find(x => x.weapUpType == WeapUpgrade.WeaponUpgrade.DAMAGE).upValues[level-1];
    }
}
