using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPlayer : MonoBehaviour
{
    public enum DropItem
    {
        XP,
        GOLD
    }

    public DropItem itemType;
    public float speed, distance, distToLamp;
    public int amount;
    public bool inRangeOfPlayer = false, inRangeOfLamp = false, givenXp = false;
    private PlayerController p1;
    public GameObject visuals, partSys; //visuals;
    public AbsorbLamp lamp;

    public void Start()
    {
        p1 = PlayerController.playerObj;
        //get lamp from game controller or event controller
        // lamp = gamezonecontroller.instance.absorblamp
        foreach (GameObject go in POIController.Instance.spawnedEvents)
        {
            if (go.tag.Equals("ALamp"))
            {
                lamp = go.GetComponent<AbsorbLamp>();
            }
        }

        distToLamp = Vector3.Distance(lamp.transform.position, transform.position);
        if (distToLamp < 14 && itemType == DropItem.XP)
        {
            inRangeOfLamp = true;
        }
    }

    public void Update()
    {
        if (!inRangeOfLamp)
        {
            distance = Vector3.Distance(p1.transform.position, transform.position);

            if (distance < p1._stats["pull"].Value)
            {
                transform.position = Vector3.MoveTowards(transform.position, p1.transform.position, speed * Time.deltaTime);
                speed += .1f;
            }
        }
        else if(inRangeOfLamp && itemType == DropItem.XP)
        {
            //move towards lamp
            //maybe get lamp script to get the point the balls should fly to
            transform.position = Vector3.MoveTowards(transform.position, lamp.transform.position, speed * Time.deltaTime);
            speed += .2f;

            distToLamp = Vector3.Distance(lamp.transform.position, transform.position);

            if (distToLamp < 3 && !givenXp)
            {
                givenXp = true;
                //call lamp script to subtract from counter
                lamp.UpdateCount();
                CallVisuals();
                Destroy(this.gameObject, 1f);
            }
        }
    }

    public void CallVisuals()
    {
        visuals.SetActive(false);
        //check if particle system exists
        if (partSys != null)
        {
            partSys.SetActive(true);
            for (int i = 0; i < partSys.GetComponentsInChildren<ParticleSystem>().Length; i++)
            {
                partSys.GetComponentsInChildren<ParticleSystem>()[i].Play();
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        //when I touch the player
        if(other.tag.Equals("Player"))
        {
            CallVisuals();
            
            //add value to player
            if(itemType == DropItem.XP)
            {
                p1.AddXP(amount);
            }
            else if(itemType == DropItem.GOLD)
            {
                p1.AddGold(amount);
            }
            
            Destroy(this.gameObject,1f);
        }        
    }
}
