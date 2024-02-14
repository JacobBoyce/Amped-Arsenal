using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatUpInfoSlot : MonoBehaviour
{
    public string upgradeName;
    public Image upgradeCurrent, upgradeTo;
    public float minVal, maxVal, curVal, nextVal;

    public void SetFillAmount(int minAmount, int currentAmount, int nextAmount, int maxAmount)
    {
        minVal = minAmount;
        maxVal = maxAmount;
        curVal = currentAmount;
        nextVal = nextAmount;

        float currentOffset = currentAmount - minAmount;
        float nextOffset = nextAmount - minAmount;

        float maxOffset = maxAmount - minAmount;

        float fillAmtCurrent = currentOffset / maxOffset;
        float fillAmtNext = nextOffset / maxOffset;

        if(currentAmount > nextAmount)
        {
            upgradeCurrent.fillAmount = fillAmtNext;
            upgradeTo.fillAmount = fillAmtCurrent;
        }
        else
        {
            upgradeCurrent.fillAmount = fillAmtCurrent;
            upgradeTo.fillAmount = fillAmtNext;
        }
    }
}
