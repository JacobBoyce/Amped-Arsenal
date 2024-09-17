using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Den.Tools;
using Unity.VisualScripting;

public class WaveController : MonoBehaviour
{
    public EnemySpawnController spawnController;
    public GlobalVolumeController gvController;
    public GameObject waveUIObj;
    public TextMeshProUGUI infoMessages;
    public Image countdownBarUI;
    public int curWave = 0, maxWave;
    public float startWaitTime;
    public bool wavesActive = false, exfilPhase = false, enemyVisuals = false;
    public ExfilLampLogic exfilLampObject;
    public GameObject enemyParentObj;

    [Header("Moon Stuff")]
    public float endAngle = 237;
    public float totalWaveTime;
    public RectTransform moons;
    [SerializeField]
    public List<EnemyListPerLevel> enemyPrefabs = new(); 
    public WaveMaster waveMaster;

    [Header("Spawn Stuff")]
    public float[] waveSpawnTimers;
    public List<EnemySpawnPoint> esPoint = new List<EnemySpawnPoint>();
    private GameObject tempPrefab;
    public bool spawnLargeTrigger = true;
    private int everyOtherWaveChangeBoss = 0;

    [Header("Stat Scaling")]
    public float strScaling, defScaling;
    public int zoneMultiplier;
    //public float[] scaleLampLevel;
    public float waveScale, levelScale, exfilScale, exfilScaleInterval;

    //Time stuff
    [Header("Timers")]
    public int maxWaveTimer;
    public float mainCountdown, exfilTimer;
    public float spawnRate, spawnRateMax, seconds;
    public int min;   
    public bool toggleSpawning = false;

    public float delayStartMax, delayStart, delayExfilMax, delayExfil;
    public bool startDelay, startExfilDelay;
    private int randomIndex;
    public int largeEnemyIndex;
    public float whenToSpawnLargeEnemy;
    public LayerMask _layersToNotSpawnOn;


    // Start is called before the first frame update
    void Start()
    {
        //waveText.text = "Wave " + curWave + "/" + maxWave;
        totalWaveTime = (maxWave-1)*maxWaveTimer;
        mainCountdown = maxWaveTimer;

        delayStart = delayStartMax;
        delayExfil = delayExfilMax;      
        whenToSpawnLargeEnemy = Mathf.RoundToInt((totalWaveTime / 5) - (maxWaveTimer / 2));
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
                
               //start exfill phase
               startExfilDelay = true;
               toggleSpawning = false;
               gvController.StartRedFade();
            }
        }
        #endregion

        #region Exfil timer
        if(exfilPhase)
        {
            mainCountdown += Time.deltaTime;
            exfilTimer += Time.deltaTime;
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
                SpawnEnemy(ChooseValidSpawnLocation());
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
                //spawnRateMax = 2;
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
                spawnRateMax = .2f;
                
            }
        }
        #endregion
    }

    public void SpawnLargeEnemy(bool randomBoss)
    {
        if(!randomBoss)
        {
            if(everyOtherWaveChangeBoss == 0 || everyOtherWaveChangeBoss == 1)
            {
                everyOtherWaveChangeBoss++;
            }
            else
            {
                everyOtherWaveChangeBoss = 1;
                largeEnemyIndex++;
            }
            int spwnLoc = ChooseValidSpawnLocation();
            GameObject largeEnemyPrefab = Instantiate(enemyPrefabs[zoneMultiplier-1].ePrefabs[largeEnemyIndex == 5 ? largeEnemyIndex-1 : largeEnemyIndex],esPoint[spwnLoc].sPoint.transform.position, esPoint[spwnLoc].sPoint.transform.rotation);
            largeEnemyPrefab.GetComponent<EnemyController>().CreateLargeEnemy(waveScale,levelScale, exfilScale, exfilScaleInterval,strScaling,defScaling,curWave,zoneMultiplier);
            largeEnemyPrefab.transform.parent = enemyParentObj.transform;
            largeEnemyPrefab.GetComponent<EnemyController>().ToggleViewHP(enemyVisuals);

            if(largeEnemyIndex == 5)
            {
                largeEnemyIndex = 0;
            }
        }
        else
        {
            int randIndex = Random.Range(0, enemyPrefabs[zoneMultiplier-1].ePrefabs.Count);
            
            int spwnLoc = ChooseValidSpawnLocation();
            tempPrefab = Instantiate(enemyPrefabs[zoneMultiplier-1].ePrefabs[randIndex],esPoint[spwnLoc].sPoint.transform.position, esPoint[spwnLoc].sPoint.transform.rotation);
            tempPrefab.GetComponent<EnemyController>().CreateLargeEnemy(waveScale,levelScale, exfilScale, exfilScaleInterval,strScaling,defScaling,curWave,zoneMultiplier);
            tempPrefab.transform.parent = enemyParentObj.transform;
            tempPrefab.GetComponent<EnemyController>().ToggleViewHP(enemyVisuals);

        }
    }
    public void StartNextWave()
    {
        if(curWave > 1)
        {
            SpawnLargeEnemy(false);
        }
        else if(curWave > 10)
        {
            SpawnLargeEnemy(true);
        }
        //update wave UI
        //waveText.text = "Wave " + curWave + "/" + maxWave;
        // if(spawnRateMax > 1f)
        // {
            spawnRateMax = waveSpawnTimers[curWave-1];
        //}
        
        //spawn enemies
        toggleSpawning = true; 
    }

    
    public void StartWaveSystem()
    {
        startDelay = true;
        //waveText.gameObject.SetActive(false);
        infoMessages.gameObject.SetActive(true);
        delayStart = delayStartMax;
        delayExfil = delayExfilMax;
        //spawnRateMax = 1f;

        moons.localRotation = Quaternion.Euler(0,0,0);
        totalWaveTime = (maxWave-1)*maxWaveTimer;
        mainCountdown = maxWaveTimer;
        spawnRateMax = waveSpawnTimers[0];
        largeEnemyIndex = 0;
        everyOtherWaveChangeBoss = 0;
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
        if(spwnIndex != -1)
        {
            if(curWave <= 15)
            {
                int randEnemy = Random.Range(0, waveMaster.levelWaveList[zoneMultiplier-1].waves[curWave-1].enemysToSpawn.Count);
                // Debug.Log("level: " + waveMaster.levelWaveList[zoneMultiplier-1].levelName
                //     + "\nWave Index: " + (curWave-1)
                //     + "\nEnemy To Spawn: " + waveMaster.levelWaveList[zoneMultiplier-1].waves[curWave-1].enemysToSpawn[randEnemy] + " randEnemy: " + randEnemy
                //     + "\nSpawnIndex: " + spwnIndex);
                tempPrefab = Instantiate(waveMaster.levelWaveList[zoneMultiplier-1].waves[curWave-1].enemysToSpawn[randEnemy],esPoint[spwnIndex].sPoint.transform.position, esPoint[spwnIndex].sPoint.transform.rotation);
                //tempPrefab = ObjectPoolManager.SpawnObject(beginningSpawnList[curWave-1],esPoint[spwnIndex].sPoint.transform.position, esPoint[spwnIndex].sPoint.transform.rotation, ObjectPoolManager.PoolType.Enemies);
                tempPrefab.transform.parent = enemyParentObj.transform;
            }
            else if(exfilPhase)
            {
                int randEnemy = Random.Range(0, enemyPrefabs[zoneMultiplier-1].ePrefabs.Count);
                tempPrefab = Instantiate(enemyPrefabs[zoneMultiplier-1].ePrefabs[randEnemy],esPoint[spwnIndex].sPoint.transform.position, esPoint[spwnIndex].sPoint.transform.rotation);
                //tempPrefab = ObjectPoolManager.SpawnObject(enemyPrefabs[randEnemy],esPoint[spwnIndex].sPoint.transform.position, esPoint[spwnIndex].sPoint.transform.rotation, ObjectPoolManager.PoolType.Enemies);
                tempPrefab.transform.parent = enemyParentObj.transform;
            }
            

            if(exfilPhase)
            {
                //increase stats by exfil amount
                tempPrefab.GetComponent<EnemyController>().IncreaseStats(waveScale, levelScale, exfilScaleInterval, strScaling,defScaling,exfilTimer,curWave,zoneMultiplier);
            }
            else
            {
                //upgrade enemy stats here
                tempPrefab.GetComponent<EnemyController>().IncreaseStats(waveScale, levelScale, strScaling,defScaling,curWave,zoneMultiplier);
            }
            tempPrefab.GetComponent<EnemyController>().ToggleViewHP(enemyVisuals);
        }
    }

    public int ChooseValidSpawnLocation()
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
            return -1;
        }
        else
        {
            //spawnstuff
            return randomIndex;
            //SpawnEnemy(randomIndex);
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

[System.Serializable]
public struct EnemyListPerLevel
{
    [SerializeField]
    public string levelName;
    [SerializeField]
    public List<GameObject> ePrefabs;

    public EnemyListPerLevel(string lvlName, List<GameObject> ep)
    {
        levelName = lvlName;
        ePrefabs = ep;
    }

}

[System.Serializable]
public struct Wave
{
    [SerializeField]
    public List<GameObject> enemysToSpawn;
    public Wave(List<GameObject> ep)
    {
        enemysToSpawn = ep;
    }
}

[System.Serializable]
public struct WaveList 
{
    [SerializeField]
    public string levelName;
    [SerializeField]
    public List<Wave> waves;
    public WaveList(string lvlName, List<Wave> ep)
    {
        levelName = lvlName;
        waves = ep;
    }
}

[System.Serializable]
public struct WaveMaster 
{
    [SerializeField]
    public List<WaveList> levelWaveList;
    public WaveMaster(List<WaveList> ep)
    {
        levelWaveList = ep;
    }
}