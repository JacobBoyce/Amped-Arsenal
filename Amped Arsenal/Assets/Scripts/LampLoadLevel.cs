using System.Collections;
using UnityEngine;
using TMPro;

public class LampLoadLevel : MonoBehaviour
{
    public GameZoneController gzController;
    public WaveController waveController;
    public LobbyController lobController;
    public POIController poiController;
    public GameObject lampLevelSpawnZone;
    public GameObject exfilLamp, terrainToLoad;
    public GameObject moveToPos, shopStartPos;
    public Light aoeLight;
    public MeshRenderer lightMat;
    public TextMeshProUGUI countdownText;
    public string levelName;
    public int levelNum;
    private string formatTime;
    private PlayerController p1;

    public float cd, cdMax;
    public bool inRange = false, usable = true;
    private bool triggeredLoad = false;

    private float maxLightIntensity, baseLightIntensity;
    public Color initColor;
    //private IEnumerator coroutine;

    public void Start()
    {
        cd = cdMax;
        countdownText.text = levelName;
        maxLightIntensity = aoeLight.intensity;
        aoeLight.intensity = maxLightIntensity/2;
        //aoeLight.gameObject.SetActive(false);
        initColor = lightMat.material.GetColor("_EmissionColor");
        //lightMat.material.SetColor("_EmissionColor", initColor /** Mathf.Pow(2, 4)*/);

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
                aoeLight.intensity = (.5f + (1 - (cd / cdMax))) * maxLightIntensity;
                //aoeLight.intensity += 2f;
                //lightMat.material.SetColor("_EmissionColor", initColor * (1 - (cd / cdMax)) * Mathf.Pow(2, 2));
            }
            else
            {
                if(triggeredLoad == false)
                {
                    inRange = false;
                    usable = false;
                    countdownText.text = "";
                    //set difficulty
                    lobController.levelDifficulty = levelNum;

                    //set up the correct terrain and deactivate others
                    lobController.DeactivateTerrains();
                    terrainToLoad.SetActive(true);

                    //set the shops location for the right area
                    lobController.frogShop.GetComponent<ShopMovement>().startpos = shopStartPos;
                    //setup the correct exfil lamp to end the level
                    waveController.exfilLampObject = exfilLamp.GetComponent<ExfilLampLogic>();

                    //Set the POI controllers spawn points
                    poiController.InitPOIPositions(lampLevelSpawnZone);
                    //move player
                    StartCoroutine(MovePlayer());
                }
            }
        }
    }

    public IEnumerator MovePlayer()
    {
        gzController.StartFadeOut();
        GameSceneManager.instance.musicMaker.SwapTrack();
        //Time.timeScale = 0;
        while(gzController.IsFadingOut)
        {
            yield return null;
        }

        //Time.timeScale = 1;
        p1.MovePlayerToField(moveToPos, true);
        yield return new WaitForSecondsRealtime(1f);
        //end loop
        triggeredLoad = true;
        StartCoroutine(AfterMovePlayer());

        waveController.waveUIObj.SetActive(true);
        waveController.StartWaveSystem();
    }

    public IEnumerator AfterMovePlayer()
    {
        
        lobController.ToggleLamps(false);
        //yield return new WaitForSeconds(2f);
        gzController.StartFadeIn();
        //Time.timeScale = 0;
        while(gzController.IsFadingIn)
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
                aoeLight.intensity = maxLightIntensity/2;
                //lightMat.material.SetColor("_EmissionColor", initColor /** Mathf.Pow(2, 4)*/);
                cd = cdMax;
                countdownText.text = levelName;
            }
            else
            {
                countdownText.text = "";
            }
        }
        
    }
}
