using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackRelic : RelicBase
{
    public int knockAmt;
    public KnockbackLogic knockbackEffect;
    public GameObject knockbackEffectPrefab;

    public WeaponBase appliedWeapon;


    public override void ApplyRelic(PlayerController player, string weapName)
    {
        //call UI to select weapon
        foreach(GameObject weap in player.equippedWeapons)
        {
            if(weap.GetComponent<WeaponBase>().wName.Equals(weapName))
            {
                weap.GetComponent<WeaponBase>().AddEffectToWeapon(knockbackEffectPrefab);
                weap.GetComponent<WeaponBase>().weapMod.knockbackModAmount += knockAmt;
            }
        }
    }
}
