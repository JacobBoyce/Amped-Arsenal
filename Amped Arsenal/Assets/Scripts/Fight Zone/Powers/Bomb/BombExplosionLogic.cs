using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosionLogic : MonoBehaviour
{
    private float cd = 1f;
    public float maxCD;
    public BombController controller;
    public ParticleSystem pSys;
    public float[] pSysSize;

    public void Start()
    {
        maxCD = cd;
        GetComponent<SphereCollider>().radius = controller.range;
        pSys.startSize = pSysSize[controller.level];
    }
    public void OnTriggerEnter(Collider collision)
    {
        EnemyController ec = collision.GetComponent<EnemyController>();
        
        if(ec != null)
        {
            controller.SendDamage(ec);
            controller.PlayDamageSound();
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
                gameObject.GetComponent<SphereCollider>().enabled = false;
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
