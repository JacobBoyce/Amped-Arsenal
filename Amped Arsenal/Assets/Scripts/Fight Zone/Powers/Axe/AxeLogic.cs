using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeLogic : MonoBehaviour
{
    public GameObject visuals;
    public int rotSpeed;
    public AxeController controller; 
    public WeaponMods weapMod;
    
    public void Update()
    {
        visuals.transform.Rotate(new Vector3(0, rotSpeed * Time.deltaTime,0));
    }
    public void UpdateInfo(AxeController acont, WeaponMods mod)
    {
        controller = acont;
        Debug.Log("Controllers knockback " + controller.weapMod.knockbackModAmount);
        weapMod.giveKnockback = controller.weapMod.giveKnockback;
        weapMod.knockbackModAmount = controller.weapMod.knockbackModAmount;
        weapMod.eleType = controller.weapMod.eleType;
    }

    public void OnTriggerEnter(Collider collision)
    {
        EnemyController ec = collision.GetComponent<EnemyController>();

        if(ec != null)
        {
            controller.SendDamage(ec);
        }
    }
}
