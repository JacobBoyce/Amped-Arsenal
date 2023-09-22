using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LampLoadLevel : MonoBehaviour
{
    public WaveController waveController;
    public LobbyController lobController;
    public GameObject moveToPos;
    public Light lampLight, aoeLight;
    public MeshRenderer lightMat;
    public TextMeshProUGUI countdownText;
    public string levelName;
    public int levelNum;
    private string formatTime;
    private PlayerController p1;

    public float cd, cdMax;
    public bool inRange = false;
    private bool triggeredLoad = false;

    private float maxLightIntensity;

    public void Start()
    {
        cd = cdMax;
        countdownText.text = levelName;

        maxLightIntensity = aoeLight.intensity;
        aoeLight.intensity = 0;
        aoeLight.gameObject.SetActive(false);
        lightMat.material.SetColor("_EmissionColor", lightMat.material.color * Mathf.Pow(2, 1));
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
                aoeLight.intensity = (1 - (cd / cdMax)) * maxLightIntensity;
                lightMat.material.SetColor("_EmissionColor", lightMat.material.color * ((1 - (cd / cdMax)) * Mathf.Pow(2, 3)));
            }
            else
            {
                if(triggeredLoad == false)
                {
                    countdownText.text = "";
                    p1.MovePlayerToField(moveToPos);
                    Debug.Log("move to battlefield");

                    waveController.SwitchWaves();
                    lobController.DeactivateLamps();
                    triggeredLoad = true;
                }
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(levelNum <= lobController.levelDifficulty)
            {
                inRange = true;
                p1 = other.gameObject.GetComponent<PlayerController>();
                aoeLight.gameObject.SetActive(true);
            }
            else
            {
                countdownText.text = "X";
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            inRange = false;
            aoeLight.gameObject.SetActive(false);
            lightMat.material.SetColor("_EmissionColor", lightMat.material.color * Mathf.Pow(2, 1));
            cd = cdMax;
            countdownText.text = levelName;
            aoeLight.intensity = 0;
        }
    }
}
