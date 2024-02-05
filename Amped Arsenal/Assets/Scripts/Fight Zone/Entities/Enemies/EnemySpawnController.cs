using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
        //POSSIBLE OBJECT POOLING FOR ENEMIES
    public bool toggleSpawning;

    public GameZoneController controller;
    public GameObject enemyPrefab, tempePrefab;
    public List<GameObject> enemyPrefabs = new();
    public List<EnemySpawnPoint> esPoint = new();
    //private Quaternion rotation;

    public float spawnRate, spawnRateMax;

    public int waveNum, checkMinChange, zoneMultiplier;

    [Space(10)]
    [Header("Stat Scaling")]
    public float hp;
    public float str,def;

    public void Start()
    {
        checkMinChange = controller.minutes;
    }

    // Update is called once per frame
    void Update()
    {
        if(toggleSpawning)
        {
            if(spawnRate < spawnRateMax)
            {
                spawnRate += Time.deltaTime;
            }
            else
            {
                spawnRate = 0;
                UpdateSpawnPoints();
                SpawnEnemy();
            }
        }
        

        //if(checkMinChange != controller.minutes)
        //{
        //    checkMinChange = controller.minutes;
            //mintute incremented
            //increase scale of enemy stats
            waveNum = controller.minutes;
        //}
    }

    public void SpawnEnemy()
    {
        int index = Random.Range(0, enemyPrefabs.Count);
        int spnpoint = Random.Range(0, esPoint.Count);
       
        tempePrefab = Instantiate(enemyPrefabs[index],esPoint[spnpoint].sPoint.transform.position, esPoint[spnpoint].sPoint.transform.rotation);
        //upgrade enemy stats here
        tempePrefab.GetComponent<EnemyController>().IncreaseStats(hp,str,def,waveNum,zoneMultiplier);
    }


    public void UpdateSpawnPoints()
    {
        foreach(EnemySpawnPoint es in esPoint)
        {
            Vector3 range = es.transform.localScale / 2.0f;
            float x = Random.Range(-range.z, range.z); //z because the value needs to be between -.5 and .5 (which y and z are)
            es.sPoint.transform.localPosition = new Vector3(x, 0, 0);
        }
    }
}
