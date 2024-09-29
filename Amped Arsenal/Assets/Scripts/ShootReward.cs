using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootReward : MonoBehaviour
{

    private float shooterX, shooterY, shooterZ; 
    public bool wantRandom;

    [Header("RandY chooses a random angle")]
    public float randY;

    [Header("randPower is the result of randomly choosing between min and max")]
    public float randPower;
    
    public float powerMax, powerMin;
    public enum ShootType
    {
        Facing,
        Up
    };
    public ShootType sType;

    public GameObject DecideReward(GameObject pointToShootFrom)
    {
        GameObject relicToSpawn = null;
        // if player has all 6 weapons and all 6 weapons have mods already then spawn from relic list
        int weapsWithMods = 0;
        foreach(GameObject weap in GameZoneController.Instance.p1.equippedWeapons)
        {
            if(weap.GetComponent<WeaponBase>().effectSlots.Count >= 1)
            {
                weapsWithMods++;
            }
        }

        //if all weapons have mods on them, then only spawn relics
        if(weapsWithMods == 6)
        {
            //pull from relic list and not mod list
            relicToSpawn = GameZoneController.Instance.relicLibrary.relicList[Random.Range(0,GameZoneController.Instance.relicLibrary.weapModList.Count)];
            GameObject tempRelicSpawned = Instantiate(relicToSpawn, pointToShootFrom.transform.position, Quaternion.identity);
            tempRelicSpawned.transform.parent = GameObject.FindGameObjectWithTag("RelicHolder").transform;
            return tempRelicSpawned;
        }
        else
        {
            // 50/50 chance to pull fom both lists
            //int randomRelicWeapModList = Random.Range(1,3);
            //Debug.Log(randomRelicWeapModList);
            //if(randomRelicWeapModList == 1)
            //{
                //relicToSpawn = GameZoneController.Instance.relicLibrary.relicList[Random.Range(0,GameZoneController.Instance.relicLibrary.relicList.Count)];
            //}
            //else
            //{
                relicToSpawn = GameZoneController.Instance.relicLibrary.ReturnAWeapModNotChosenYet();
                GameObject tempRelicSpawned = Instantiate(relicToSpawn, pointToShootFrom.transform.position, Quaternion.identity);
                GameZoneController.Instance.relicLibrary.weapModList.Remove(relicToSpawn);
                tempRelicSpawned.transform.parent = GameObject.FindGameObjectWithTag("RelicHolder").transform;
                return tempRelicSpawned;
            //}
        }
                
        
        
    }

    public void GiveRewardAndYeetIt(GameObject pointToShootFrom)
    {
        //shoot it
        ShootObject(pointToShootFrom, DecideReward(pointToShootFrom), ShootType.Facing);
    }

    public void ShootObject(GameObject shooter, GameObject objToShoot, ShootType shootingType)
    {
        shooterX = shooter.transform.eulerAngles.x;
        shooterY = shooter.transform.eulerAngles.y;
        shooterZ = shooter.transform.eulerAngles.z;

        randY = Random.Range(0,360);
        randPower = Random.Range(powerMin, powerMax); //15 and 20
        shooter.transform.eulerAngles = new Vector3(shooterX, randY, shooterZ);


        if(wantRandom)
        {
            //randY = Random.Range(0,360);
            if(shootingType == ShootType.Facing)
            {
                //shootAngle += shooter.transform.forward;
                objToShoot.GetComponent<Rigidbody>().AddForce(shooter.transform.forward * randPower, ForceMode.Impulse);
            }
            else if(shootingType == ShootType.Up)
            {
                //shootAngle += shooter.transform.up;
                objToShoot.GetComponent<Rigidbody>().AddForce(shooter.transform.up * randPower, ForceMode.Impulse);
            }
        }
        else
        {
            if(shootingType == ShootType.Facing && !wantRandom)
            {
                //shootAngle += shooter.transform.forward;
                objToShoot.GetComponent<Rigidbody>().AddForce(shooter.transform.forward * randPower, ForceMode.Impulse);
            }
            else if(shootingType == ShootType.Up && !wantRandom)
            {
                //shootAngle += shooter.transform.up;
                objToShoot.GetComponent<Rigidbody>().AddForce(shooter.transform.up * randPower, ForceMode.Impulse);
            }
        }
    }

    public void ShootObjectPlusOffset(GameObject shooter, Vector3 offset, GameObject objToShoot, ShootType shootingType)
    {
        shooterX = shooter.transform.eulerAngles.x;
        shooterY = shooter.transform.eulerAngles.y;
        shooterZ = shooter.transform.eulerAngles.z;

        randY = Random.Range(0,360);
        randPower = Random.Range(powerMin, powerMax); //15 and 20
        shooter.transform.eulerAngles = new Vector3(shooterX, randY, shooterZ);


        if(wantRandom)
        {
            //randY = Random.Range(0,360);
            if(shootingType == ShootType.Facing)
            {
                //shootAngle += shooter.transform.forward;
                objToShoot.GetComponent<Rigidbody>().AddForce(shooter.transform.forward * randPower, ForceMode.Impulse);
            }
            else if(shootingType == ShootType.Up)
            {
                //shootAngle += shooter.transform.up;
                objToShoot.GetComponent<Rigidbody>().AddForce(shooter.transform.up + offset* randPower, ForceMode.Impulse);
            }
        }
        else
        {
            if(shootingType == ShootType.Facing && !wantRandom)
            {
                //shootAngle += shooter.transform.forward;
                objToShoot.GetComponent<Rigidbody>().AddForce(shooter.transform.forward * randPower, ForceMode.Impulse);
            }
            else if(shootingType == ShootType.Up && !wantRandom)
            {
                //shootAngle += shooter.transform.up;
                objToShoot.GetComponent<Rigidbody>().AddForce(shooter.transform.up * randPower, ForceMode.Impulse);
            }
        }
    }
}
