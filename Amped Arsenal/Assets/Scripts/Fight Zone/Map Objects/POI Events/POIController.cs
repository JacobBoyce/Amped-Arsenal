using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POIController : MonoBehaviour
{
    public static POIController Instance { get; private set; }

    public List<GameObject> pois = new List<GameObject>();

    public List<GameObject> lampPrefabs = new List<GameObject>();

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
        foreach (GameObject go in lampPrefabs)
        {
            //if (go.tag.Equals("ALamp"))
            //{
                GameObject temp;
                temp = Instantiate(go, new Vector3(pois[i].transform.position.x, 0, pois[i].transform.position.z), pois[i].transform.rotation);
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
