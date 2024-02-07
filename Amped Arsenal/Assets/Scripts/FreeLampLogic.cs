using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FreeLampLogic : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public Light aoeLight;
    public MeshRenderer lightMat;
    private string formatTime;
    public float countdown, cdMax;
    public bool inRange, done = false;
    private float maxLightIntensity;

    public GameObject spawnRewardPoint;

    public Color initColor;
    // Start is called before the first frame update
    void Start()
    {
        countdown = cdMax;
        formatTime = countdown.ToString("0");
        countdownText.text = formatTime;

        maxLightIntensity = aoeLight.intensity;
        aoeLight.intensity = maxLightIntensity/4;
 
        initColor = lightMat.material.GetColor("_EmissionColor");
    }

    // Update is called once per frame
    void Update()
    {
        if(inRange && done == false)
        {
            countdown -= Time.deltaTime;
            formatTime = countdown.ToString("0");
            countdownText.text = formatTime;
            aoeLight.intensity = (.25f + (1 - (countdown / cdMax))) * maxLightIntensity;
            //lightMat.material.SetColor("_EmissionColor", lightMat.material.color * ((1 - (countdown / cdMax)) * Mathf.Pow(2, 5)));
            if (countdown <= 0)
            {
                //aoeLight.intensity = 0;
                done = true;
                countdownText.text = "";
                //spawn artifact

                // get random relic and spawn reward
                GetComponent<ShootReward>().GiveRewardAndYeetIt(spawnRewardPoint);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
           inRange = true;
           aoeLight.gameObject.SetActive(true);
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            inRange = false;
            aoeLight.intensity = maxLightIntensity/4;
            countdown = cdMax;
        }
    }
}
