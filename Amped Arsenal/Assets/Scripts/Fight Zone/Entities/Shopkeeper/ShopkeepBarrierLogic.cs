using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopkeepBarrierLogic : MonoBehaviour
{
    public ShopMovementAI moveScript;
    public bool obstHit;
    
    public void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "TerrainObj")
        {
            obstHit = true;
        }
    }

    public void OnCollisionExit(Collision other)
    {
        if(other.gameObject.tag == "TerrainObj")
        {
            obstHit = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            if(!obstHit)
            {
                moveScript.enemyCount++;
                moveScript.RunawayLogic(other.gameObject);
            }
        }
        if(other.tag == "TerrainObj")
        {
            if(!obstHit)
            {
                moveScript.RunawayLogic(other.gameObject);
            }
        }

        if(other.tag == "Player")
        {
            moveScript.RunToLogic(other.gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        //if player exits turn off toggle
        if(other.tag == "Player")
        {
            moveScript.inPlayerRange = false;
            moveScript.finishMovedToPlayer = false;
            //moveScript.StartCoroutine(moveScript.StandStill());
        }
        //moveScript.TogglePlayerLogic(false)

        if(other.tag == "Enemy")
        {
            moveScript.runAwayDir = Vector3.zero;
        }

        if(other.tag == "TerrainObj")
        {
            moveScript.runAwayDir = Vector3.zero;
        }
    }
}
