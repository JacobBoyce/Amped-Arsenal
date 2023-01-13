using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonRelic : RelicBase
{
    public PoisonEffectLogic poisonEffect;
    public GameObject poisonEffectPrefab;

    public WeaponBase appliedWeapon;

    public void Start()
    {
        
    }
    public override void ApplyRelic(PlayerController player)
    {
        //call UI to select weapon
        foreach(GameObject weap in player.equippedWeapons)
        {
            if(weap.GetComponent<WeaponBase>().wName.Equals("Axe"))
            {
                weap.GetComponent<WeaponBase>().AddEffectToWeapon(poisonEffectPrefab);
            }
        }
    }
}
