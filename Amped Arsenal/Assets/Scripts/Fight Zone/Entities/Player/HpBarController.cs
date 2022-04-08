using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HpBarController : MonoBehaviour
{
    //listens for when damage was done
    public Image hpBar;
    void Start()
    {
        PlayerController.playerObj.UpdateHPBar += UpdateHPUI;
    }

    public void UpdateHPUI(Stat stat)
    {
        hpBar.fillAmount = (float)(stat.Value) / (float)(stat.Max);
    }
}
