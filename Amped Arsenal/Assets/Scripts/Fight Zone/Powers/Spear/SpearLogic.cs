using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearLogic : MonoBehaviour
{
    public SpearController controller;
    public WeaponMods weapMod;
    Rigidbody thisRB;
    public int speed, pierceNum;
    // Start is called before the first frame update
    void Start()
    {
        thisRB = GetComponent<Rigidbody>();
        thisRB.AddForce(-(controller.playerObj.transform.position - transform.position).normalized * speed, ForceMode.Impulse);
    }
    
    public void InitSpear(SpearController sCont, WeaponMods mod)
    {
        controller = sCont;
        weapMod = mod;
        speed = controller.speed;
        pierceNum = controller.pierceNum;
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
    }
}
