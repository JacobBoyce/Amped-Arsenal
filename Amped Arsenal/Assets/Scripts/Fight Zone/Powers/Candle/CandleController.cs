using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleController : WeaponBase
{
    public float aoeRange;
    public float deathTimer;
    public float damageTick;
    public LayerMask _layersToNotSpawnOn; 
    public int totalAttempts, maxTotalAttempts;
    public bool isSpawnPosValid;

    void Start()
    {
        totalAttempts = 0;
        maxTotalAttempts = 200;
        isSpawnPosValid = false;
        TickSystem.OnSubTick += delegate (object sender, TickSystem.OnTickEventArgs e)
        {
            curCooldown++;
        };
        //SetSpawnDetails();
        UpdateValues();
    }
    public void Update()
    {
        if (curCooldown >= tickMaxCD)
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
        curCooldown = 0;
        totalAttempts = 0;
        isSpawnPosValid = false;
        Collider[] colliders;
        while (!isSpawnPosValid && totalAttempts < maxTotalAttempts)
        {
            colliders = Physics.OverlapSphere(playerObj.spawnPoints[spawnDetails[0].spawnpoint].transform.position, 2f);
            foreach (Collider col in colliders)
            {
                if (((1 << col.gameObject.layer) & _layersToNotSpawnOn) != 0)
                {
                    isSpawnPosValid = false;
                    totalAttempts++;
                    break;
                }
                else
                {
                    isSpawnPosValid = true;
                }
            }

            if(isSpawnPosValid)
            {
                GameObject tempCandle = Instantiate(weapPrefab, playerObj.spawnPoints[spawnDetails[0].spawnpoint].transform.position + new Vector3(0,1,0), playerObj.spawnPoints[spawnDetails[0].spawnpoint].transform.rotation);
                tempCandle.GetComponentInChildren<CandleLogic>().InitCandle(this, weapMod, deathTimer , aoeRange);
                break;
            }
        }
    }

    public override void PlayDamageSound()
    {
        damageSound.pitch = Random.Range(1 - pitchMultiplier, 1 + pitchMultiplier);
        damageSound.PlayOneShot(damageSound.clip);
    }


    public override void UpgradeWeapon()
    {
        level++;
        UpdateValues();
    }

    public void UpdateValues()
    {
        deathTimer = (int)weapUpgrades.UpgradeList.Find(x => x.weapUpType == WeapUpgrade.WeaponUpgrade.DURATION).upValues[level];
        tickMaxCD = (int)weapUpgrades.UpgradeList.Find(x => x.weapUpType == WeapUpgrade.WeaponUpgrade.COOLDOWN).upValues[level];
        damage = (int)weapUpgrades.UpgradeList.Find(x => x.weapUpType == WeapUpgrade.WeaponUpgrade.DAMAGE).upValues[level];
        aoeRange = (int)weapUpgrades.UpgradeList.Find(x => x.weapUpType == WeapUpgrade.WeaponUpgrade.RANGE).upValues[level];
    }
}
