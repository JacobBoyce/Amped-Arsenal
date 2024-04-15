using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackLogic : EffectBase
{
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

        enemy.movementController.enemyState = EnemyMovementController.EnemyStates.STAGGER;
        //enemy.movementController.isStaggered = true;
        enemy.movementController.StartCoroutine(enemy.movementController.ApplyKnockback());
        enemy.movementController.stagCD = tickMaxDuration;
    }

    //private IEnumerator ApplyKnockback()
    //{

    //}
}
