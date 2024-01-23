using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Relic : RelicBase
{
    public FireEffectLogic fireEffectLogic;
    public GameObject fireEffectPrefab;
    public WeaponBase appliedWeapon;

    public override void ApplyRelic(PlayerController player, string weapName)
    {
        //call UI to select weapon
        foreach(GameObject weap in player.equippedWeapons)
        {
            if(weap.GetComponent<WeaponBase>().wName.Equals(weapName))
            {
                weap.GetComponent<WeaponBase>().AddEffectToWeapon(fireEffectPrefab);
            }
        }
    }
}
