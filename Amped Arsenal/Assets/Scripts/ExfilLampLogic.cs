using System.Collections;
using UnityEngine;
using TMPro;

public class ExfilLampLogic : MonoBehaviour
{
    public GameZoneController gzController;
    public WaveController waveController;
    public LobbyController lobController;
    public POIController poiController;
    private BarLogic bLogic;
    
    public MeshRenderer lightMat;
    public GameObject moveToPos;
    public bool inRange = false, exfilTime = false, triggeredLoad = false;
    private PlayerController p1;

    public float cdMax, cd;
    [Space(10)]
    //public TextMeshProUGUI countdownText;
    private string formatTime;

    public void Start()
    {
        cd = cdMax;
        //countdownText.text = "";
        bLogic = GetComponentInChildren<BarLogic>();
    }


    void Update()
    {
        if(exfilTime == true)
        {
            if(inRange == true)
            {
                if(cd > 0)
                {
                    cd -= Time.deltaTime;
                    //formatTime = cd.ToString("0");
                    //countdownText.text = formatTime;
                }
                else
                {
                    if(triggeredLoad == false)
                    {
                        //countdownText.text = "";
                        triggeredLoad = true;
                        exfilTime = false;
                        waveController.startExfilDelay = false;
                        

                        //set player pref flags to tell new scene that you exfilled
                        StartCoroutine(MovePlayer());
                    }
                }
                bLogic.FillBar(cd, cdMax);
            }
        }
    }

    public IEnumerator MovePlayer()
    {
        //gzController.StartFadeOut();
        GameSceneManager.instance.musicMaker.SwapTrack(GameSceneManager.instance.musicMaker.mainMenuMusic);
        gzController.StartFadeOut();
        gzController.inLobby = true;
        //Time.timeScale = 0;
        while(gzController.IsFadingOut)
        {
            yield return null;
        }
        //Time.timeScale = 1;
        p1.MovePlayerToField(moveToPos, false);
        waveController.DeactivateWaveSystem();
        gzController.gameObject.GetComponent<GlobalVolumeController>().StopAllCoroutines();
        gzController.gameObject.GetComponent<GlobalVolumeController>().ReturnToNormalColor();
        
        lobController.lobbyTerrain.SetActive(true);
        lobController.ToggleLamps(true);
        if(MainMenuController.Instance.stageDifficulty == waveController.zoneMultiplier)
        {
            MainMenuController.Instance.stageDifficulty++;
            MainMenuController.Instance.SaveProgress();
        }

        StartCoroutine(AfterMovePlayer());
    }

    public IEnumerator AfterMovePlayer()
    {
        waveController.gvController.ReturnToNormalColor();
        gzController.StartFadeIn();
        //Time.timeScale = 0;
        while(gzController.IsFadingIn)
        {
            yield return null;
        }

         //delete all xp and gold
        GameObject[] xpObj = GameObject.FindGameObjectsWithTag("XP");
        foreach(GameObject go in xpObj)
        {
            ObjectPoolManager.ReturnObjectToPool(go);
        }

        GameObject[] goldObj = GameObject.FindGameObjectsWithTag("Gold");
        foreach(GameObject go in goldObj)
        {
            ObjectPoolManager.ReturnObjectToPool(go);
        }
        lobController.DeactivateTerrains();
        poiController.CleanUpEvents();
    }

    public void ResetExfilLamp()
    {
        triggeredLoad = false;
        exfilTime = false;
        inRange = false;
        cd = cdMax;
        bLogic.FillBar(cd, cdMax);
        //countdownText.text = "";
    }

    public void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if (exfilTime == true)
            {
                p1 = other.GetComponent<PlayerController>();
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
                inRange = false;
                cd = cdMax;
                bLogic.FillBar(cd, cdMax);
                //formatTime = cd.ToString("0");
                //countdownText.text = formatTime;
            }
        }
    }
}
