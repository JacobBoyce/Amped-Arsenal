using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBase : MonoBehaviour
{
    
    public string effectName;
    public float damage = .5f;
    public int tickAmtDuration, tickMaxDuration;

    //particle system
    public ParticleSystem partSys;
    public EnemyController enemy;

    public virtual void CallEffect()
    {

    }
    //action method

    //upgrades values
}
