using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicLib : MonoBehaviour
{
    public List<GameObject> relicList = new();
    public List<GameObject> weapModList = new();

    public GameObject FindRelicFromLib(string searchName)
    {
        GameObject relic = relicList.Find(x => x.GetComponent<WeaponBase>().wName.Equals(searchName));
        return relic;
    }

    public GameObject FindWeapModFromLib(string searchName)
    {
        GameObject weapMod = weapModList.Find(x => x.GetComponent<WeaponBase>().wName.Equals(searchName));
        return weapMod;
    }
}
