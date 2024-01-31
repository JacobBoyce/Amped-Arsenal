using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootReward : MonoBehaviour
{
    public Vector3 shootAngle, offsetVector;
    private float shooterX, shooterY, shooterZ; 
    public bool wantRandom, addXOffset;

    [Header("RandY chooses a random angle")]
    public float randY, offsetX;

    [Header("randPower is the result of randomly choosing between min and max")]
    public float randPower;
    
    public float powerMax, powerMin;
    public enum ShootType
    {
        Facing,
        Up
    };
    public ShootType sType;

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
}
