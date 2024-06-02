using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Den.Tools;

public class WaveController : MonoBehaviour
{
    public EnemySpawnController spawnController;
    public GlobalVolumeController gvController;
    public GameObject waveUIObj;
    public TextMeshProUGUI infoMessages;
    public Image countdownBarUI;
    public int curWave = 0, maxWave;
    public float startWaitTime;
    public bool wavesActive = false, exfilPhase = false;
    public ExfilLampLogic exfilLampObject;
    public GameObject enemyParentObj;
    [Header("Moon Stuff")]
    public float endAngle = 237;
    public float totalWaveTime;
    public RectTransform moons;

    public List<GameObject> enemyPrefabs = new List<GameObject>();

    public List<GameObject> beginningSpawnList = new List<GameObject>();
    public List<GameObject> midAndLateGameSpwnList = new List<GameObject>();

    [Header("Spawn Stuff")]
    public List<EnemySpawnPoint> esPoint = new List<EnemySpawnPoint>();
    private GameObject tempPrefab;
    public bool spawnLargeTrigger = false;

    [Header("Stat Scaling")]
    public float hp;
    public float str,def;
    public int zoneMultiplier;
    public float[] scaleLampLevel;

    //Time stuff
    [Header("Timers")]
    public int maxWaveTimer;
    public float mainCountdown;
    public float spawnRate, spawnRateMax, seconds;
    public int min;   
    public bool toggleSpawning = false;

    public float delayStartMax, delayStart, delayExfilMax, delayExfil;
    public bool startDelay, startExfilDelay;
    private int randomIndex;
    public LayerMask _layersToNotSpawnOn;


    // Start is called before the first frame update
    void Start()
    {
        //waveText.text = "Wave " + curWave + "/" + maxWave;
        totalWaveTime = (maxWave-1)*maxWaveTimer;
        mainCountdown = maxWaveTimer;

        delayStart = delayStartMax;
        delayExfil = delayExfilMax;


        
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
                if (totalWaveTime > 0)
                {
                    float t = 1 - (totalWaveTime / ((maxWave-1) * maxWaveTimer));
                    float currentRotation = Mathf.Lerp(0, endAngle, t);
                    moons.localRotation = Quaternion.Euler(0, 0, currentRotation);
                    totalWaveTime -= Time.deltaTime;
                }

                if(mainCountdown >= 0)
                {
                    //Countdown the wave time
                    mainCountdown -= Time.deltaTime;
                    //update the countdown bar UI
                    
                    //countdownBarUI.fillAmount = 1 - (mainCountdown / maxWaveTimer);

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
            else if(curWave >= maxWave)
            {
                //end game stuff
                //open menu for player to choose to go to next lamp or go to the main menu
                wavesActive = false;
                infoMessages.gameObject.SetActive(true);
                mainCountdown = 0;
                exfilLampObject.exfilTime = true;
                
                
                //PlayerPrefs.GetInt("StageDifficulty") == zoneMultiplier
                
                if(MainMenuController.Instance.stageDifficulty == zoneMultiplier)
                {
                    MainMenuController.Instance.stageDifficulty++;
                    MainMenuController.Instance.SaveProgress();
                }
                
               //start exfill phase
               startExfilDelay = true;
               toggleSpawning = false;
               gvController.StartRedFade();
            }

                //check if wave is a multiple of 5 -> (start peace wave)
                //if(curWave % 5 == 0 )
        }
        #endregion

        #region Exfil timer
        if(exfilPhase)
        {
            mainCountdown += Time.deltaTime;
            seconds = Mathf.FloorToInt(mainCountdown) % 60;
    
            if (seconds == 0 && Mathf.FloorToInt(mainCountdown) != 0)
            {
                min++;
                mainCountdown = 0f;
            }

            string niceTime = string.Format("{0:0}:{1:00}", min, seconds);
            infoMessages.text = niceTime;
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
                ChooseValidSpawnLocation();
            }
        }
        #endregion

        #region Delay Start
        if(startDelay)
        {
            if(delayStart > 0)
            {
                delayStart -= Time.deltaTime;
                seconds = Mathf.FloorToInt(delayStart);
                string niceTime = "The night starts in: " + string.Format("{0}", seconds);
                //string niceTime = string.Format("{0:0}:{1:00}", min, seconds);
                infoMessages.text = niceTime;
            }
            else
            {
                startDelay = false;

                infoMessages.gameObject.SetActive(false);
                curWave = 1;
                spawnRateMax = 2;
                spawnRate = spawnRateMax;
                //waveText.text = "Wave " + curWave + "/" + maxWave;
                mainCountdown = maxWaveTimer;
                seconds = 0;
                min = 0;
                waveUIObj.SetActive(true);
                StartNextWave();
                wavesActive = true;
            }
        }
        #endregion
   
        #region Delay Exfil
        if(startExfilDelay)
        {
            if(delayExfil > 0)
            {
                delayExfil -= Time.deltaTime;
                seconds = Mathf.FloorToInt(delayExfil);
                string niceTime = "Return to the camp to escape the Blood moon!\n" + string.Format("{0}", seconds);
                //string niceTime = string.Format("{0:0}:{1:00}", min, seconds);
                infoMessages.text = niceTime;
            }
            else
            {
                startExfilDelay = false;
                seconds = 0;
                exfilPhase = true;
                toggleSpawning = true;
                spawnRateMax = .05f;
                
            }
        }
        #endregion
    }

    public void StartNextWave()
    {
        //update wave UI
        //waveText.text = "Wave " + curWave + "/" + maxWave;
        spawnRateMax -= .1f;
        DecideWave();
        //spawn enemies
        toggleSpawning = true;
        spawnLargeTrigger = false;
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

            enemiesSelected++;

            while(enemiesSelected < 2 && tryCount < 200)
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
    public void StartWaveSystem()
    {
        startDelay = true;
        //waveText.gameObject.SetActive(false);
        infoMessages.gameObject.SetActive(true);
        delayStart = delayStartMax;
        delayExfil = delayExfilMax;

        moons.localRotation = Quaternion.Euler(0,0,0);
        totalWaveTime = (maxWave-1)*maxWaveTimer;
        mainCountdown = maxWaveTimer;
    }

    public void DeactivateWaveSystem()
    {
        waveUIObj.SetActive(false);
        wavesActive = false;
        toggleSpawning = false;
        exfilPhase = false;

        foreach(Transform child in enemyParentObj.transform)
        {
            Destroy(child.gameObject);
        }
    }
    public void SpawnEnemy(int spwnIndex)
    {
    
        if(curWave <= 10)
        {
            tempPrefab = Instantiate(beginningSpawnList[curWave-1],esPoint[spwnIndex].sPoint.transform.position, esPoint[spwnIndex].sPoint.transform.rotation);
            //tempPrefab = ObjectPoolManager.SpawnObject(beginningSpawnList[curWave-1],esPoint[spwnIndex].sPoint.transform.position, esPoint[spwnIndex].sPoint.transform.rotation, ObjectPoolManager.PoolType.Enemies);
            tempPrefab.transform.parent = enemyParentObj.transform;
        }
        else if(curWave >= 11)
        {
            int randEnemy = Random.Range(0, midAndLateGameSpwnList.Count);
            tempPrefab = Instantiate(midAndLateGameSpwnList[randEnemy],esPoint[spwnIndex].sPoint.transform.position, esPoint[spwnIndex].sPoint.transform.rotation);
            //tempPrefab = ObjectPoolManager.SpawnObject(midAndLateGameSpwnList[randEnemy],esPoint[spwnIndex].sPoint.transform.position, esPoint[spwnIndex].sPoint.transform.rotation, ObjectPoolManager.PoolType.Enemies);
            tempPrefab.transform.parent = enemyParentObj.transform;
        }
        else if(exfilPhase)
        {
            int randEnemy = Random.Range(0, enemyPrefabs.Count);
            tempPrefab = Instantiate(enemyPrefabs[randEnemy],esPoint[spwnIndex].sPoint.transform.position, esPoint[spwnIndex].sPoint.transform.rotation);
            //tempPrefab = ObjectPoolManager.SpawnObject(enemyPrefabs[randEnemy],esPoint[spwnIndex].sPoint.transform.position, esPoint[spwnIndex].sPoint.transform.rotation, ObjectPoolManager.PoolType.Enemies);
            tempPrefab.transform.parent = enemyParentObj.transform;
        }

        if(curWave > 8 && (curWave % 3) == 0)
        {
            if(spawnLargeTrigger == false)
            {
                Debug.Log("Spawn Large enemy!");
                spawnLargeTrigger = true;
                tempPrefab.GetComponent<EnemyController>().CreateLargeEnemy(scaleLampLevel[zoneMultiplier-1],str,def,curWave,zoneMultiplier);
            }
        }
        else
        {
            if(exfilPhase)
            {
                //increase stats by exfil amount
                tempPrefab.GetComponent<EnemyController>().IncreaseStats(scaleLampLevel[zoneMultiplier-1],str,def,mainCountdown,zoneMultiplier);
            }
            else
            {
                //upgrade enemy stats here
                tempPrefab.GetComponent<EnemyController>().IncreaseStats(scaleLampLevel[zoneMultiplier-1],str,def,curWave,zoneMultiplier);
            }
        }
    }

    public void ChooseValidSpawnLocation()
    {
        bool isSpawnPosValid;
        int maxAttemptsPerLocation = 50;
        int maxTotalAttempts = 200;
        int totalAttempts = 0;
        randomIndex = 0;
        List<int> triedLocations = new();

        isSpawnPosValid = false;
        Collider[] colliders;

        //for i = 0; i < 4; i++
        //

        //find suitable spawn location
        while (!isSpawnPosValid && totalAttempts < maxTotalAttempts)
        {
            bool isInvalidCollision = false;
            randomIndex = Random.Range(0, esPoint.Count);

            if (!triedLocations.Contains(randomIndex))
            {
                triedLocations.Add(randomIndex);

                int attemptsOnCurrentLocation = 0;
                while (!isInvalidCollision && attemptsOnCurrentLocation < maxAttemptsPerLocation)
                {
                    isInvalidCollision = false;
                    UpdateSpawnPoints(esPoint[randomIndex]);

                    colliders = Physics.OverlapSphere(esPoint[randomIndex].sPoint.transform.position, 2f);

                    foreach (Collider col in colliders)
                    {
                        if (((1 << col.gameObject.layer) & _layersToNotSpawnOn) != 0)
                        {
                            isInvalidCollision = true;
                            break;
                        }
                    }

                    if (isInvalidCollision)
                    {
                        attemptsOnCurrentLocation++;
                        totalAttempts++;
                    }
                    else
                    {
                        isSpawnPosValid = true;
                        break;
                    }
                }
            }
            else
            {
                totalAttempts++;
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