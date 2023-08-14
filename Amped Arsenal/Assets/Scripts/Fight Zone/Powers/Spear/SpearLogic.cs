using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearLogic : MonoBehaviour
{
    public SpearController controller;
    public WeaponMods weapMod;
    Rigidbody thisRB;

    public int speed, pierceNum, range;

    //spawn damage object with a sphere collider
    //change its radius to be the bombs range
    

    void Start()
    {
        //when spawned throw the spear in the firsction faced
        thisRB = GetComponent<Rigidbody>();
        thisRB.AddForce(-(controller.playerObj.transform.position - transform.position).normalized * speed, ForceMode.Impulse);
    }
    

    public void InitSpear(SpearController sCont, WeaponMods mod)
    {
        controller = sCont;
        weapMod = mod;
        speed = controller.speed;
        pierceNum = controller.pierceNum;
        range = controller.range;
    }

    public void OnTriggerEnter(Collider collision)
    {
        EnemyController ec = collision.GetComponent<EnemyController>();

        if(ec != null)
        {
            pierceNum--;
            controller.SendDamage(ec);
        }

        if(collision.tag == "TerrainObj")
        {
            Destroy(this.gameObject);
        }
    }

    public void Update()
    {
        if(pierceNum <= 0)
        {
            Destroy(this.gameObject);
        }

        float dist = Vector3.Distance(this.transform.position, controller.playerObj.gameObject.transform.position);
        if(dist > range)
        {
            Destroy(this.gameObject);
        }
    }
}
