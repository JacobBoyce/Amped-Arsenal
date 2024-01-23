using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionRelic : RelicBase
{
    public GameObject explosionEffectPrefab;

    public override void ApplyRelic(PlayerController player, string weapName)
    {
        //call UI to select weapon
        foreach(GameObject weap in player.equippedWeapons)
        {
            if(weap.GetComponent<WeaponBase>().wName.Equals(weapName))
            {
                weap.GetComponent<WeaponBase>().AddEffectToWeapon(explosionEffectPrefab);
            }
        }
    }
}
