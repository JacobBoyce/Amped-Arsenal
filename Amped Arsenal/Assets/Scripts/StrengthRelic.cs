using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthRelic : RelicBase
{
    public Modifier mod;

    public override void ApplyRelic(PlayerController player)
    {
        player._stats["str"].AddMod(mod);
        //subscribe action method (TriggerRelic) to takeDamage on player
        //p1.OnHealed += TriggerRelic;
    }

    // public void TriggerRelic()
    // {
    //     //when adding the mod check if its already applied, if so reset its timer for the turn off
    //     //update method countdown to delete mod from player after a few seconds
        
    //     if(player._stats["str"].IfExists(mod.modName) == true)
    //     {
    //         Debug.Log("mod already applied");
    //     }
    //     else
    //     {
    //         Debug.Log("applying mod");
    //         player._stats["str"].AddMod(mod);
    //         modApplied = true;
    //     }
    // }
}
