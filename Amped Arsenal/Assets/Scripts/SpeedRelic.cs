using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedRelic : RelicBase
{
    public int speedAmount;
    public float timer, maxTimer;
    private PlayerController player;

    public Modifier mod;
    private bool modApplied;
    public override void ApplyRelic(PlayerController p1)
    {
        timer = maxTimer;
        mod.modAmount = speedAmount;
        player = p1;
        //subscribe action method (TriggerRelic) to takeDamage on player
        p1.OnDamaged += TriggerRelic;
    }

    public void TriggerRelic()
    {
        //when adding the mod check if its already applied, if so reset its timer for the turn off
        //update method countdown to delete mod from player after a few seconds
        
        if(player._stats["spd"].IfExists(mod.modName) == true)
        {
            //Debug.Log("mod already applied");
            timer = maxTimer;
        }
        else
        {
            //Debug.Log("applying mod");
            player._stats["spd"].AddMod(mod);
            timer = maxTimer;
            modApplied = true;
        }
    }


    public void Update()
    {
        if(modApplied == true)
        {
            if(timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else if(timer <= 0)
            {
                // remove mod from player
                player._stats["spd"].RemoveMod(mod.modName);
                modApplied = false;
            }
        }
    }
}
