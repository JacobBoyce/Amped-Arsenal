using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExfilLampLogic : MonoBehaviour
{
    public bool inRange = false, exfilTime = false;
    public int minuteTimer = 0, intervals, lightIntensity;

    [Space(10)]
    public float shortCdMax;
    public float shortCd;
    [Space(10)]
    public float longCdMax = 60f;
    public float longCd = 0;
    [Space(10)]
    public Light lampLight;
    public TextMeshProUGUI countdownText;
    private string formatTime;



    //Large cooldown starts every 5 min and lasts for 1 min
    //short cooldown starts when player is in the circle

    public void Start()
    {
        GameZoneController.Instance.OnMinutesChanged += MinIncremented;
    }
    // Update is called once per frame
    void Update()
    {
        //when minutes % 5 == 0 start large cooldown
        if(exfilTime == true)
        {
            if(longCd < longCdMax)
            {
                longCd += Time.deltaTime;
            }
            else
            {
                if(!inRange)
                {
                    exfilTime = false;
                    longCd = 0;
                    lampLight.intensity = 0;
                    //turn off light
                }
            }

            if(inRange == true)
            {
                if(shortCd > 0)
                {
                    shortCd -= Time.deltaTime;
                    formatTime = shortCd.ToString("0");
                    countdownText.text = formatTime;
                }
                else
                {
                    //exfil code here!
                }
            }
        }
        //when long cooldown is over check if in range then turn off light and exfil opportunity
    }

    private void MinIncremented(int newVal)
    {
        minuteTimer = newVal;
        if(minuteTimer % intervals == 0)
        {
            exfilTime = true;
            //turn on light
            lampLight.intensity = lightIntensity;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (exfilTime == true)
            {
                //start short cooldown
                inRange = true;
            }
        }
        
        //start short countdown
        //stop long cooldown?
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //stop short count down
            if (exfilTime == true)
            {
                //start short cooldown
                inRange = false;
                shortCd = shortCdMax;
                countdownText.text = "";
            }
        }
    }
}
