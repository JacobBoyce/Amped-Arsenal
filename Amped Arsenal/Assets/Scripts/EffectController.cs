using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    public List<GameObject> effectObjs = new List<GameObject>();
    public EnemyController enemyCont;
    public GameObject effectParent, uiSatusEffectParent, uiStatusEffectPrefab, effectSpwnPointOnEnemy;

    public void AddEffect(GameObject effectPrefab, EnemyController en)
    {
        GameObject tempEffect = Instantiate(effectPrefab);
        tempEffect.transform.SetParent(effectParent.transform);
        tempEffect.transform.localPosition = Vector3.zero;
        // call effect init method or whatever it needs to start acting
        enemyCont = en;
        tempEffect.GetComponent<EffectBase>().enemy = enemyCont;
        tempEffect.GetComponent<EffectBase>().CallEffect();
        effectObjs.Add(tempEffect);
        //add to ui if it has ui effect
    }

    public void RemoveEffect(string eName)
    {
        foreach(GameObject go in effectObjs)
        {
            if(go.GetComponent<EffectBase>().effectName.Equals(eName) == true)
            {
                Destroy(go);
                effectObjs.Remove(go);
               //remove ui on enemy for effect
            }
        }
    }

    public void UpdateEffect(GameObject effect)
    {
        //GameObject tempEffect = GetEffect(effect.GetComponent<EffectBase>().effectName);
        //EffectBase _effect = tempEffect.GetComponent<EffectBase>();

        foreach(GameObject go in effectObjs)
        {
            if(go.GetComponent<EffectBase>().effectName.Equals(effect.GetComponent<EffectBase>().effectName) == true)
            {
                //Debug.Log("Reset Cooldown of " + go.GetComponent<EffectBase>().effectName + " effect");
                go.GetComponent<EffectBase>().tickAmtDuration = 0;
            }
        }

       
        //_effect.tickAmtDuration = 0;
    }
    //tick
    public GameObject GetEffect(string eName)
    {
        GameObject tempEffect = new GameObject();
        foreach(GameObject go in effectObjs)
        {
            if(go.GetComponent<EffectBase>().Equals(eName) == true)
            {
                tempEffect = go;
            }
        }
        return tempEffect;
    }


    //clear all effect (for player)
    
}
