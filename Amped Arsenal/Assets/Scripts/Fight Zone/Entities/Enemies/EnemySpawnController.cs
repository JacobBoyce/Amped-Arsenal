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
    public LayerMask _layersToNotSpawnOn;
    //private Quaternion rotation;

    public float spawnRate, spawnRateMax;
    public int randomIndex;

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
                ChooseValidSpawnLocation();
                //SpawnEnemy();
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

    public void ChooseValidSpawnLocation()
    {
        bool isSpawnPosValid;
        int attemptCount;
        int maxAttempts = 200;
        randomIndex = 0;
        List<int> triedLocations = new();

        isSpawnPosValid = false;
        attemptCount = 0;
        Collider[] colliders;

        //for i = 0; i < 4; i++
        //

        //find suitable spawn location
        while(!isSpawnPosValid && attemptCount < maxAttempts)
        {
            bool isInvalidCollision = false;
            randomIndex = Random.Range(0, esPoint.Count);

            if (!triedLocations.Contains(randomIndex))
            {
                triedLocations.Add(randomIndex);

                //loop this 50 times, then get new spawn rectangle
                while(!isInvalidCollision && attemptCount < triedLocations.Count*50)
                {
                    isInvalidCollision = false;
                    UpdateSpawnPoints(esPoint[randomIndex]);

                    colliders = Physics.OverlapSphere(esPoint[randomIndex].sPoint.transform.position, 2f);

                    foreach(Collider col in colliders)
                    {
                        if(((1 << col.gameObject.layer) & _layersToNotSpawnOn) !=0)
                        {
                            isInvalidCollision = true;
                            break;
                        }
                    }

                    if(isInvalidCollision)
                    {
                        attemptCount++;
                    }
                }
            }

            if(!isInvalidCollision)
            {   
                isSpawnPosValid = true;
                break;          
            }
        }

        if(!isSpawnPosValid)
        {
            Debug.Log("couldnt find valid spawn position for - enemy to spawn");
        }
        else
        {
            //spawnstuff
            SpawnEnemy(randomIndex);
        }
    }

    public void SpawnEnemy(int randomInd)
    {
        int index = Random.Range(0, enemyPrefabs.Count);
       
        tempePrefab = Instantiate(enemyPrefabs[index],esPoint[randomInd].sPoint.transform.position, esPoint[randomInd].sPoint.transform.rotation);
        //upgrade enemy stats here
        tempePrefab.GetComponent<EnemyController>().IncreaseStats(hp,str,def,waveNum,zoneMultiplier);
    }


    public void UpdateSpawnPoints(EnemySpawnPoint es)
    {
        //foreach(EnemySpawnPoint es in esPoint)
        //{
            Vector3 range = es.transform.localScale / 2.0f;
            float x = Random.Range(-range.z, range.z); //z because the value needs to be between -.5 and .5 (which y and z are)
            es.sPoint.transform.localPosition = new Vector3(x, 0, 0);
        //}
    }
}
