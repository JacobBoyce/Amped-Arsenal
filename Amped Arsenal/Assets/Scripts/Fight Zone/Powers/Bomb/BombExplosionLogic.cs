using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosionLogic : MonoBehaviour
{
    private float cd = 2.5f;
    public float maxCD;
    public BombController controller;

    public void Start()
    {
        maxCD = cd;
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
            if(cd < maxCD*.8f)
            {
                GetComponent<SphereCollider>().radius = 0;
            }
            else
            {
                GetComponent<SphereCollider>().radius = Mathf.Max(0, GetComponent<SphereCollider>().radius - 0.05f);
            }
            

        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
