using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleController : WeaponBase
{
    public float aoeRange;
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
        if (curCooldown == tickMaxCD)
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

        GameObject tempCandle = Instantiate(weapPrefab, playerObj.spawnPoints[spawnDetails[0].spawnpoint].transform.position + new Vector3(0,1,0), playerObj.spawnPoints[spawnDetails[0].spawnpoint].transform.rotation);
        tempCandle.GetComponentInChildren<CandleLogic>().InitCandle(this, weapMod);
    }


    public void SendDamage(EnemyController enemy)
    {
        enemy.TakeDamage(damage * playerObj._stats["str"].Value);
    }

    public override void UpgradeWeapon()
    {
        level++;
        if (level == 2)
        {
            tickMaxCD -= 1;
            damage++;
        }
        else if (level == 3)
        {
            tickMaxCD -= 1;
        }
        else if (level == 4)
        {
            tickMaxCD -= 1;
            damage++;
        }
        else if (level == 5)
        {
            tickMaxCD -= 1;
            damage++;
        }
    }
}
