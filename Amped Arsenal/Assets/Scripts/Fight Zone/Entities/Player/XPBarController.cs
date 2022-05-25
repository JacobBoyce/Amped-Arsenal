using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPBarController : MonoBehaviour
{
    public Image xpBar, xpBarFX;
    public float fillSpeed;

    private bool updateBar = false;
    void Start()
    {
        //PlayerController.playerObj.UpdateXPBar += UpdateXPUI;
    }

    public void UpdateXPUI(Stat stat, bool hasLeveled)
    {
        xpBarFX.fillAmount = (float)(stat.Value) / (float)(stat.Max);
        if(hasLeveled)
        {
            xpBar.fillAmount = 0;
        }
        updateBar = true;
    }

    public void Update()
    {
        if(updateBar)
        {
            if(xpBar.fillAmount < xpBarFX.fillAmount)
            {
                xpBar.fillAmount += fillSpeed * Time.deltaTime;
            }
            else
            {

                updateBar = false;
            }
        }
    }
}
