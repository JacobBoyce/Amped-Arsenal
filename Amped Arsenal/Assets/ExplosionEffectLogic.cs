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
        GetComponent<SphereCollider>().radius = explRadius;
    }

    public void Update()
    {
        if(activate == true)
        {
            enemy.RemoveEffect(this.effectName);
        }
    }
    public override void CallEffect()
    {
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
        if(ec != null)
        {
            GameObject tempExpl = Instantiate(explAnimObj, ec.gameObject.transform);
            tempExpl.transform.SetLocalPositionAndRotation(new Vector3(0,0,0), Quaternion.Euler(0,0,0));
            ec.TakeDamageFromEffect(damage);
        }
    }
}
