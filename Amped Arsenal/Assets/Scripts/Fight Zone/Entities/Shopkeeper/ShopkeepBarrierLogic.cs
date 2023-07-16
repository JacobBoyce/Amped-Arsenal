using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopkeepBarrierLogic : MonoBehaviour
{
    public ShopMovement moveScript;
    public bool obstHit;
    
    public void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "TerrainObj")
        {
            moveScript.runFromObject = other.gameObject;
        }
        else if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().mainController.OpenShop();
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
            moveScript.alertUI.text = "!";
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
            other.GetComponent<PlayerController>().OpenShop(false);

            //trigger ! over head off
            moveScript.alertUI.text = "";
        }
    }
}
