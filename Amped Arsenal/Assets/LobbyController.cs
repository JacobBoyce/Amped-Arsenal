using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbyController : MonoBehaviour
{
    public WaveController waveController;
    public ExfilLampLogic exfilLamp;

    public GameObject lobbyTerrain;
    public GameObject frogShop;
    public int levelDifficulty;
    public List<LampLoadLevel> lamps;
    public GameObject globalLobbyLight, mainMenuLightLamp, middleSpawnPoint;
    public List<GameObject> uiToInitAfterLoadingScreen;
    // Start is called before the first frame update
    void Start()
    {
        //waveController.zoneMultiplier = levelDifficulty;
        waveController.waveUIObj.SetActive(false);
        GameZoneController.Instance.inLobby = true;
        //set lamps activatable stuff here
        //if player pref is true then show main menu lamp

        mainMenuLightLamp.SetActive(false);

        foreach(GameObject go in uiToInitAfterLoadingScreen)
        {
            go.SetActive(false);
        }
    }

    public void Update()
    {
        if(GameSceneManager.instance.triggerEndLoadingScreen)
        {
            
            GameSceneManager.instance.triggerEndLoadingScreen = false;
            foreach(GameObject go in uiToInitAfterLoadingScreen)
            {
                go.SetActive(true);
            }
            GameZoneController.Instance.p1.GetComponent<PlayerInput>().ActivateInput();
        }
    }

    public void ToggleLamps(bool toggle)
    {
        //set level difficulty so the next lamp can be used
        waveController.zoneMultiplier = levelDifficulty;
        

        foreach(LampLoadLevel lamp in lamps)
        {
            lamp.gameObject.SetActive(toggle);
        }
        globalLobbyLight.SetActive(toggle);
        mainMenuLightLamp.SetActive(toggle);

        //turn on fight zone stuff
        GameZoneController.Instance.ToggleFightZoneLights(!toggle);

        

        //if going back to the lobby
        if(toggle == true)
        {
            exfilLamp.ResetExfilLamp();
        }
    }

    public void DeactivateTerrains()
    {
        foreach(LampLoadLevel lamp in lamps)
        {
            lamp.terrainToLoad.SetActive(false);
        }
    }    
}
