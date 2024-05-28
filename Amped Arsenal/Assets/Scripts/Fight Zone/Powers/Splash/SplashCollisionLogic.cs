using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashCollisionLogic : MonoBehaviour
{
    public SplashController controller;

    public void OnTriggerEnter(Collider collision)
    {
        EnemyController ec = collision.gameObject.GetComponent<EnemyController>();
        
        if(ec != null)
        {
            //Debug.Log("hitEnemy");
            controller.SendDamage(ec);
            controller.PlayDamageSound();
        }
    }
}
