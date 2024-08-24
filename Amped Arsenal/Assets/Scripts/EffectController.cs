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
        //Debug.Log("instantiate effect and set its parent\ncall its activate method and add it to the list.");
        GameObject tempEffect = Instantiate(effectPrefab);
        tempEffect.transform.SetParent(effectParent.transform);
        tempEffect.transform.localPosition = Vector3.zero;

        // call effect init method or whatever it needs to start acting
        enemyCont = en;
        tempEffect.GetComponent<EffectBase>().enemy = enemyCont;
        effectObjs.Add(tempEffect);
        tempEffect.GetComponent<EffectBase>().CallEffect();
        
        //add to ui if it has ui effect
        //effect itself adds it to the list
    }

    public void RemoveEffect(string eName, GameObject uiEffect)
    {
        GameObject temp1 = null, temp2;
        temp2 = uiEffect;
        if(effectObjs.Count > 0)
        {
            foreach(GameObject go in effectObjs)
            {
                if(go.GetComponent<EffectBase>().effectName.Equals(eName) == true)
                {
                    temp1 = go;
                    effectObjs.Remove(go);
                    Destroy(temp1);
                    Destroy(temp2);
                    break;
                }
            }
        }
    }
    public void RemoveEffect(string eName)
    {
        GameObject temp1;
        if(effectObjs.Count > 0)
        {
            foreach(GameObject go in effectObjs)
            {
                if(go.GetComponent<EffectBase>().effectName.Equals(eName) == true)
                {
                    temp1 = go;
                    effectObjs.Remove(go);
                    Destroy(go);
                    break;
                //remove ui on enemy for effect
                }
            }
        }
    }

    public void UpdateEffect(GameObject effect)
    {
        if(effectObjs.Count > 0)
        {
            foreach(GameObject go in effectObjs)
            {
                if(go.GetComponent<EffectBase>().effectName.Equals(effect.GetComponent<EffectBase>().effectName) == true)
                {
                    //Debug.Log("Reset Cooldown of " + go.GetComponent<EffectBase>().effectName + " effect");
                    go.GetComponent<EffectBase>().tickAmtDuration = 0;
                }
            }
        }
    }

    //tick
    public GameObject GetEffect(string eName)
    {
        GameObject tempEffect = new();
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
