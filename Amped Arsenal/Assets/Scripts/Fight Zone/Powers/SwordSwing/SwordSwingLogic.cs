using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSwingLogic : MonoBehaviour
{
    public Animator swingAnimator;
    public int swingNum = 3;

    public float damage = 3;

    void Awake()
    {
        swingAnimator.SetInteger("swingNum", swingNum);
    }

    public void UpdateSwingNumber()
    {
        if(swingNum > 0)
        {
            swingNum--;
            swingAnimator.SetInteger("swingNum", swingNum);
        }
        else
        {
            Destroy(this.gameObject.transform.parent.gameObject);
        }
    }

    public void OnTriggerEnter(Collider collision)
    {
        EnemyController ec = collision.GetComponent<EnemyController>();

        if(ec != null)
        {
            //Debug.Log("Sending damage: " +PlayerController.playerObj._stats["str"].Value * damage);
            ec.TakeDamage(PlayerController.playerObj._stats["str"].Value * damage);
        }
    }
}
