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
    public GameObject explAnimObj;
    public List<GameObject> animsTODestroy;
    public AnimationClip animClip;

    public void Start()
    {
        //.5 is good
        GetComponent<SphereCollider>().radius = explRadius;
        animsTODestroy = new();

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
                //Destroy(this.gameObject,1f);
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
        foreach(GameObject go in animsTODestroy) Destroy(go);  
        activate = true;   
    }

    public void OnTriggerEnter(Collider collision)
    {
        EnemyController ec = collision.GetComponent<EnemyController>();
        GameObject tempExpl = Instantiate(explAnimObj, ec.gameObject.transform);
        tempExpl.transform.SetLocalPositionAndRotation(new Vector3(0,0,0), Quaternion.Euler(0,0,0));
        animsTODestroy.Add(tempExpl);
        // if ec is not null
        ec?.TakeDamageFromEffect(damage, damageColor);
    }
}
