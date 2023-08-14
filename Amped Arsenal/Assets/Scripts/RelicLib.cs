using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicLib : MonoBehaviour
{
    public List<GameObject> relicList = new List<GameObject>();

    public GameObject FindRelicFromLib(string searchName)
    {
        GameObject relic = relicList.Find(x => x.GetComponent<WeaponBase>().wName.Equals(searchName));
        return relic;
    }
}
