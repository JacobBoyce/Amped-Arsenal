using System.Collections;
using System.Collections.Generic;
using Den.Tools;
using UnityEngine;

public class MoveToPlayer : MonoBehaviour
{
    public enum DropItem
    {
        XP,
        GOLD
    }
    

    public DropItem itemType;
    public float speed, rateOfSpeed, distance, distToLamp;
    public int amount;
    public bool isDelay = true, inRangeOfLamp = false, givenXp = false;
    private bool addedValue = false;
    private PlayerController p1;
    public GameObject visuals, partSys; //visuals;
    public AbsorbLamp lamp;

    [Header("Sounds Vars")]
    public AudioSource pickupSound;
    [Range(0.1f, 0.5f)]
    public float pitchMultiplier;

    public void OnEnable()
    {
        Debug.Log("Enabled");
        //p1 = PlayerController.playerObj;
        givenXp = false;
        speed = 1;
        isDelay = true;
        visuals.SetActive(true);
        partSys.SetActive(false);
        //get lamp from game controller or event controller
        // lamp = gamezonecontroller.instance.absorblamp
        StartCoroutine(DelayStart());
    }

    public IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("delay over");
        isDelay = false;
    }

    public void Start()
    {
        p1 = PlayerController.playerObj;
        visuals.SetActive(true);
        partSys.SetActive(false);
        speed = 1;
        isDelay = true;
        foreach (GameObject go in POIController.Instance.spawnedEvents)
        {
            if (go.tag.Equals("ALamp"))
            {
                lamp = go.GetComponent<AbsorbLamp>();
                distToLamp = Vector3.Distance(lamp.transform.position, transform.position);
            }
        }

        if (distToLamp < 14 && itemType == DropItem.XP)
        {
            if(lamp!= null && lamp.ableToAbsorb)
            {
                inRangeOfLamp = true;
            }
        }

        //StartCoroutine(DelayStart());
    }

    public void Update()
    {
        // if (!inRangeOfLamp)
        // {
        //     distance = Vector3.Distance(p1.transform.position, transform.position);

        //     if (distance < p1._stats["pull"].Value && isDelay == false)
        //     {
        //         transform.position = Vector3.MoveTowards(transform.position, p1.transform.position, speed * Time.deltaTime);
        //         speed += rateOfSpeed*rateOfSpeed;
        //     }
        // }
        // else if(inRangeOfLamp && itemType == DropItem.XP)
        // {
        //     if(lamp.ableToAbsorb == true)
        //     {
        //         //move towards lamp
        //         //maybe get lamp script to get the point the balls should fly to
        //         transform.position = Vector3.MoveTowards(transform.position, lamp.transform.position, speed * Time.deltaTime);
        //         speed += .2f;

        //         distToLamp = Vector3.Distance(lamp.transform.position, transform.position);

        //         if (distToLamp < 3 && !givenXp)
        //         {
        //             givenXp = true;
        //             //call lamp script to subtract from counter
        //             lamp.UpdateCount();
        //             CallVisuals();
        //             StartCoroutine(ReturnToPoolAfterTime());
        //             //Destroy(this.gameObject, 1f);
        //         }
        //     }
        // }
    }

    private IEnumerator ReturnToPoolAfterTime()
    {
        Debug.Log("delay release");
        yield return new WaitForSeconds(1f);
        ObjectPoolManager.ReturnObjectToPool(this.gameObject);
        Debug.Log("released");
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
            if(givenXp == false)
            {
                givenXp = true;
                CallVisuals();
                pickupSound.pitch = Random.Range(1 - pitchMultiplier, 1 + pitchMultiplier);
                pickupSound.PlayOneShot(pickupSound.clip);
                //add value to player
                if(itemType == DropItem.XP)
                {
                    p1.AddXP(amount);
                }
                else if(itemType == DropItem.GOLD)
                {
                    p1.AddGold(amount);
                }
                StartCoroutine(ReturnToPoolAfterTime());
                //Destroy(this.gameObject,1f);
            }
        }        
    }
}
