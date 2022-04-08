using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSwingController : WeaponBase
{
    void Start()
    {
        TickSystem.OnSubTick += delegate (object sender, TickSystem.OnTickEventArgs e) 
        {
            curCooldown++;
        };
        
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
            PlayerController.playerObj.SpawnWeapon(weapPrefab, spawnDetails[i].spawnpoint, spawnDetails[i].needsParent);
        }
    }
}
