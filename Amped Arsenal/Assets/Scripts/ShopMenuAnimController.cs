using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenuAnimController : MonoBehaviour
{
    public GameZoneController cont;
    public RawImage dots;

    public bool makeTransition = false;
    public float cd = 0, cdM = 3f;
    public void TurnOffShopUI()
    {
        cont.TurnOffShop();
    }

    public void Update()
    {
        if(makeTransition == true)
        {
            if (dots.color.a == 0)
            {
                if (cd < cdM)
                {
                    cd += Time.fixedUnscaledDeltaTime;
                    dots.CrossFadeAlpha(1 / cdM, 3, true);
                }
                else
                {
                    makeTransition = false;
                }
            }

            if (dots.color.a == 1)
            {
                cd = cdM;
                if (cd > 0)
                {
                    cd -= Time.fixedUnscaledDeltaTime;
                    dots.CrossFadeAlpha(1 / cdM, 3, true);
                }
                else
                {
                    makeTransition = false;
                }
            }
        }
    }

    public void ToggleDots()
    {
        makeTransition = makeTransition ? false : true;
        //dots.CrossFadeAlpha(1 / cdM, 3, true);

    }    
}
