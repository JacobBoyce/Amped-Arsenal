using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleDamagerLogic : MonoBehaviour
{
    public CandleController controller;
    // Start is called before the first frame update
    public void OnTriggerStay(Collider collision)
    {
        EnemyController ec = collision.GetComponent<EnemyController>();

        if (ec != null)
        {
            controller.SendDamage(ec);
        }
    }
}
