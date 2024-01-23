using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEffectLogic : EffectBase
{
    [Header("Specific Effect Vars")]

    //public float cd, maxCd;
    public bool activate;
    //public EnemyController en;
    public int tickDamage, maxTickDamage;
    public Modifier mod = new Modifier("fireRelic", .15f, Modifier.ChangeType.PERCENT, true);   

    public void Start()
    {
        TickSystem.OnSubTick += delegate (object sender, TickSystem.OnTickEventArgs e) 
        {
            tickAmtDuration++;
            tickDamage++;
        };
    }
    //duration
    public void Update()
    {
        if(activate == true)
        {
            if(tickDamage == maxTickDamage)
            {
                enemy.TakeDamage(damage);
                tickDamage = 0;
            }

            if(tickAmtDuration == tickMaxDuration)
            {
                enemy._stats["spd"].RemoveMod(mod.modName);
                enemy.RemoveEffect(this.effectName);
            }
        }
    }

    public override void CallEffect()
    {
        partSys.Play();
        tickAmtDuration = 0;
        activate = true;
        enemy._stats["spd"].AddMod(mod);
    }
}