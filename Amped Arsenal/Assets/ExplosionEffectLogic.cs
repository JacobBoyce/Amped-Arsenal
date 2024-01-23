using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffectLogic : EffectBase
{
    [Header("Specific Effect Vars")]
    public Color damageColor;
    public float intensity;
    public bool activate;
    public float explRadius;
    public AnimationClip animClip;

    public void Start()
    {
        explRadius = GetComponent<SphereCollider>().radius;

        foreach (AnimationClip clip in GetComponent<Animator>().runtimeAnimatorController.animationClips)
        {
            if (clip.name == "BombExplosion_Effect_Anim")
            {
                animClip = clip;
            }
        }
        //tickMaxDuration = animClip.length
    }

    public void Update()
    {
        if(activate == true)
        {
            //if(tickAmtDuration == tickMaxDuration)
            //{
                enemy.RemoveEffect(this.effectName);
                Destroy(this.gameObject);
            //}
        }
    }
    public override void CallEffect()
    {
        //set effect damage (scales with weapon)
                //20      *  .5 = 10
        //damage = weap dmg * poison damage
        spawnOnBodyEffect.transform.SetParent(enemy.effectCont.effectSpwnPointOnEnemy.transform);
        spawnOnBodyEffect.transform.SetLocalPositionAndRotation(new Vector3(0,0,0), Quaternion.Euler(0,0,0));

    }

    public void DelayDelete()
    {
        activate = true;        
    }

    public void OnTriggerEnter(Collider collision)
    {
        EnemyController ec = collision.GetComponent<EnemyController>();
        
        // if ec is not null
        ec?.TakeDamageFromEffect(damage, damageColor);
    }
}
