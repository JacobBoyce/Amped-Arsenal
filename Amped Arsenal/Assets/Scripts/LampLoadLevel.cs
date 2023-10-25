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
    public bool inRange = false, usable = true;
    private bool triggeredLoad = false;

    private float maxLightIntensity;
    //private IEnumerator coroutine;

    public void Start()
    {
        cd = cdMax;
        countdownText.text = levelName;

        maxLightIntensity = aoeLight.intensity;
        aoeLight.intensity = 0;
        aoeLight.gameObject.SetActive(false);
        lightMat.material.SetColor("_EmissionColor", lightMat.material.color * Mathf.Pow(2, 4));

        //coroutine = waveController.StartWaveSystem(5f);
    }

    public void Update()
    {
        if(inRange == true && usable == true)
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
                    inRange = false;
                    usable = false;
                    countdownText.text = "";
                    lobController.levelDifficulty = levelNum;

                    //move player
                    StartCoroutine(MovePlayer());
                    //p1.MovePlayerToField(moveToPos, true);
                }
            }
        }
    }

    public IEnumerator MovePlayer()
    {
        MainMenuController.Instance.StartFadeOut();
        //Time.timeScale = 0;
        while(MainMenuController.Instance.IsFadingOut)
        {
            yield return null;
        }

        //Time.timeScale = 1;
        p1.MovePlayerToField(moveToPos, true);
        
        //end loop
        triggeredLoad = true;
        StartCoroutine(AfterMovePlayer());

        waveController.waveUIObj.SetActive(true);
        waveController.StartWaveSystem();
    }

    public IEnumerator AfterMovePlayer()
    {
        MainMenuController.Instance.StartFadeIn();
        lobController.ToggleLamps(false);

        //Time.timeScale = 0;
        while(MainMenuController.Instance.IsFadingIn)
        {
            yield return null;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(usable)
            {
                if(levelNum <= MainMenuController.Instance.stageDifficulty)
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
            if(usable)
            {
                inRange = false;
                aoeLight.gameObject.SetActive(false);
                lightMat.material.SetColor("_EmissionColor", lightMat.material.color * Mathf.Pow(2, 4));
                cd = cdMax;
                countdownText.text = levelName;
                aoeLight.intensity = 0;
            }
            else
            {
                countdownText.text = "";
            }
        }
        
    }
}
