using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootReward : MonoBehaviour
{
    public float shooterX, shooterZ, randY, randPower;


    public void ShootObject(GameObject shooter, GameObject objToShoot)
    {
        shooterX = shooter.transform.eulerAngles.x;
        shooterZ = shooter.transform.eulerAngles.z;

        randY = Random.Range(0,360);
        randPower = Random.Range(15f, 20f);

        shooter.transform.eulerAngles = new Vector3(shooterX, randY, shooterZ);
        objToShoot.GetComponent<Rigidbody>().AddForce(shooter.transform.forward * randPower, ForceMode.Impulse);
    }
}
