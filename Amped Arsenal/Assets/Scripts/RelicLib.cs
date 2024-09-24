using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicLib : MonoBehaviour
{
    public List<GameObject> relicList = new();
    public List<GameObject> weapModList = new();
    //public List<GameObject> weapModNotToSpawnList;
    public List<string> modstoRemoveNames;

    public GameObject FindRelicFromLib(string searchName)
    {
        GameObject relic = relicList.Find(x => x.GetComponent<WeaponBase>().wName.Equals(searchName));
        return relic;
    }

    public void Start()
    {
        
    }

    public GameObject ReturnAWeapModNotChosenYet()
    {
        int randomIndex = Random.Range(0, weapModList.Count);
        // Get a list of available mods that are not in the not-to-spawn list

            // foreach (GameObject mod in weapModList)
            // {
            //     foreach(string notSpawnMod in modstoRemoveNames)
            //     {
            //         if (mod.GetComponent<RelicBase>().relicName == notSpawnMod)
            //         {
            //             Debug.Log(mod.GetComponent<RelicBase>().relicName + " Has been spawned");
            //             modstoRemoveNames.Add(mod.GetComponent<RelicBase>().relicName);
            //         }
            //     }
                
            //     foreach(string mName in modstoRemoveNames)
            //     {
            //         weapModList.Remove(FindWeapModFromLib(mName));
            //     }
            // }
        
        

        // Choose a random mod from the available mods
        
        //GameObject selectedMod = availableMods[randomIndex];

        return weapModList[randomIndex]; // Return the selected weapon mod
    }

    public GameObject FindWeapModFromLib(string searchName)
    {
        GameObject weapMod = weapModList.Find(x => x.GetComponent<WeaponBase>().wName.Equals(searchName));
        return weapMod;
    }
}
