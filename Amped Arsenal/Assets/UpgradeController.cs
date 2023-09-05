using System.Collections;
using System.Collections.Generic;
using Den.Tools;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    public MainMenuController mainController;
    public GameObject upgradeParentObj, upgradeSquarePrefab;
    private MainMenuController.BaseUpgrade tempUpgrade = new MainMenuController.BaseUpgrade();
    [SerializeField]
    public List<MainMenuController.BaseUpgrade> upgradeList;
    public List<GameObject> upgradePrefabs = new();
    
    [SerializeField]
    public List<UpgradeValues> upVals;
    

    [Space(10)]

    public int[] upgradeCostValues = new int[5];

    public void Awake()
    {
        upgradeList ??= new List<MainMenuController.BaseUpgrade>();
        // the code below equals this ^
        /*if(upgradeList == null)
        {
            upgradeList = new List<MainMenuController.BaseUpgrade>();
        }*/
        
        //InitUpgradeList();
    }

    public void InitUpgradeList()
    {
        //load values to tempUpgrade
        tempUpgrade.upgradeName = "Strength";
        tempUpgrade.upgradeLevel = 0;
        tempUpgrade.upValues = new int[5];


        //save it to player prefs so when you start a game it will load the value in the fight zone scene correctly
        //PlayerPrefs.SetInt(tempUpgrade.upgradeName,tempUpgrade.upgradeLevel);
        upgradeList.Add(tempUpgrade);

        //--------------------------------------------
        tempUpgrade.upgradeName = "HP";
        tempUpgrade.upgradeLevel = 0;
        tempUpgrade.upValues = new int[5];

        //PlayerPrefs.SetInt(tempUpgrade.upgradeName,tempUpgrade.upgradeLevel);
        upgradeList.Add(tempUpgrade);

        //--------------------------------------------
        tempUpgrade.upgradeName = "Armor";
        tempUpgrade.upgradeLevel = 0;
        tempUpgrade.upValues = new int[5];

        //PlayerPrefs.SetInt(tempUpgrade.upgradeName,tempUpgrade.upgradeLevel);
        upgradeList.Add(tempUpgrade);

        //--------------------------------------------
        tempUpgrade.upgradeName = "Speed";
        tempUpgrade.upgradeLevel = 0;
        tempUpgrade.upValues = new int[5];

        //PlayerPrefs.SetInt(tempUpgrade.upgradeName,tempUpgrade.upgradeLevel);
        upgradeList.Add(tempUpgrade);
        //--------------------------------------------
        tempUpgrade.upgradeName = "Magnet";
        tempUpgrade.upgradeLevel = 0;
        tempUpgrade.upValues = new int[5];

        //PlayerPrefs.SetInt(tempUpgrade.upgradeName,tempUpgrade.upgradeLevel);
        upgradeList.Add(tempUpgrade);
        //--------------------------------------------
        tempUpgrade.upgradeName = "Inflation";
        tempUpgrade.upgradeLevel = 0;
        tempUpgrade.upValues = new int[5];

        //PlayerPrefs.SetInt(tempUpgrade.upgradeName,tempUpgrade.upgradeLevel);
        upgradeList.Add(tempUpgrade);
    }

    public void LoadUpgradeList()
    {
        int i = 0;
        foreach(MainMenuController.BaseUpgrade bu in upgradeList)
        {
            //GameObject temp;
            //temp = Instantiate(upgradeSquarePrefab, upgradeParentObj.transform);
            //set name and level of the prefab
            if(bu.upgradeLevel != 5)
            {
                upgradePrefabs[i].GetComponent<BaseUpgradeSquare>().SetUpgradeVisuals(bu.upgradeName, bu.upgradeLevel, upgradeCostValues[bu.upgradeLevel]);
            }
            else
            {
                upgradePrefabs[i].GetComponent<BaseUpgradeSquare>().SetUpgradeVisuals(bu.upgradeName, bu.upgradeLevel, -1);
            }
            
            i++;
        }
    }

    public void ChooseUpgrade(string upName)
    {
        MainMenuController.BaseUpgrade tempUpgrade = new();
        tempUpgrade.upgradeName = "";
        tempUpgrade.upgradeLevel = 0;

        //Get the upgrade from the upgrade list to update the data to be saved
        foreach(MainMenuController.BaseUpgrade bu in upgradeList)
        {
            if (bu.upgradeName == upName)
            {
                tempUpgrade = bu;
            }
        }

        //buy upgrade stuff
        if(mainController.PlayerGold < upgradeCostValues[tempUpgrade.upgradeLevel])
        {
            //cant buy
            Debug.Log("Cant buy");
        }
        else
        {
            Debug.Log(upgradeCostValues[tempUpgrade.upgradeLevel]);
            //subtract money
            mainController.PlayerGold -= upgradeCostValues[tempUpgrade.upgradeLevel];
        }

        if(tempUpgrade.upgradeLevel > 0)
        {
            //increment level
            tempUpgrade.upgradeLevel++;

            //Get the gameobject to update
            foreach(GameObject go in upgradePrefabs)
            {
                if(tempUpgrade.upgradeName == go.GetComponent<BaseUpgradeSquare>().baseUpgradeName)
                {
                    //update price on upgrade
                    BaseUpgradeSquare bus = go.GetComponent<BaseUpgradeSquare>();
                    bus.BaseUpgradeLevel++;
                    bus.BaseCost = upgradeCostValues[tempUpgrade.upgradeLevel];
                }
            }

            foreach(GameObject go in upgradePrefabs)
            {
                if(go.GetComponent<BaseUpgradeSquare>().baseUpgradeName.Equals(tempUpgrade.upgradeName))
                {
                    go.GetComponent<BaseUpgradeSquare>().UpdateBar(true);
                }
            }
            mainController.UpdatePlayerPrefs();
            mainController.SaveUpgradeValues();
            //save upgrade
        }
    }

    public void SaveStuffButton()
    {
        mainController.SaveUpgradeValues();
    }

    
}

[System.Serializable]
public class UpgradeValues
{
    [SerializeField]
    public string upName;
    [SerializeField]
    public int[] upValues;
}
