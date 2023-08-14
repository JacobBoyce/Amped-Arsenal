using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonRelic : RelicBase
{
    public PoisonEffectLogic poisonEffect;
    public GameObject poisonEffectPrefab;

    public WeaponBase appliedWeapon;


    public override void ApplyRelic(PlayerController player, string weapName)
    {
        //call UI to select weapon
        foreach(GameObject weap in player.equippedWeapons)
        {
            if(weap.GetComponent<WeaponBase>().wName.Equals(weapName))
            {
                weap.GetComponent<WeaponBase>().AddEffectToWeapon(poisonEffectPrefab);
            }
        }
    }
}
