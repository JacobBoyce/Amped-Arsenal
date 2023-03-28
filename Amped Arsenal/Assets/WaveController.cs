using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveController : MonoBehaviour
{
    public EnemySpawnController spawnController;
    public TextMeshProUGUI countdownText, waveText;
    public int curWave = 0;
    private int seconds;
    public bool wavesActive = false, wasPeaceWave = false;

    public List<GameObject> enemyPrefabs = new List<GameObject>();

    public List<GameObject> toSpawnList = new List<GameObject>();

    [Header("Spawn Stuff")]
    public List<EnemySpawnPoint> esPoint = new List<EnemySpawnPoint>();
    private GameObject tempPrefab;
    private int toSpawnIndex = 0;

    [Header("Stat Scaling")]
    public float hp;
    public float str,def;
    public int zoneMultiplier;

    //Time stuff
    [Header("Timers")]
    public int maxWaveTimer;
    public float mainCountdown;
    public float spawnRate, spawnRateMax;


    public bool toggleSpawning = false;
    
    [Header("Threat")]
    public int waveThreatLVL;
    public int tempThreat, increaseThreatMin, increaseThreatMax;


    

    // Start is called before the first frame update
    void Start()
    {
        mainCountdown = maxWaveTimer;
        waveText.text = "Wave " + curWave + "/50";
    }

    // Update is called once per frame
    void Update()
    {
        #region Game Timer
        //check if 0 if so restart the timer and start next wave, or if wave 5 then restart timer for peace time
        if(wavesActive == true)
        {
            if(mainCountdown >= 0)
            {
                mainCountdown -= Time.deltaTime;
                seconds = Mathf.FloorToInt(mainCountdown);
                string niceTime = string.Format("{0}", seconds);
                //string niceTime = string.Format("{0:0}:{1:00}", Min, seconds);
                countdownText.text = niceTime;
            }
            else
            {
                if(wasPeaceWave)
                {
                    //start waves
                    curWave++;
                    StartWave();
                    Debug.Log("Next wave");
                    wasPeaceWave = false;
                }
                else
                {
                    //check if wave is a multiple of 5 -> (start peace wave)
                    if(curWave % 5 == 0 || wasPeaceWave == true)
                    {
                        //if its the first wave then start the actual waves after the first 30 sec peace period
                        if(curWave == 0)
                        {
                            //start waves
                            curWave++;
                            StartWave();
                            Debug.Log("Next wave");
                        }
                        else
                        {
                            //tell shop keeper to come out
                            wasPeaceWave = true;
                            Debug.Log("Peace wave");
                        }
                    }
                    else
                    {
                        //call next wave
                        curWave++;
                        StartWave();
                        Debug.Log("Next wave");
                    }
                }
                waveText.text = "Wave " + curWave + "/50";
                mainCountdown = maxWaveTimer;
            }
        }
        #endregion
    
        #region Spawner

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

        #endregion
    }

    public void StartWave()
    {
        //raise threat level
        RaiseThreatLVL();
        DecideWave();
        //spawn enemies
        toSpawnIndex = 0;
        toggleSpawning = true;
    }

    private void DecideWave()
    {
        //clear last waves enemy spawn list
        //toSpawnList.Clear();
        tempThreat = 0;

        while(tempThreat < waveThreatLVL)
        {
            //choose enemy from list add its threat stat to tempThreat
            int randomChoice = Random.Range(0,enemyPrefabs.Count);
            GameObject tempEnemy = enemyPrefabs[randomChoice];
            Debug.Log(tempEnemy.GetComponent<EnemyController>().actorName + " chosen (" + randomChoice + ") // current threat level: " + tempThreat);

            //if the threat to add is less than the wave threat level, continue
            if(tempThreat + tempEnemy.GetComponent<EnemyController>().threatLVL <= waveThreatLVL)
            {
                //add the threat level
                tempThreat += tempEnemy.GetComponent<EnemyController>().threatLVL;
                //add to spawnlist
                toSpawnList.Add(tempEnemy);
                Debug.Log(tempEnemy.GetComponent<EnemyController>().actorName + " added // current threat level: " + tempThreat);

            }
            else
            {
                Debug.Log(tempEnemy.GetComponent<EnemyController>().actorName + " not chosen, rechoosing");
                /*break loop and start again untill one is found to fill the threat level*/
            }
            
        }
    }

    public void RaiseThreatLVL()
    {
        int rand = Random.Range(increaseThreatMin, increaseThreatMax);

        waveThreatLVL += rand;

    }

    public void SwitchWaves()
    {
        wavesActive = true;
    }

    public void SpawnEnemy()
    {
        //if tospawnlist is empty turn off togglespawning
        if(toSpawnList.Count == 0)
        {
            toggleSpawning = false;
        }
        else
        {
            int spnpoint = Random.Range(0, esPoint.Count);
       
            tempPrefab = Instantiate(toSpawnList[0],esPoint[spnpoint].sPoint.transform.position, esPoint[spnpoint].sPoint.transform.rotation);
            //upgrade enemy stats here
            tempPrefab.GetComponent<EnemyController>().IncreaseStats(hp,str,def,curWave,zoneMultiplier);
            toSpawnList.RemoveAt(0);
            //toSpawnIndex++;
        }
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
    /*
    
    -) decide threat level of each wave. 
    -) controller decides how to fill that threat level with the values
        of threat from each enemy
            -) choices can be made by choosing the biomes enemies
            -) then randomly choose an enemy from the list and add it to
                the temporary threat level 
                (this will match the wave threat level)
            -) later we can choose a bias to make waves have more or less weaker or stronger enemies
    -) decide how fast to spawn enemies

    */
}
