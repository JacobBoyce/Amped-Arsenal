using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSwingController : WeaponBase
{
    [Space(10)]
    public GameObject weapPrefabDown;
    //one prefab all references set, when spawned move to proper position deacitvate and activate on cooldown stuff
    public int curSwingNum, maxSwingNum;
    public bool swangUp, swangDown;

    public List<GameObject> swordObjs = new List<GameObject>();
    public List<SwordSwingLogic> swordlogics = new List<SwordSwingLogic>();

    void Start()
    {
        TickSystem.OnSubTick += delegate (object sender, TickSystem.OnTickEventArgs e) 
        {
            curCooldown++;
        };
        SetSpawnDetails();
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
    }
    
    public override void ActivateAbility()
    {
        curCooldown = 0;
        curSwingNum = maxSwingNum;
        swordObjs[0].SetActive(true);
        swordlogics[0].TurnOn();
    }

    public override void SetSpawnDetails()
    {
        GameObject tempWeapSpawnUp = Instantiate(weapPrefab);

        GameObject tempWeapSpawnDown = Instantiate(weapPrefabDown);

        //Swing Up Object
        tempWeapSpawnUp.gameObject.transform.SetParent(playerObj.spawnPoints[spawnDetails[0].spawnpoint].transform);
        tempWeapSpawnUp.gameObject.transform.localPosition = Vector3.zero;

        tempWeapSpawnUp.GetComponent<SwordSwingLogic>().InitSword(this, weapMod, tempWeapSpawnUp, tempWeapSpawnDown);

        swordObjs.Add(tempWeapSpawnUp);
        swordlogics.Add(tempWeapSpawnUp.GetComponent<SwordSwingLogic>());
        tempWeapSpawnUp.SetActive(false);

        //Swing Down object
        tempWeapSpawnDown.gameObject.transform.SetParent(playerObj.spawnPoints[spawnDetails[1].spawnpoint].transform);
        tempWeapSpawnDown.gameObject.transform.localPosition = Vector3.zero;
        
        tempWeapSpawnDown.GetComponent<SwordSwingLogic>().InitSword(this, weapMod, tempWeapSpawnDown, tempWeapSpawnUp);
        
        swordObjs.Add(tempWeapSpawnDown);
        swordlogics.Add(tempWeapSpawnDown.GetComponent<SwordSwingLogic>());
        tempWeapSpawnDown.SetActive(false);
        
        UpdateValues();
    }

    public override void UpgradeWeapon()
    {
        level++;
        UpdateValues();
    }

    public void UpdateValues()
    {
        maxSwingNum = (int)weapUpgrades.UpgradeList.Find(x => x.weapUpType == WeapUpgrade.WeaponUpgrade.AMOUNT).upValues[level - 1];
        tickMaxCD = (int)weapUpgrades.UpgradeList.Find(x => x.weapUpType == WeapUpgrade.WeaponUpgrade.COOLDOWN).upValues[level - 1];
        damage = (int)weapUpgrades.UpgradeList.Find(x => x.weapUpType == WeapUpgrade.WeaponUpgrade.DAMAGE).upValues[level - 1];
    }
}
