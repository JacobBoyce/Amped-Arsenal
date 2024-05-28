using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootEffectLogic : EffectBase
{
    [Header("Specific Effect Vars")]
    public Color damageColor;
    public float intensity;
    public bool activate, endAbility = false;
    public Modifier mod = new("rootRelic", -.99f, Modifier.ChangeType.PERCENT, true); 
    public AnimationClip animClip;

    public void Start()
    {
        TickSystem.OnSubTick += delegate (object sender, TickSystem.OnTickEventArgs e) 
        {
            tickAmtDuration++;
        };

        foreach (AnimationClip clip in GetComponent<Animator>().runtimeAnimatorController.animationClips)
        {
            if (clip.name == "RootAnim")
            {
                animClip = clip;
            }
        }
    }

    public void Update()
    {
        if(activate == true)
        {
            if(tickAmtDuration == tickMaxDuration)
            {
                endAbility = true;
            }
        }
    }
    public override void CallEffect()
    {
        //add effect visuals
        spawnOnBodyEffect.transform.SetParent(enemy.effectCont.effectSpwnPointOnEnemy.transform);
        spawnOnBodyEffect.transform.SetLocalPositionAndRotation(new Vector3(0,0,0), Quaternion.Euler(-90,0,0));

        //add enemy stop
        enemy.GetComponent<EnemyMovementController>().stagCD = tickMaxDuration;
        enemy._stats["spd"].AddMod(mod);

        activate = true;

        //change enemy color
        //enemy.spriteR.color = new Color(0.9104829f, 0.5613208f, 1, 1);
    }

    public void EndOfStartAnim()
    {
        GetComponent<Animator>().SetTrigger("Idle");
    }

    public void CheckEndRootAnim()
    {
        //if something then reverse clip
        if(endAbility)
        {
            //play clip backwards
            GetComponent<Animator>().SetTrigger("End");
        }
    }

    public void EndEffect()
    {
        enemy._stats["spd"].RemoveMod(mod.modName);
        enemy.RemoveEffect(this.effectName);
    }
}
