using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleLogic : MonoBehaviour
{
    public Animator animCont;
    public float timeToDie, countdown;
    private bool switched2 = false, switched3 = false;
    public CandleController controller;
    public WeaponMods weapMod;
    public CapsuleCollider bodyCollider;

    [Header("Battle Vars")]
    //Battle Vars
    public float aoeRange;
    public SphereCollider rangeCollider;
    public float damageTick, damageTickCooldown;

    [Header("Spawning Vars")]
    private float cdM = .5f, cd;
    private bool touchingGround = false;
   

    // code jumping animation from player
    void Start()
    {
        countdown = 0;

        
    }


    public void InitCandle(CandleController sCont, WeaponMods mod)
    {
        //this.transform.eulerAngles = new Vector3(0,0,0);
        controller = sCont;
        weapMod = mod;
        aoeRange = controller.aoeRange;
        damageTick = controller.damageTick;
        GetComponent<Rigidbody>().freezeRotation = true;

        rangeCollider.radius = aoeRange;
    }


    // change the sprite based on its lifespan
    void Update()
    {
        if(cd < cdM)
        {
            cd += Time.deltaTime;
        }
        else
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            GetComponent<Rigidbody>().freezeRotation = true;
        }

        if(countdown < timeToDie)
        {
            countdown += Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject);
        }

        if (switched2 == false)
        {
            if ((countdown / timeToDie) > .33)
            {
                //switch anim
                animCont.SetBool("stage2", true);
                switched2 = true;
            }

        }

        if(switched2 == true && switched3 == false)
        {
            if ((countdown / timeToDie) > .66)
            {
                //switch anim
                animCont.SetBool("stage3", true);
                switched3 = true;
            }
        }        
    }

    public void OnTriggerStay(Collider collision)
    {
        EnemyController ec = collision.GetComponent<EnemyController>();

        if (ec != null)
        {
            if(damageTickCooldown < damageTick)
            {
                damageTickCooldown += Time.deltaTime;
            }
            else
            {
                damageTickCooldown = 0;
                controller.SendDamage(ec);
            }           
        }
    }
}
