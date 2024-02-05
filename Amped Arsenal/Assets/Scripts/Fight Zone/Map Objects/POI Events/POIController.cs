using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POIController : MonoBehaviour
{
    public static POIController Instance { get; private set; }

    public List<GameObject> poiLocations = new List<GameObject>();

    public List<GameObject> eventPrefabs = new List<GameObject>();

    public List<GameObject> spawnedEvents = new List<GameObject>();

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

    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        foreach (GameObject go in eventPrefabs)
        {
            //if (go.tag.Equals("ALamp"))
            //{
                GameObject temp;
                temp = Instantiate(go, new Vector3(poiLocations[i].transform.position.x, 0, poiLocations[i].transform.position.z), poiLocations[i].transform.rotation);
                spawnedEvents.Add(temp);
                i++;
            //}
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
