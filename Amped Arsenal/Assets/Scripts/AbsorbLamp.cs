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
    //public TextMeshProUGUI countdownText;
    private BarLogic bLogic;

    public void Start()
    {
        absorbAmountMax = absorbAmount;
        
        lightIntensityMax = lampLight.intensity;
        lampLight.intensity = 200;

        bLogic = GetComponentInChildren<BarLogic>();

        //countdownText.text = absorbAmount.ToString();
    }

    public void UpdateCount()
    {
        absorbAmount--;
        //countdownText.text = absorbAmount.ToString();
        //Debug.Log((1 - (absorbAmount / absorbAmountMax)) * lightIntensityMax);
        lampLight.intensity = (1 - (absorbAmount / absorbAmountMax)) * lightIntensityMax;
        bLogic.FillBar(absorbAmount, absorbAmountMax);
    }

    public void Update()
    {
        if(absorbAmount <= 0 && ableToAbsorb == true)
        {
            ableToAbsorb = false;
            //countdownText.text = "";
            lampLight.intensity = lightIntensityMax;
            //decide what relic to spawn
            //GameZoneController.Instance.
            //shoot reward
            //GameObject tempRewardObj = intantiate / Get random reward here
            GetComponent<ShootReward>().GiveRewardAndYeetIt(spawnRewardPoint);
        }
    }
}
