using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosionLogic : MonoBehaviour
{
    private float cd = .2F;
    public BombController controller;

    public void Start()
    {
        GetComponent<SphereCollider>().radius = controller.range;
    }
    public void OnTriggerEnter(Collider collision)
    {
        EnemyController ec = collision.GetComponent<EnemyController>();
        
        if(ec != null)
        {
            controller.SendDamage(ec);
        }
    }

    public void Update()
    {
        if(cd > 0)
        {
            cd -= Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
