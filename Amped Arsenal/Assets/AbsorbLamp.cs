using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbsorbLamp : MonoBehaviour
{
    public int absorbAmount;
    public bool ableToAbsorb;
    public GameObject lampPoint;
    public Light lampLight;
    private MoveToPlayer tempXp;

    public TextMeshProUGUI countdownText;

    public void UpdateCount()
    {
        absorbAmount--;
        countdownText.text = absorbAmount.ToString();
    }

    public void Update()
    {
        if(absorbAmount <= 0)
        {
            ableToAbsorb = false;
            countdownText.text = "";
            lampLight.intensity = 0;
            //change light color? or turn off light?
        }
    }
}
