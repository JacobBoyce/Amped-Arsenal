using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WaveController : MonoBehaviour
{
    public EnemySpawnController spawnController;
    public TextMeshProUGUI countdownText, waveText;
    public Image countdownBarUI;
    public int curWave = 0, maxWave;
    public float startWaitTime;
    public bool wavesActive = false, wasPeaceWave = false;

    public List<GameObject> enemyPrefabs = new List<GameObject>();

    public List<GameObject> beginningSpawnList = new List<GameObject>();
    public List<GameObject> midAndLateGameSpwnList = new List<GameObject>();

    [Header("Spawn Stuff")]
    public List<EnemySpawnPoint> esPoint = new List<EnemySpawnPoint>();
    private GameObject tempPrefab;

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



    // Start is called before the first frame update
    void Start()
    {
        waveText.text = "Wave " + curWave + "/" + maxWave;
        mainCountdown = maxWaveTimer;
    }

    // Update is called once per frame
    void Update()
    {
        #region Game Timer
        //start the waves
        if(wavesActive == true)
        {
            //check if current wave is less than the max amount
            if(curWave < maxWave)
            {
                if(mainCountdown >= 0)
                {
                    //Countdown the wave time
                    mainCountdown -= Time.deltaTime;
                    //update the countdown bar UI
                    countdownBarUI.fillAmount = 1 - (mainCountdown / maxWaveTimer);

                    //seconds = Mathf.FloorToInt(mainCountdown);
                    //string niceTime = string.Format("{0}", seconds);
                    //string niceTime = string.Format("{0:0}:{1:00}", Min, seconds);
                    //countdownText.text = niceTime;
                }
                else
                {
                    //decide the next wave
                    curWave++;
                    mainCountdown = maxWaveTimer;
                    StartNextWave();
                }
            }
            //if the end of the max waves
            else if(curWave > maxWave)
            {
                //end game stuff
                //open menu for player to choose to go to next lamp or go to the main menu
                wavesActive = false;
                PlayerPrefs.SetInt("StageDifficulty", PlayerPrefs.GetInt("StageDifficulty") + 1);
            }

                    //check if wave is a multiple of 5 -> (start peace wave)
                    //if(curWave % 5 == 0 )
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

    public void StartNextWave()
    {
        //update wave UI
        waveText.text = "Wave " + curWave + "/" + maxWave;

        DecideWave();
        //spawn enemies
        toggleSpawning = true;
    }

    private void DecideWave()
    {
        //if wave 1-10 use single enemy list defined by me
        if(curWave >= 11 && curWave <= 15)
        {
            int tryCount = 0;
            int enemiesSelected = 0;
            int randomChoice;
            midAndLateGameSpwnList.Clear();

            randomChoice = Random.Range(0,enemyPrefabs.Count);

            midAndLateGameSpwnList.Add(enemyPrefabs[randomChoice]);
            Debug.Log("added: " + enemyPrefabs[randomChoice].GetComponent<EnemyController>().actorName);
            enemiesSelected++;

            while(enemiesSelected < 2 && tryCount < 200)
            {  
                randomChoice = Random.Range(0,enemyPrefabs.Count);
                Debug.Log("Checking if this is in list already: " + enemyPrefabs[randomChoice].GetComponent<EnemyController>().actorName);
                if(!midAndLateGameSpwnList.Contains(enemyPrefabs[randomChoice]))
                {
                    Debug.Log("added: " + enemyPrefabs[randomChoice].GetComponent<EnemyController>().actorName);
                    midAndLateGameSpwnList.Add(enemyPrefabs[randomChoice]);
                    enemiesSelected++;
                }
                Debug.Log("enemies selected Count: " + enemiesSelected);
                tryCount++;
            }
        }
        else if(curWave >= 16 && curWave <= 20)
        {
            int tryCount = 0;
            int enemiesSelected = 0;
            int randomChoice;
            midAndLateGameSpwnList.Clear();

            randomChoice = Random.Range(0,enemyPrefabs.Count);

            midAndLateGameSpwnList.Add(enemyPrefabs[randomChoice]);
            enemiesSelected++;

            while(enemiesSelected < 3 && tryCount < 200)
            {  
                randomChoice = Random.Range(0,enemyPrefabs.Count);

                if(!midAndLateGameSpwnList.Contains(enemyPrefabs[randomChoice]))
                {
                    midAndLateGameSpwnList.Add(enemyPrefabs[randomChoice]);
                    enemiesSelected++;
                }
                tryCount++;
            }
            
        }
        //if wave 10-15 use 2 random enemies from the list
        //if wave 15-20 use 3 or more random enemies from the list
            //int randomChoice = Random.Range(0,enemyPrefabs.Count);
            //GameObject tempEnemy = enemyPrefabs[randomChoice];
 

        //spawnRateMax = (maxWaveTimer - (maxWaveTimer * .25f)) / toSpawnList.Count; 
    }



    public IEnumerator StartWaveSystem()
    {
        yield return new WaitForSeconds(startWaitTime);
        StartNextWave();
        wavesActive = true;
    }

    public void SpawnEnemy()
    {

        int spnpoint = Random.Range(0, esPoint.Count);
    
        if(curWave <= 10)
        {
            tempPrefab = Instantiate(beginningSpawnList[curWave-1],esPoint[spnpoint].sPoint.transform.position, esPoint[spnpoint].sPoint.transform.rotation);
        }
        else if(curWave >= 11)
        {
            int randEnemy = Random.Range(0, midAndLateGameSpwnList.Count);
            tempPrefab = Instantiate(midAndLateGameSpwnList[randEnemy],esPoint[spnpoint].sPoint.transform.position, esPoint[spnpoint].sPoint.transform.rotation);
        }

        //upgrade enemy stats here
        tempPrefab.GetComponent<EnemyController>().IncreaseStats(hp,str,def,curWave,zoneMultiplier);
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
    
    -) 20 waves
    -) 30 second waves
    -) spawn a specific enemy or defined group (SO?) at a specific spawn rate
    -) rate increases each wave or depending on enemy type
    -) every N waves spawn a mega creep
    -) 

    */
}
