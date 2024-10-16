using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeChild : MonoBehaviour
{
    public GameObject axePrefab, curAxe, visuals;
    public float distToPoint, distFromParent;
    private float speedOut;
    public bool moveOut = false;

    public Vector3 origPos;
    public void Awake()
    {
        origPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(moveOut)
        {
            if(distToPoint <= distFromParent)
            {
                distToPoint += speedOut * Time.deltaTime;
            }
            transform.localPosition = new Vector3(origPos.x * distToPoint, 0, origPos.z * distToPoint);
        }
    }

    public void InitStuff(AxeController cont, WeaponMods mod,float speed,float distFP)
    {
        moveOut = false;
        UpdateValues(speed, distFP);
        curAxe = Instantiate(axePrefab, transform.position, transform.rotation);
        curAxe.GetComponent<AxeLogic>().UpdateInfo(cont, mod);
        visuals = curAxe.GetComponent<AxeLogic>().visuals;
        curAxe.GetComponent<FollowObject>().SetValues(gameObject, 100f);
        ToggleAxe(false);
        //curAxe.SetActive(false);
    }

    public void UpdateValues(float speed,float distFP)
    {
        distFromParent = distFP;
        speedOut = speed;
    }

    public void TurnOnSpawnAxe()
    {
        ToggleAxe(true);
        moveOut = true;
    }

    public void TurnOffAxe()
    {
        ToggleAxe(false);
        distToPoint = 0;
        transform.localPosition = origPos;
        curAxe.transform.position = transform.position;
        moveOut = false;
    }

    public void ToggleAxe(bool t)
    {
        //turn off visuals
        visuals.SetActive(t);
        //curAxe.GetComponentInChildren<MeshRenderer>().enabled = t;
        curAxe.GetComponentInChildren<BoxCollider>().enabled = t;
    }
}
