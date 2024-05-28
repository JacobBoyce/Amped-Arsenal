using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleController : WeaponBase
{
    public float aoeRange;
    public float deathTimer;
    public float damageTick;

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

        GameObject tempCandle = Instantiate(weapPrefab, playerObj.spawnPoints[spawnDetails[0].spawnpoint].transform.position + new Vector3(0,1,0), playerObj.spawnPoints[spawnDetails[0].spawnpoint].transform.rotation);
        tempCandle.GetComponentInChildren<CandleLogic>().InitCandle(this, weapMod, deathTimer , aoeRange);
        //send weap stats to logic script
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
        deathTimer = (int)weapUpgrades.UpgradeList.Find(x => x.weapUpType == WeapUpgrade.WeaponUpgrade.DURATION).upValues[level - 1];
        tickMaxCD = (int)weapUpgrades.UpgradeList.Find(x => x.weapUpType == WeapUpgrade.WeaponUpgrade.COOLDOWN).upValues[level - 1];
        damage = (int)weapUpgrades.UpgradeList.Find(x => x.weapUpType == WeapUpgrade.WeaponUpgrade.DAMAGE).upValues[level - 1];
        aoeRange = (int)weapUpgrades.UpgradeList.Find(x => x.weapUpType == WeapUpgrade.WeaponUpgrade.RANGE).upValues[level - 1];
    }
}
