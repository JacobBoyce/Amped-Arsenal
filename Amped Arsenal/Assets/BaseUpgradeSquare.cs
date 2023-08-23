using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using UnityEngine.UI;
using TMPro;
using System;
using MapMagic.Nodes.MatrixGenerators;

public class BaseUpgradeSquare : MonoBehaviour
{
    public TextMeshProUGUI bUpName;
    public TextMeshProUGUI bUpCost;
    //possible level graphic to show what level its at

    public string baseUpgradeName;
    public int baseUpgradeLevel;
    private int _baseCost;

    public int BaseCost
    {
        get{return _baseCost;}
        set
        {
            _baseCost = value;
            UpdateUI();
        }
    }

    public void SetNameAndLevel(string uName, int ulvl)
    {
        baseUpgradeName = uName;
        baseUpgradeLevel = ulvl;

        bUpName.text = baseUpgradeName;
        //set grapgic level to given level
    }

    public void UpdateUI()
    {
        bUpCost.text = BaseCost.ToString();
    }
}
