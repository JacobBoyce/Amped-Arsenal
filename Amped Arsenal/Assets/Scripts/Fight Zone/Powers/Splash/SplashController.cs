using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashController : WeaponBase
{
    public int splashSplitAmount;
    public float offsetY;
    public float shootPower, buffer;
    public GameObject initialSpawnSplash;

    void Start()
    {
        TickSystem.OnSubTick += delegate (object sender, TickSystem.OnTickEventArgs e) 
        {
            curCooldown++;
        };

        UpdateValues(); 
        //SetSpawnDetails();
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

        UpdateValues();
    }
    
    public override void ActivateAbility()
    {
        //spawn object and call its init method 
        curCooldown = 0;

        GameObject tempSplash = Instantiate(weapPrefab, playerObj.rotatingSpawnPoints[spawnDetails[0].spawnpoint].transform.position + (playerObj.rotatingSpawnPoints[spawnDetails[0].spawnpoint].transform.forward *5f), playerObj.spawnPoints[1].transform.rotation);
        tempSplash.GetComponentInChildren<SplashLogic>().InitSplash(this, weapMod);

        
        Instantiate(initialSpawnSplash, tempSplash.transform.position, tempSplash.transform.rotation);
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
        splashSplitAmount = (int)weapUpgrades.UpgradeList.Find(x => x.weapUpType == WeapUpgrade.WeaponUpgrade.AMOUNT).upValues[level - 1];        
    }
}
