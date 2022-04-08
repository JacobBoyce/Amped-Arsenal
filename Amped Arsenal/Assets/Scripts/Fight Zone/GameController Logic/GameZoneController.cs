using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.IO;
using TMPro;
using UnityEngine.UI;

public class GameZoneController : MonoBehaviour
{
    public PlayerController p1;
    public GameObject statsPanel;
    public TextMeshProUGUI statsTxt;

    public bool statsVisible;

    // Start is called before the first frame update
    void Start()
    {
        statsVisible = false;
        ShowStats(statsVisible);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            statsVisible = statsVisible ? false : true;
            ShowStats(statsVisible);
        }

        statsTxt.text = "HP: " + p1._stats["hp"].Value + " / " + p1._stats["hp"].Max
                        + "\nSTR: " + p1._stats["str"].Value
                        + "\nDEF: " + p1._stats["def"].Value
                        + "\nSPD: " + p1._stats["spd"].Value
                        + "\nLUCK: " + p1._stats["luck"].Value
                        + "\nPULL: " + p1._stats["pull"].Value;
        
        if(Input.GetKeyDown(KeyCode.D))
        {
            p1.TakeDamage(Random.Range(1,11));
        }
    }

    public void ShowStats(bool onoff)
    {
        statsPanel.SetActive(onoff);
    }
}
