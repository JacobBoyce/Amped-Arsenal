using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FearRelic : RelicBase
{
    public GameObject fearEffectPrefab;
    //public Modifier mod = new Modifier("curseRelic", .1f, Modifier.ChangeType.PERCENT, true);
    public override void ApplyRelic(PlayerController player, string weapName)
    {
        //call UI to select weapon
        foreach(GameObject weap in player.equippedWeapons)
        {
            if(weap.GetComponent<WeaponBase>().wName.Equals(weapName))
            {
                weap.GetComponent<WeaponBase>().AddEffectToWeapon(fearEffectPrefab);
            }
        }
    }
}