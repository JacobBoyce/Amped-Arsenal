using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartRelic : RelicBase
{
    public Modifier mod = new Modifier("heartRelic", .1f, Modifier.ChangeType.PERCENT, true);
    public override void ApplyRelic(PlayerController player)
    {
        //give player stat boost
        player._stats["hp"].AddMod(mod);
    }
}
