using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FreeLampLogic : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public Light lampLight;
    private string formatTime;
    public float countdown, cdMax;
    public bool inRange, done = false;

    public GameObject relicToSpawn, spawnRewardPoint;
    private GameObject tempRelicSpawned;
    // Start is called before the first frame update
    void Start()
    {
        countdown = cdMax;
        formatTime = countdown.ToString("0");
        countdownText.text = formatTime;
        lampLight.intensity = 14;
    }

    // Update is called once per frame
    void Update()
    {
        if(inRange && done == false)
        {
            countdown -= Time.deltaTime;
            formatTime = countdown.ToString("0");
            countdownText.text = formatTime;
            if (countdown <= 0)
            {
                lampLight.intensity = 0;
                done = true;
                countdownText.text = "";
                //spawn artifact
                tempRelicSpawned = Instantiate(relicToSpawn , spawnRewardPoint.transform.position, spawnRewardPoint.transform.rotation);
                GetComponent<ShootReward>().ShootObject(spawnRewardPoint, tempRelicSpawned);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
           inRange = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            inRange = false;
        }
    }

}
