using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbsorbLamp : MonoBehaviour
{
    public float absorbAmount, absorbAmountMax, lightIntensityMax;
    public bool ableToAbsorb;
    public GameObject spawnRewardPoint;
    public Light lampLight;
    private MoveToPlayer tempXp;
    public GameObject testPrefab, tempRewardObj;

    public TextMeshProUGUI countdownText;

    public void Start()
    {
        absorbAmountMax = absorbAmount;
        lampLight.intensity = 0;
        countdownText.text = absorbAmount.ToString();
    }

    public void UpdateCount()
    {
        absorbAmount--;
        countdownText.text = absorbAmount.ToString();
        //Debug.Log((1 - (absorbAmount / absorbAmountMax)) * lightIntensityMax);
        lampLight.intensity = (1 - (absorbAmount / absorbAmountMax)) * lightIntensityMax;
    }

    public void Update()
    {
        if(absorbAmount <= 0 && ableToAbsorb == true)
        {
            ableToAbsorb = false;
            countdownText.text = "";
            lampLight.intensity = 0;
            //decide what relic to spawn
            //GameZoneController.Instance.
            //shoot reward
            //GameObject tempRewardObj = intantiate / Get random reward here
            GetComponent<ShootReward>().GiveRewardAndYeetIt(spawnRewardPoint);
        }
    }
}
