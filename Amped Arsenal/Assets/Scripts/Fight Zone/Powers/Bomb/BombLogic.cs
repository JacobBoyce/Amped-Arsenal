using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BombLogic : MonoBehaviour
{
    public VisualEffects vfx;
    public BombController controller;
    public WeaponMods myMods;
    public Rigidbody thisRB;
    public GameObject explosion;
    public GameObject deathPoof;

    public float shootPower, offsetY;
    public int range;
    public float timeToExplode, cd;

    public void Start()
    {
        cd = timeToExplode;
        thisRB = GetComponent<Rigidbody>();

        //lob the bomb here
        GameObject shootBackDir, shootCenterDir;
        Vector3 shootDir;
        shootCenterDir = controller.spawnPointCenter;
        shootBackDir = controller.spawnPointBack;

        shootDir = shootBackDir.transform.position - shootCenterDir.transform.position;
        Debug.DrawLine(shootCenterDir.transform.position,shootBackDir.transform.position, Color.white, 2f);
        //add offset
        shootDir = new Vector3(shootDir.x, shootDir.y + offsetY, shootDir.z);
        thisRB.AddForce(shootDir * shootPower, ForceMode.Impulse);

    }

    public void InitBomb(BombController sCont, WeaponMods mod)
    {
        controller = sCont;
        timeToExplode = sCont.timeToExplode;
        range = sCont.range;
        myMods = mod;
    }

    public void Update()
    {
        if(cd > 0)
        {
            cd -= Time.deltaTime;
            if(cd/timeToExplode < .3f)
            {
                vfx.wantFlash = true;
            }
        }
        else
        {
            //explode
            //Instantiate(deathPoof, new Vector3(transform.position.x , transform.position.y, transform.position.z), transform.rotation);
            GameObject tempExpl = Instantiate(explosion, new Vector3(transform.position.x , transform.position.y, transform.position.z), quaternion.identity);
            tempExpl.GetComponent<BombExplosionLogic>().controller = controller;
            controller.PlayDamageSound();
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision) 
    { 
        if (collision.gameObject.tag == "Enemy") 
        { 
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>()); 
        } 
    }
}
