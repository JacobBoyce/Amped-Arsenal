using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashLogic : MonoBehaviour
{
    [Header("Parent Vars")]
    public SplashController controller;
    public GameObject visualObjectToRotate;
    public WeaponMods myMods;
    public Rigidbody thisRB;

    [Header("Logic Vars")]
    public GameObject splashPrefab;
    public GameObject waterBallPrefab;
    public List<GameObject> shootFromObjects = new List<GameObject>();
    public Vector3 shootDir;
    public int randomIndex, babySpawnIndex;
    public float offsetY;
    public float shootPower, buffer;
    public bool isBabySplash = false;
    public List<int> spawnStuff = new List<int>();

    public void InitSplash(SplashController cont, WeaponMods mod)
    {
        controller = cont;
        myMods = mod;

        offsetY = cont.offsetY;
        shootPower = cont.shootPower;
        buffer = cont.buffer;

        waterBallPrefab = cont.weapPrefab;
        shootFromObjects = cont.playerObj.spawnPoints;

        
        for(int i = 0; i < controller.splashSplitAmount; i++)
        {
            bool chooseNotCenter = false;
            while(chooseNotCenter == false)
            {
                randomIndex = UnityEngine.Random.Range(0,shootFromObjects.Count);
                if(randomIndex != 4 && randomIndex != 0 && randomIndex != 1)
                {
                    if(!spawnStuff.Contains(randomIndex))
                    {
                        spawnStuff.Add(randomIndex);
                        chooseNotCenter = true;
                    }                            
                }
            }
        }
        

        if(isBabySplash)
        {
            ChooseBabyDir();
        }
        else
        {
            ChooseShootDir();
        }
        
    }

    private void ChooseShootDir()
    {
        GameObject shootObjectDir, shootCenterDir;
        bool chooseNotCenter = false;
        //choose a random object to create the shoot vector
        while(chooseNotCenter == false)
        {
            randomIndex = UnityEngine.Random.Range(0,shootFromObjects.Count);
            if(randomIndex != 4 && randomIndex != 0 && randomIndex != 1)
            {
                chooseNotCenter = true;
            }
            
        }
        shootCenterDir = shootFromObjects[4];
        shootObjectDir = shootFromObjects[randomIndex];

        shootDir = shootCenterDir.transform.position - shootObjectDir.transform.position;
        
        shootDir = new Vector3(shootDir.x, shootDir.y + offsetY, shootDir.z);
        //Debug.DrawLine(gameObject.transform.position, shootObjectDir.transform.position, Color.white, 2.5f);

        thisRB.AddForce(shootDir * shootPower, ForceMode.Impulse);
    }

    public void ChooseBabyDir()
    {
        GameObject shootObjectDir, shootCenterDir;

        shootCenterDir = shootFromObjects[4];
        shootObjectDir = shootFromObjects[babySpawnIndex];

        shootDir = shootCenterDir.transform.position - shootObjectDir.transform.position;
        
        shootDir = new Vector3(shootDir.x, shootDir.y + offsetY, shootDir.z);
        //Debug.DrawLine(gameObject.transform.position, shootObjectDir.transform.position, Color.white, 2.5f);

        thisRB.AddForce(shootDir * shootPower, ForceMode.Impulse);
    }

    public void Update()
    {
        float x,y;
        x = visualObjectToRotate.transform.localRotation.x;
        y = visualObjectToRotate.transform.localRotation.y;

        // moving right
        if(thisRB.velocity.x > 0)
        {
            this.GetComponentInChildren<SpriteRenderer>().flipY = true;
            visualObjectToRotate.transform.localRotation = Quaternion.Euler(x,y,(thisRB.velocity.y + 180)*buffer);
        }
        else if(thisRB.velocity.x < 0)
        {
            visualObjectToRotate.transform.localRotation = Quaternion.Euler(x,y,(-thisRB.velocity.y)*buffer);
        }
        else
        {
            //if not moving delete it
            //Destroy(this.gameObject);
        }

        if(Vector3.Distance(controller.playerObj.gameObject.transform.position, transform.position) > 100)
        {
            Destroy(this.gameObject);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "TerrainObj" || collision.gameObject.tag == "Enemy")
        {
            //spawn splash object
            if(isBabySplash)
            {
                GameObject babySplashCollision = Instantiate(splashPrefab, transform.position, transform.rotation);
                babySplashCollision.GetComponent<SplashCollisionLogic>().controller = controller;
                babySplashCollision.transform.localScale = new Vector3(4f,4f,4f);
            }
            else
            {
                GameObject splashCollision = Instantiate(splashPrefab, transform.position, transform.rotation);
                splashCollision.GetComponent<SplashCollisionLogic>().controller = controller;
            }
            

            //spawn more splash balls
            //get splash split amount from parent. subtract amount on init?
            if(!isBabySplash)
            {
                for(int i = 0; i < controller.splashSplitAmount; i++)
                {
                    GameObject tempSplash = Instantiate(waterBallPrefab, transform.position, transform.rotation);
                    tempSplash.transform.localScale = new Vector3(.7f,.7f,.7f);
                    tempSplash.GetComponentInChildren<SplashLogic>().isBabySplash = true;

                    tempSplash.GetComponentInChildren<SplashLogic>().babySpawnIndex = spawnStuff[i];
                    

                    tempSplash.GetComponentInChildren<SplashLogic>().InitSplash(controller, myMods);
                }
            }
            

            Destroy(this.gameObject);
        }       
    }
}
