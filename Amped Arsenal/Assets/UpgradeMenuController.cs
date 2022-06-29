using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenuController : MonoBehaviour
{
    public WeaponFocusUI weapFocus;
    public EquippedUI equippedUICont;
    private PlayerController playerCont;

    public void Awake()
    {
        playerCont = PlayerController.playerObj;
        //playerCont.equippedWeapons
    }

    public void PopulateUI()
    {

    }

    public void InitUI()
    {

    }
}
