using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectLib : MonoBehaviour
{
    public List<EffectBase> effectLibrary = new List<EffectBase>();

    public EffectBase FindEffectFromLib(string searchName)
    {
        EffectBase effect = effectLibrary.Find(x => x.GetComponent<EffectBase>().effectName.Equals(searchName));
        return effect;
    }
}
