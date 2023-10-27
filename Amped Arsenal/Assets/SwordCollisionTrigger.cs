using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollisionTrigger : MonoBehaviour
{
    public WeaponMods weapMod;

    public void OnTriggerEnter(Collider collision)
    {
        
        EnemyController ec = collision.GetComponent<EnemyController>();

        if(ec != null)
        {
            gameObject.GetComponentInParent<SwordSwingLogic>().CollisionWithEnemy(ec);
        }
    }
}
