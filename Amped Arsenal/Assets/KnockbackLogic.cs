using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackLogic : EffectBase
{
    public float knockbackAmt;
    [Header("Specific Effect Vars")]

    public Color damageColor;
    public float intensity;
    //public float cd, maxCd;
    public bool activate;


    public void Start()
    {
        TickSystem.OnSubTick += delegate (object sender, TickSystem.OnTickEventArgs e) 
        {
            tickAmtDuration++;
        };
    }
    //duration
    public void Update()
    {
        if(activate == true)
        {
            if(tickAmtDuration == tickMaxDuration)
            {
                enemy.RemoveEffect(this.effectName);
            }
        }
    }

    public override void CallEffect()
    {
        partSys.Play();
        tickAmtDuration = 0;
        
        //This starts the countdown for the effect to be removed
        activate = true;
        enemy.TakeDamageFromEffect(damage);
        
        //enemy.movementController.isStaggered = true;
        if(enemy.movementController.gettingKnockedBack == false)
        {
            enemy.movementController.enemyState = EnemyMovementController.EnemyStates.STAGGER;
            enemy.movementController.StartCoroutine(enemy.movementController.ApplyKnockback(knockbackAmt));
            enemy.movementController.stagCD = tickMaxDuration;
        }
    }
}
