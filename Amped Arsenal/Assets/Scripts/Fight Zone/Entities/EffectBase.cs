using System.Collections;
using System.Collections.Generic;
//using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;
using UnityEngine;

public class EffectBase : MonoBehaviour
{
    /*public enum SpawnEffectTypes
    {
        Particle,
        UIEffect,
        OnBodyEffect
    }*/
    
    public string effectName;
    public float damage = .5f;
    public int tickAmtDuration, tickMaxDuration;

    public Sprite modRelicImg;
    
    public ParticleSystem partSys;
    public Image uiEffectImg;
    public GameObject spawnOnBodyEffect;
    public EnemyController enemy;

    public virtual void CallEffect()
    {

    }
    //action method

    //upgrades values
}
