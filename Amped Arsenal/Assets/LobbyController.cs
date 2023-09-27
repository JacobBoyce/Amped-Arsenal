using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyController : MonoBehaviour
{
    public WaveController waveController;
    public int levelDifficulty;
    public List<LampLoadLevel> lamps;
    public GameObject globalLobbyLight;
    // Start is called before the first frame update
    void Start()
    {
        levelDifficulty = PlayerPrefs.GetInt("StageDifficulty");
        waveController.zoneMultiplier = levelDifficulty;

        //set lamps activatable stuff here
    }

    public void DeactivateLamps()
    {
        foreach(LampLoadLevel lamp in lamps)
        {
            lamp.gameObject.SetActive(false);
        }
        globalLobbyLight.SetActive(false);

        GameZoneController.Instance.ToggleFightZoneLights(true);
    }
}
