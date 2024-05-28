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
        //transform.rotation = Quaternion.Euler(controller.playerObj.transform.position - transform.position);
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
            controller.PlayDamageSound();
        }

        if(collision.tag == "TerrainObj")
        {
            gameObject.SetActive(false);
        }
    }

    public void Update()
    {
        if(pierceNum <= 0)
        {
            gameObject.SetActive(false);
        }

        float dist = Vector3.Distance(this.transform.position, controller.playerObj.gameObject.transform.position);
        if(dist > range)
        {
            gameObject.SetActive(false);
        }

        transform.LookAt(transform.position + thisRB.velocity);
    }
}
