using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : WeaponBase
{
    public int range;
    public float timeToExplode;
    public GameObject spawnPointCenter, spawnPointBack;
    public float _offsetY;
    void Start()
    {
        TickSystem.OnSubTick += delegate (object sender, TickSystem.OnTickEventArgs e) 
        {
            curCooldown++;
        };
        //SetSpawnDetails();
        UpdateValues();
    }
    public void Update()
    {
        if(curCooldown >= tickMaxCD)
        {
            ActivateAbility();
        }        
    }

    public void OnEquipped()
    {
        TickSystem.OnSubTick += delegate (object sender, TickSystem.OnTickEventArgs e) 
        {
            curCooldown++;
        };
        //set where to shoot from, when equiping the object
        spawnPointCenter = playerObj.spawnPoints[spawnDetails[0].spawnpoint];
        spawnPointBack = playerObj.rotatingSpawnPoints[spawnDetails[1].spawnpoint];

        UpdateValues();
    }
    
    public override void ActivateAbility()
    {
        //spawn object and call its init method 
        curCooldown = 0;
        spawnPointCenter = playerObj.spawnPoints[spawnDetails[0].spawnpoint];
        spawnPointBack = playerObj.rotatingSpawnPoints[spawnDetails[1].spawnpoint];

        //Vector3 offsetY = new Vector3(objToShootFrom.transform.position.x, objToShootFrom.transform.position.y + _offsetY, objToShootFrom.transform.position.x);

        GameObject tempBomb = Instantiate(weapPrefab, spawnPointCenter.transform.position, spawnPointCenter.transform.rotation);
        tempBomb.GetComponentInChildren<BombLogic>().InitBomb(this, weapMod);
    }

    public override void PlayDamageSound()
    {
        damageSound.pitch = Random.Range(1 - pitchMultiplier, 1 + pitchMultiplier);
        damageSound.PlayOneShot(damageSound.clip);
    }


    public override void UpgradeWeapon()
    {
        level++;
        UpdateValues();
    }

    //This updates the values of the weapon each time it is activated. so after you upgrade the weapon
    //then it will give it the new stats right before the next time it activates
    public void UpdateValues()
    {
        tickMaxCD = (int)weapUpgrades.UpgradeList.Find(x => x.weapUpType == WeapUpgrade.WeaponUpgrade.COOLDOWN).upValues[level - 1];
        damage = (int)weapUpgrades.UpgradeList.Find(x => x.weapUpType == WeapUpgrade.WeaponUpgrade.DAMAGE).upValues[level - 1];
        range = (int)weapUpgrades.UpgradeList.Find(x => x.weapUpType == WeapUpgrade.WeaponUpgrade.RANGE).upValues[level - 1];        
    }
}
