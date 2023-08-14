using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEffectLogic : EffectBase
{
    [Header("Specific Effect Vars")]
    //damage
    //ADD TICKS
    //duration /5 ticks
    //public GameObject poisonPrefab;
    public Color damageColor;
    public float intensity;
    //public float cd, maxCd;
    public bool activate;
    //public EnemyController en;
    public int tickDamage, maxTickDamage;

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
        //if bool is true 
            //start cooldown
            //call visual damage everytime you send damage to the enemy

        if(activate == true)
        {
            if(tickDamage == maxTickDamage)
            {
                enemy.TakeDamage(damage);
                tickDamage = 0;
            }

            if(tickAmtDuration == tickMaxDuration)
            {
                enemy.RemoveEffect(this.effectName);
            }

            /*if(cd > 0)
            {
                cd -= Time.deltaTime;
            }
            else if(cd <= 0)
            {
                duration++;
                
                if(duration >= maxDuration)
                {
                    //delete poison effect
                    
                    //call it from enemy script
                }
                else
                {
                    enemy.TakeDamage(damage);
                    cd = maxCd;
                }
            }*/
        }
    }

    public override void CallEffect()
    {
        //set effect damage (scales with weapon)
                //20      *  .5 = 10
        //damage = weap dmg * poison damage
        partSys.Play();
        tickAmtDuration = 0;
        //cd = maxCd;
        activate = true;
        //change enemy color
        //enemy.spriteR.color = new Color(0.9104829f, 0.5613208f, 1, 1);
        

        //start the cooldown to be removed from the actor (bool)
    }
}
