using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LobbyController : MonoBehaviour
{
    public WaveController waveController;
    public ExfilLampLogic exfilLamp;
    public int levelDifficulty;
    public List<LampLoadLevel> lamps;
    public GameObject globalLobbyLight, mainMenuLightLamp;
    // Start is called before the first frame update
    void Start()
    {
        //waveController.zoneMultiplier = levelDifficulty;
        waveController.waveUIObj.SetActive(false);

        //set lamps activatable stuff here
        //if player pref is true then show main menu lamp

        mainMenuLightLamp.SetActive(false);
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

    
}
