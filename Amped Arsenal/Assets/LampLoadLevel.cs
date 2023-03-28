using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LampLoadLevel : MonoBehaviour
{
    public WaveController waveController;
    public GameObject moveToPos;
    public Light lampLight;
    public TextMeshProUGUI countdownText;
    private string formatTime;
    private PlayerController p1;

    public float cd, cdMax;
    public bool inRange = false;
    private bool triggeredLoad = false;

    public void Start()
    {
        cd = cdMax;
        countdownText.text = "";
    }

    public void Update()
    {
        if(inRange == true)
        {
            if(cd > 0)
            {
                cd -= Time.deltaTime;
                formatTime = cd.ToString("0");
                countdownText.text = formatTime;
            }
            else
            {
                if(triggeredLoad == false)
                {
                    countdownText.text = "";
                    p1.MovePlayerToField(moveToPos);
                    Debug.Log("move to battlefield");

                    waveController.SwitchWaves();

                    triggeredLoad = true;
                }
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            inRange = true;
            p1 = other.gameObject.GetComponent<PlayerController>();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            inRange = false;
            cd = cdMax;
            countdownText.text = "";
        }
    }
}
