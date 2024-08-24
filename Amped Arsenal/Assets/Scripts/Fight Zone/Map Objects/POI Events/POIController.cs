using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class POIController : MonoBehaviour
{
    public static POIController Instance { get; private set; }

    public List<GameObject> poiLocations = new();

    public List<GameObject> eventPrefabs = new();

    public List<GameObject> spawnedEvents = new();
    public GameObject spawnController, tempEvent;
    public LayerMask _layersToNotSpawnOn;
    public GameObject enemyParentObject;
    public int distToSpawnFromObjects, distToSpawnFromPOIs;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void InitPOIPositions(GameObject spawnArea)
    {
        spawnController = spawnArea;
        bool isSpawnPosValid;
        int attemptCount;
        int maxAttempts = 200;

        for(int i = 0; i < poiLocations.Count; i++)
        {
            //init vars per spawnpoint location 
            isSpawnPosValid = false;
            attemptCount = 0;
            while(!isSpawnPosValid && attemptCount < maxAttempts)
            {
                bool isInvalidCollision = false;

                UpdateSpawnPoints();
                Collider[] colliders = Physics.OverlapSphere(spawnController.GetComponent<EnemySpawnPoint>().sPoint.transform.position, distToSpawnFromObjects);

                foreach(Collider col in colliders)
                {
                    if(((1 << col.gameObject.layer) & _layersToNotSpawnOn) !=0)
                    {
                        //this means it collided with something
                        isInvalidCollision = true;
                        //end it and start the while loop over to find a new location
                        break;
                    }
                }

                //if we found a suitable spot then...
                if(!isInvalidCollision)
                {   
                    bool isInvalidPosFromPOIs= false;
                    //check distance to any other spawned POIs, if distance is good then set the isSpawnPosValid = true
                    if(poiLocations.Count > 0)
                    {   
                        foreach(GameObject poi in poiLocations)
                        {
                            float dist = Vector3.Distance(poi.transform.position, spawnController.GetComponent<EnemySpawnPoint>().sPoint.transform.position);
                            if(dist < distToSpawnFromPOIs)
                            {
                                isInvalidPosFromPOIs = true;
                            }
                        }

                        if(isInvalidPosFromPOIs == false)
                        {
                            isSpawnPosValid = true;
                        }
                    }
                    else
                    {
                        isSpawnPosValid = true;
                    }                    
                }

                attemptCount++;
            }

            if(!isSpawnPosValid)
            {
                Debug.Log("couldnt find valid spawn position for - event");
            }
            else
            {
                //set spawn location
                poiLocations[i].transform.position = spawnController.GetComponent<EnemySpawnPoint>().sPoint.transform.position;

                bool isUnusedEvent = false;
                int attemptCount2 = 0;
                int maxAttempts2 = 200;
                
                if(spawnedEvents.Count != 0)
                {
                    //try and get a random event and see if its being used already, if not random again until one is found
                    while(isUnusedEvent == false && attemptCount2 < maxAttempts2)
                    {
                        tempEvent = eventPrefabs[Random.Range(0,eventPrefabs.Count)];
                        int counter = 0;
                        for(int j = 0; j < spawnedEvents.Count; j++)
                        {
                            string goName = spawnedEvents[j].name[..^7]; //by taking off 7, we are removing the (Clone) from the name of the passed in obj
                            //Debug.Log(goName + " == " + tempEvent.name);
                            if(goName.Equals(tempEvent.name))
                            {
                                attemptCount2++;
                                break;
                            }
                            else
                            {
                                counter++;
                            }
                        }

                        if(counter == spawnedEvents.Count)
                        {
                            isUnusedEvent = true;
                        }
                    }

                    if(attemptCount2 >= maxAttempts2)
                    {
                        tempEvent = null;
                    }
                }
                else
                {
                    tempEvent = eventPrefabs[Random.Range(0,eventPrefabs.Count)];
                }
                
                //spawn event if not null
                if(tempEvent != null)
                {
                    GameObject temp = Instantiate(tempEvent, new Vector3(poiLocations[i].transform.position.x, 0, poiLocations[i].transform.position.z), poiLocations[i].transform.rotation);
                    temp.transform.parent = gameObject.transform;
                    spawnedEvents.Add(temp);
                }
            }
        }

        //SpawnEvents();

    }

    public void UpdateSpawnPoints()
    {
        Vector3 range = spawnController.transform.localScale / 2.0f;
        float x = Random.Range(-range.y, range.y); //z because the value needs to be between -.5 and .5 (which y and z are)
        float z = Random.Range(-range.y, range.y); 
        spawnController.GetComponent<EnemySpawnPoint>().sPoint.transform.localPosition = new Vector3(x, 0, z);
    }

    public void CleanUpEvents()
    {
        foreach(GameObject go in spawnedEvents)
        {
            go.GetComponent<DeliveryEventLogic>()?.CleanUp();

            Destroy(go);
        }
        spawnedEvents.Clear();
    }
}
