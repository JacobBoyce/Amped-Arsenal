using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SwordSwingLogic : MonoBehaviour
{
    public GameObject otherSword;
    public Animator swingAnimator;
    public int swingNum;

    public SwordSwingController controller;
    public WeaponMods weapMod;

    //public float damage = 3;

    public void InitSword(SwordSwingController cont, WeaponMods mod, GameObject self, GameObject other)
    {
        controller = cont;
        weapMod = mod;
        otherSword = other;
    }

    //animator uses this to know how many times to swing
    public void UpdateSwingNumber()
    {
        if(controller.curSwingNum > 0)
        {
            controller.curSwingNum--;
            this.gameObject.SetActive(false);
            otherSword.SetActive(true);
            otherSword.GetComponent<SwordSwingLogic>().TurnOn();
            //StartCoroutine(WaitForAnimEnd());
        }
        else if(controller.curSwingNum == 0)
        {
            //swingAnimator.SetInteger("swingNum", swingNum);
            this.gameObject.SetActive(false);
        }
    }

    IEnumerator WaitForAnimEnd()
    {
        yield return new WaitForSeconds(.2f);
        
    }

    public void TurnOn()
    {
        swingAnimator.SetTrigger("Swing");
    }

    public void OnTriggerEnter(Collider collision)
    {
        EnemyController ec = collision.GetComponent<EnemyController>();

        if(ec != null)
        {
            controller.SendDamage(ec);
            
            //to remove any knock back
            //ec.SendMessage("DetectIfKnockback");
        }
    }
}
