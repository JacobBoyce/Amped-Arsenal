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
    public TextMeshProUGUI bUpName, bUpCost, bUpLevel;
    //possible level graphic to show what level its at
    public Image bUpImage;
    public string baseUpgradeName;
    public int _baseUpgradeLevel;

    public Image lvlFillBar;
    public float timeToFill, cd;
    private float barLevel;
    [SerializeField]
    private int maxLvl = 10;
    private int _baseCost;
    public bool lerpBar = false, useLastLevel = false;

    public int BaseCost
    {
        get{return _baseCost;}
        set
        {
            _baseCost = value;
            UpdateCostUI();
        }
    }

    public int BaseUpgradeLevel
    {
        get{return _baseUpgradeLevel;}
        set
        {
            if(value > maxLvl)
            {
                _baseUpgradeLevel = maxLvl;
            }
            else if (value < 0 )
            {
                _baseUpgradeLevel = 0;
            }
            else
            {
                _baseUpgradeLevel = value;
            }
            UpdateLevelUI();
        }
    }

    public void SetUpgradeVisuals(string uName, int ulvl, int uCost)
    {
        baseUpgradeName = uName;
        BaseUpgradeLevel = ulvl;
        BaseCost = uCost;

        bUpName.text = baseUpgradeName;
        bUpLevel.text = "LVL: " + ulvl;
    }

    public void SetUpgradeVisuals(int ulvl, int uCost)
    {
        BaseUpgradeLevel = ulvl;
        BaseCost = uCost;

        bUpLevel.text = "LVL: " + ulvl;
    }

    

    public void OnEnable()
    {
        UpdateBar(false);
    }

    public void UpdateBar(bool useLast)
    {
        lerpBar = true;
        cd = 0;
        useLastLevel = useLast;
        barLevel = (float)(BaseUpgradeLevel / 10f);
    }

    public void Update()
    {
        if(lerpBar)
        {
            //if lerpbar cd > 0 or somewthing
            if(cd < timeToFill )
            {
                cd += Time.deltaTime;
                if(useLastLevel)
                {
                    lvlFillBar.fillAmount = Mathf.Lerp(lvlFillBar.fillAmount, barLevel,cd);
                }
                else
                {
                    lvlFillBar.fillAmount = Mathf.Lerp(0, barLevel,cd);
                }
            }
            else
            {
                lerpBar = false;
            }
            //else lerpbar = false
        }
    }

    public void UpdateCostUI()
    {
        if(BaseCost == -1)
        {
            bUpCost.text = "MAX";
        }
        else
        {
            bUpCost.text = BaseCost.ToString();
        }
    }

    public void UpdateLevelUI()
    {
        bUpLevel.text = "LVL: " + BaseUpgradeLevel;
    }
}
