using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopkeepBarrierLogic : InteractableObject
{
    public ShopMovement moveScript;
    public bool obstHit;
    public bool isInRange = false;
    
    public void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "TerrainObj")
        {
            moveScript.runFromObject = other.gameObject;
        }
        else if(other.gameObject.tag == "Player")
        {
            //other.gameObject.GetComponent<PlayerController>().mainController.OpenShop();
        }
    }

    public void OnCollisionExit(Collision other)
    {
        if(other.gameObject.tag == "TerrainObj")
        {
            moveScript.runFromObject = null;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            if(!obstHit)
            {
                //moveScript.RunawayLogic(other.gameObject);
            }
        }
        if(other.tag == "TerrainObj")
        {
            if(!obstHit)
            {
                //moveScript.RunawayLogic(other.gameObject);
            }
        }

        if(other.tag == "Player")
        {
            //moveScript.RunToLogic(other.gameObject);
            //other.GetComponent<PlayerController>().OpenShop(true);
            //tell player he can press space
            if(moveScript.escapedPhase == false)
            {
                //other.GetComponent<PlayerController>().OpenShop(true);
                moveScript.alertUI.text = "!";
            }
            
            //trigger ! over head off
        }
    }

    public void OnTriggerExit(Collider other)
    {
        //if player exits turn off toggle
        if(other.tag == "Player")
        {
            //moveScript.inPlayerRange = false;
            //moveScript.finishMovedToPlayer = false;
            //moveScript.StartCoroutine(moveScript.StandStill());

            //tell player he canot open the shop
            //other.GetComponent<PlayerController>().OpenShop(false);

            //trigger ! over head off
            moveScript.alertUI.text = "";
        }
    }

    public override void InRange(PlayerController player)
    {
        //base.InRange();

        //subscribe to player interact method
        player.GetComponent<PlayerController>().OpenShop(true);
        player.TriggerInteractEvent += TriggerInteract;
        isInRange = true;
    }

    public override void NotInRange(PlayerController player)
    {
        //base.InRange();
        isInRange = false;
        EventTriggered = false;
        player.GetComponent<PlayerController>().OpenShop(false);
        player.TriggerInteractEvent -= TriggerInteract;
    }

    public override void TriggerInteract()
    {
        if(EventTriggered == false && !GameZoneController.Instance.isUpgrading)
        {
            
            GameZoneController.Instance.OpenShop();
            EventTriggered = true;
        }
    }

    public void TriggerEscapeIfWasNearby(PlayerController player)
    {
        isInRange = false;
        EventTriggered = false;
        player.TriggerInteractEvent -= TriggerInteract;
    }
}
