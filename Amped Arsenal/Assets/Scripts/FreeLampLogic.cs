using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FreeLampLogic : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public Light lampLight, aoeLight;
    public MeshRenderer lightMat;
    private string formatTime;
    public float countdown, cdMax;
    public bool inRange, done = false;
    private float maxLightIntensity;

    public GameObject relicToSpawn, spawnRewardPoint;
    private GameObject tempRelicSpawned;
    // Start is called before the first frame update
    void Start()
    {
        countdown = cdMax;
        formatTime = countdown.ToString("0");
        countdownText.text = formatTime;
        lampLight.intensity = 14;

        maxLightIntensity = aoeLight.intensity;
        aoeLight.intensity = 0;
        aoeLight.gameObject.SetActive(false);
        lightMat.material.SetColor("_EmissionColor", lightMat.material.color * Mathf.Pow(2, 4));
    }

    // Update is called once per frame
    void Update()
    {
        if(inRange && done == false)
        {
            countdown -= Time.deltaTime;
            formatTime = countdown.ToString("0");
            countdownText.text = formatTime;
            aoeLight.intensity = (1 - (countdown / cdMax)) * maxLightIntensity;
            lightMat.material.SetColor("_EmissionColor", lightMat.material.color * ((1 - (countdown / cdMax)) * Mathf.Pow(2, 5)));
            if (countdown <= 0)
            {
                //aoeLight.intensity = 0;
                done = true;
                countdownText.text = "";
                //spawn artifact

                // get random relic
                relicToSpawn = GameZoneController.Instance.relicLibrary.relicList[Random.Range(0,GameZoneController.Instance.relicLibrary.relicList.Count)];
                tempRelicSpawned = Instantiate(relicToSpawn , spawnRewardPoint.transform.position, spawnRewardPoint.transform.rotation);
                GetComponent<ShootReward>().ShootObject(spawnRewardPoint, tempRelicSpawned, ShootReward.ShootType.Facing);
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
        }
    }

}
