using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuLamp : MonoBehaviour
{
    public MeshRenderer lightMat;
    public TextMeshProUGUI countdownText;
    public bool inRange = false, triggeredLoad = false;
    private PlayerController p1;
    public Light aoeLight;
    public float exfilPercentAmount, cd, cdMax;
    public string exfilMessage;
    private string formatTime;
    private float maxLightIntensity;

    public void Start()
    {
        cd = cdMax;
        countdownText.text = exfilMessage;

        maxLightIntensity = aoeLight.intensity;
        aoeLight.intensity = 0;
        aoeLight.gameObject.SetActive(false);
        lightMat.material.SetColor("_EmissionColor", lightMat.material.color * Mathf.Pow(2, 4));
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
                lightMat.material.SetColor("_EmissionColor", lightMat.material.color * ((1 - (cd / cdMax)) * Mathf.Pow(2, 5)));
            }
            else
            {
                if(triggeredLoad == false)
                {
                    countdownText.text = "";
                    int temp = Mathf.RoundToInt(p1._stats["gold"].Value * ((PlayerPrefs.GetInt("Inflation") / 10) + exfilPercentAmount));
                    Debug.Log("returning with " + temp + " gold");
                    MainMenuController.Instance._playerGold += temp;
                    triggeredLoad = true;
                    SceneManager.LoadSceneAsync("MainMenu");
                    
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
            aoeLight.gameObject.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            inRange = false;
            countdownText.text = exfilMessage;
            aoeLight.gameObject.SetActive(false);
            lightMat.material.SetColor("_EmissionColor", lightMat.material.color * Mathf.Pow(2, 4));
            cd = cdMax;
            aoeLight.intensity = 0;
        }
    }
}
