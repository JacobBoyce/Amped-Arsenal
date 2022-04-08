using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLib : MonoBehaviour
{
    public List<GameObject> weaponList = new List<GameObject>();

    public GameObject FindWeaponFromLib(string searchName)
    {
        GameObject weap = weaponList.Find(x => x.GetComponent<WeaponBase>().wName.Equals(searchName));
        return weap;
    }
    
}