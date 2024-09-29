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
    public bool isDelay = true, inRangeOfLamp = false, givenXp = false, magnetPickedUp = false;
    private PlayerController p1;
    public GameObject visuals; //visuals;
    public AbsorbLamp lamp;

    [Header("Sounds Vars")]
    public AudioSource pickupSound;
    [Range(0.1f, 0.5f)]
    public float pitchMultiplier;

    public void OnEnable()
    {
        //Debug.Log("Enabled");
        //p1 = PlayerController.playerObj;
        givenXp = false;
        speed = 1;
        magnetPickedUp = false;
        isDelay = true;
        visuals.SetActive(true);
        
        //get lamp from game controller or event controller
        // lamp = gamezonecontroller.instance.absorblamp
        StartCoroutine(DelayStart());

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
    }

    public void OnDisable()
    {
        magnetPickedUp = false;
    }

    public IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(1f);
        //Debug.Log("delay over");
        isDelay = false;
    }

    public void Start()
    {
        p1 = PlayerController.playerObj;
        visuals.SetActive(true);
        speed = 1;
        //isDelay = true;
        

        //StartCoroutine(DelayStart());
    }

    public void Update()
    {

        // Check if the magnet is picked up
        if (magnetPickedUp)
        {
            // Move towards the player (p1) if magnet is picked up
            transform.position = Vector3.MoveTowards(transform.position, p1.transform.position, speed * Time.deltaTime);
            speed += rateOfSpeed * rateOfSpeed;
        }
        else
        {
            // Check if the lamp exists
            if (lamp != null)
            {
                // Check if the lamp can absorb and the item is XP and in range
                if (lamp.ableToAbsorb == true && inRangeOfLamp && itemType == DropItem.XP)
                {
                    transform.position = Vector3.MoveTowards(transform.position, lamp.transform.position, speed * Time.deltaTime);
                    speed += .2f;

                    float distToLamp = Vector3.Distance(lamp.transform.position, transform.position);

                    // Check if the item is close enough to the lamp to absorb it
                    if (distToLamp < 3 && !givenXp)
                    {
                        givenXp = true;
                        // Call lamp script to subtract from counter
                        lamp.UpdateCount(amount);
                        ObjectPoolManager.ReturnObjectToPool(this.gameObject);
                    }
                }
                else
                {
                    // If not absorbed by the lamp, check distance to player
                    float distance = Vector3.Distance(p1.transform.position, transform.position);
                    if (distance < p1._stats["pull"].Value && !isDelay)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, p1.transform.position, speed * Time.deltaTime);
                        speed += rateOfSpeed * rateOfSpeed;
                    }
                }
            }
            else // If the lamp does not exist
            {
                // Just move towards the player if in range
                float distance = Vector3.Distance(p1.transform.position, transform.position);
                if (distance < p1._stats["pull"].Value && !isDelay)
                {
                    transform.position = Vector3.MoveTowards(transform.position, p1.transform.position, speed * Time.deltaTime);
                    speed += rateOfSpeed * rateOfSpeed;
                }
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

                if(itemType == DropItem.XP)
                {
                    p1.AddXP(amount);
                }
                else if(itemType == DropItem.GOLD)
                {
                    p1.AddGold(amount);
                }
                ObjectPoolManager.ReturnObjectToPool(this.gameObject);
            }
        }        
    }
}
