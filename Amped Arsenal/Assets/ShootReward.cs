using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootReward : MonoBehaviour
{
    public float shooterX, shooterZ, randY, randPower;
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
        shooterZ = shooter.transform.eulerAngles.z;

        randY = Random.Range(0,360);
        randPower = Random.Range(powerMin, powerMax); //15 and 20

        if(shootingType == ShootType.Facing)
        {
            shooter.transform.eulerAngles = new Vector3(shooterX, randY, shooterZ);
            objToShoot.GetComponent<Rigidbody>().AddForce(shooter.transform.forward * randPower, ForceMode.Impulse);
        }
        else if(shootingType == ShootType.Up)
        {
            shooter.transform.eulerAngles = new Vector3(shooterX, randY, shooterZ);
            objToShoot.GetComponent<Rigidbody>().AddForce(shooter.transform.up * randPower, ForceMode.Impulse);
        }
    }
}
