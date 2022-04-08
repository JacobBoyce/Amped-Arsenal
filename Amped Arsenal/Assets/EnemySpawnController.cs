using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
        //POSSIBLE OBJECT POOLING FOR ENEMIES

    public GameObject enemyPrefab, tempePrefab;
    public List<EnemySpawnPoint> esPoint = new List<EnemySpawnPoint>();
    //private Quaternion rotation;

    public float spawnRate, spawnRateMax;
    // Start is called before the first frame update
    void Awake()
    {
        //rotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
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
        //transform.rotation = rotation;
    }

    public void SpawnEnemy()
    {
        //Front spawning logic - most often
        tempePrefab = Instantiate(enemyPrefab,esPoint[0].sPoint.transform.position, esPoint[0].sPoint.transform.rotation);

        //Back spawning logic - least often
        

        //Left spawning logic - less often


        //Right spawning logic - less often
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
