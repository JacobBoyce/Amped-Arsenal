using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSwingLogic : MonoBehaviour
{
    public Animator swingAnimator;
    public int swingNum;

    public SwordSwingController controller;
    public WeaponMods weapMod;

    //public float damage = 3;

    public void InitSword(SwordSwingController cont, WeaponMods mod, int initSwingNum)
    {
        controller = cont;
        weapMod = mod;
        SetSwingNum(initSwingNum);
    }

    //animator uses this to know how many times to swing
    public void UpdateSwingNumber()
    {
        if(swingNum > 0)
        {
            swingNum--;
            SetSwingNum(swingNum);
        }
        else if(swingNum == 0)
        {
            //swingAnimator.SetInteger("swingNum", swingNum);
            StartCoroutine(WaitForAnimEnd());
        }
    }

    IEnumerator WaitForAnimEnd()
    {
        yield return new WaitForSeconds(.2f);
        this.gameObject.SetActive(false);
    }

    public void TurnOn()
    {
        SetSwingNum(controller.swingNumber);
        swingAnimator.SetBool("moreSwings", true);
    }

    public void SetSwingNum(int num)
    {
        swingNum = num;
        swingAnimator.SetInteger("swingNum", swingNum);
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
