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
    public float speed, distance;
    public int amount;
    public bool inRange = false;
    private PlayerController p1;
    public GameObject visuals, partSys; //visuals;

    public void Start()
    {
        p1 = PlayerController.playerObj;
    }

    public void Update()
    {
        distance = Vector3.Distance(p1.transform.position, transform.position);

        if(distance < p1._stats["pull"].Value)
        {
            transform.position = Vector3.MoveTowards(transform.position, p1.transform.position, speed * Time.deltaTime);
            speed += .1f;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            visuals.SetActive(false);
            //check if particle system exists
            if(partSys != null)
            {
                partSys.SetActive(true);
                for(int i = 0; i < partSys.GetComponentsInChildren<ParticleSystem>().Length; i++)
                {
                    partSys.GetComponentsInChildren<ParticleSystem>()[i].Play();
                }
            }
            
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
