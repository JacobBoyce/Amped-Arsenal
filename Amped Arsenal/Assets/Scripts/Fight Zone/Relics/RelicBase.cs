using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicBase : MonoBehaviour
{
    public string relicName;
    public enum RelicType
    {
        NONE,
        ONHIT,
        ONDAMAGED,
        ONKILL,
        ONHEAL,
        ONGET
    };   
    public RelicType rType;

    public GameObject visuals;
    private int weapsAvailable = 0;

    public virtual void ApplyRelic(PlayerController player, string weapName)
    {

    }

    public virtual void ApplyRelic(PlayerController player)
    {
        
    }

    /*
    //on hit
    public void ApplyRelic(WeaponBase weap)
    {
        
    }

    //on damaged or on heal
    public void ApplyRelic(PlayerController player, bool damagedOrNot)// bool is to differ if it needs to be on heal
    {

    }

    //on kill
    public void ApplyRelic(EnemyController enemy)
    {

    }

    public void ApplyRelic(string relicName)
    {
        
    }*/

    //on get can be a generic one time call method

    public void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Player")
        {
            //add relic to player equipment
            if(rType == RelicType.ONHIT || rType == RelicType.ONKILL)
            {
                //check if weapon list has a weapon and if the weapon has enough slots
                //if you have a weapon
                if(other.gameObject.GetComponent<PlayerController>().equippedWeapons.Count > 0)
                {
                    foreach(GameObject weap in other.gameObject.GetComponent<PlayerController>().equippedWeapons)
                    {
                        //if the weapon has an open slot
                        if(weap.GetComponent<WeaponBase>().currentEquippedSlots < weap.GetComponent<WeaponBase>().maxSlots)
                        {
                            weapsAvailable++;
                        }
                    }

                    if(weapsAvailable > 0)
                    {
                        other.gameObject.GetComponent<PlayerController>().AddRelicToCache(this);
                        //player script will handle the trigger for the relics

                        //turn off visuals
                        visuals.SetActive(false);
                        GetComponent<BoxCollider>().enabled = false;
                    }
                }
            }
            else
            {
                other.gameObject.GetComponent<PlayerController>().AddRelicToCache(this);
                //player script will handle the trigger for the relics

                //turn off visuals
                visuals.SetActive(false);
                GetComponent<BoxCollider>().enabled = false;
            }
        }
    }
}