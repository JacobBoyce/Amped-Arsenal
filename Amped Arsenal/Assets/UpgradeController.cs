using System.Collections;
using System.Collections.Generic;
using Den.Tools;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class UpgradeController : MonoBehaviour
{
    public MainMenuController mainController;
    public MenuItemSelectionManager selectionManager;
    public GameObject upgradeParentObj, upgradeSquarePrefab;
    public TextMeshProUGUI goldText;
    private MainMenuController.BaseUpgrade tempUpgrade = new MainMenuController.BaseUpgrade();
    [SerializeField]
    public List<MainMenuController.BaseUpgrade> upgradeList;
    public List<GameObject> upgradePrefabs = new();

    
    [SerializeField]
    public List<UpgradeValues> upVals;
    public int[] upgradeCostValues;

    public void Awake()
    {
        mainController = MainMenuController.Instance;
        mainController.upController = this;
        upgradeList ??= new List<MainMenuController.BaseUpgrade>();
        #region the code below equals this ^
        /*if(upgradeList == null)
        {
            upgradeList = new List<MainMenuController.BaseUpgrade>();
        }*/
        #endregion
    }

    public void OnEnable()
    {
        MainMenuController.Instance.LoadUpgrades();
        LoadUpgradeList();
        UpdatedGoldUI();
    }

    //init updrade list (depracated)
    /*
    public void InitUpgradeList()
    {
        //load values to tempUpgrade
        tempUpgrade.upgradeName = "Strength";
        tempUpgrade.UpgradeLevel = 0;
        tempUpgrade.upValues = new int[5];


        //save it to player prefs so when you start a game it will load the value in the fight zone scene correctly
        //PlayerPrefs.SetInt(tempUpgrade.upgradeName,tempUpgrade.upgradeLevel);
        upgradeList.Add(tempUpgrade);

        //--------------------------------------------
        tempUpgrade.upgradeName = "HP";
        tempUpgrade.UpgradeLevel = 0;
        tempUpgrade.upValues = new int[5];

        //PlayerPrefs.SetInt(tempUpgrade.upgradeName,tempUpgrade.upgradeLevel);
        upgradeList.Add(tempUpgrade);

        //--------------------------------------------
        tempUpgrade.upgradeName = "Armor";
        tempUpgrade.UpgradeLevel = 0;
        tempUpgrade.upValues = new int[5];

        //PlayerPrefs.SetInt(tempUpgrade.upgradeName,tempUpgrade.upgradeLevel);
        upgradeList.Add(tempUpgrade);

        //--------------------------------------------
        tempUpgrade.upgradeName = "Speed";
        tempUpgrade.UpgradeLevel = 0;
        tempUpgrade.upValues = new int[5];

        //PlayerPrefs.SetInt(tempUpgrade.upgradeName,tempUpgrade.upgradeLevel);
        upgradeList.Add(tempUpgrade);
        //--------------------------------------------
        tempUpgrade.upgradeName = "Magnet";
        tempUpgrade.UpgradeLevel = 0;
        tempUpgrade.upValues = new int[5];

        //PlayerPrefs.SetInt(tempUpgrade.upgradeName,tempUpgrade.upgradeLevel);
        upgradeList.Add(tempUpgrade);
        //--------------------------------------------
        tempUpgrade.upgradeName = "Inflation";
        tempUpgrade.UpgradeLevel = 1;
        tempUpgrade.upValues = new int[5];

        //PlayerPrefs.SetInt(tempUpgrade.upgradeName,tempUpgrade.upgradeLevel);
        upgradeList.Add(tempUpgrade);
    }*/

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            mainController._playerGold += 10;
            UpdatedGoldUI();
        }
    }

    public void LoadUpgradeList()
    {
        int i = 0;
        foreach(MainMenuController.BaseUpgrade bu in upgradeList)
        {
            //GameObject temp;
            //temp = Instantiate(upgradeSquarePrefab, upgradeParentObj.transform);
            //set name and level of the prefab
            if(!bu.IsMaxLevel())
            {
                upgradePrefabs[i].GetComponent<BaseUpgradeSquare>().SetUpgradeVisuals(bu.upgradeName, bu.UpgradeLevel, upgradeCostValues[bu.UpgradeLevel]);
            }
            else
            {
                upgradePrefabs[i].GetComponent<BaseUpgradeSquare>().SetUpgradeVisuals(bu.upgradeName, bu.UpgradeLevel, -1);
            }
            
            i++;
        }
    }

    public void DeselectAll()
    {
        foreach(GameObject go in upgradePrefabs)
        {
            go.GetComponent<BaseUpgradeSquare>().isSelected = false;
            go.GetComponent<BaseUpgradeSquare>().SetUnFocused();
        }
    }
    public void DeselectAllV2()
    {
        foreach(GameObject go in upgradePrefabs)
        {
            go.GetComponent<BaseUpgradeSquare>().uiBGObj.SetNormal(false);
        }
    }

    public void SelectUpgrade(GameObject me)
    {
        #region old way
        // DeselectAll();
        // me.GetComponent<BaseUpgradeSquare>().isSelected = true;
        // me.GetComponent<BaseUpgradeSquare>().SetHover();
        // StartCoroutine(ShopItemSelectionManager.instance.SetSelectedAfterOneFrame(7, true));
        // //EventSystem.current.SetSelectedGameObject(ShopItemSelectionManager.instance.shopItems[7]);
        // ShopItemSelectionManager.instance.shopItems[7].GetComponent<BuyButtonLogic>().PopulateBuyButton(me.GetComponent<BaseUpgradeSquare>().baseUpgradeName, me.GetComponent<BaseUpgradeSquare>().toolTIP, me.GetComponent<BaseUpgradeSquare>().BaseCost);

        #endregion
    
        #region new way
            DeselectAllV2();
            me.GetComponent<BaseUpgradeSquare>().uiBGObj.SetGreen(true);
            StartCoroutine(selectionManager.SetSelectedAfterOneFrame(8, true));
            selectionManager.menuItems[8].GetComponent<BuyButtonLogic>().PopulateBuyButton(me.GetComponent<BaseUpgradeSquare>().baseUpgradeName, me.GetComponent<BaseUpgradeSquare>().toolTIP, me.GetComponent<BaseUpgradeSquare>().BaseCost, false);

        #endregion
    }
    public void ChooseUpgrade(string upName)
    {
        BaseUpgradeSquare bus = null;
        MainMenuController.BaseUpgrade tempUpgrade = new()
        {
            upgradeName = "",
            UpgradeLevel = 0
        };

        //Get the upgrade from the upgrade list to update the data to be saved
        foreach (MainMenuController.BaseUpgrade bu in upgradeList)
        {
            if (bu.upgradeName == upName)
            {
                tempUpgrade = bu;
            }
        }

        //if not max level then you can see if you can buy
        if(!tempUpgrade.IsMaxLevel())
        {
            if(mainController._playerGold < upgradeCostValues[tempUpgrade.UpgradeLevel])
            {
                //cant buy
                Debug.Log("Cant buy");
            }
            else
            {
                //subtract money
                mainController._playerGold -= upgradeCostValues[tempUpgrade.UpgradeLevel];
                UpdatedGoldUI();
                
                //up the level
                tempUpgrade.UpgradeLevel++;

                //update the values to be loaded into the fight zone
                mainController.UpdatePlayerPrefs();

                //Get the visual gameobject to update
                foreach(GameObject go in upgradePrefabs)
                {
                    if(tempUpgrade.upgradeName == go.GetComponent<BaseUpgradeSquare>().baseUpgradeName)
                    {
                        //update price on upgrade
                        bus = go.GetComponent<BaseUpgradeSquare>();
                        if(tempUpgrade.IsMaxLevel())
                        {
                            bus.SetUpgradeVisuals(tempUpgrade.UpgradeLevel, -1);
                        }
                        else
                        {
                            bus.SetUpgradeVisuals(tempUpgrade.UpgradeLevel, upgradeCostValues[tempUpgrade.UpgradeLevel]);
                        }
                         
                        bus.UpdateBar(true);              
                    }
                }  

                //save upgrade
                mainController.SaveUpgradeValues();

                //update upgrade visuals and buy button
                //ShopItemSelectionManager.instance.shopItems[7].GetComponent<BuyButtonLogic>().PopulateBuyButton(bus.baseUpgradeName, bus.toolTIP, bus.BaseCost, false);
                selectionManager.menuItems[8].GetComponent<BuyButtonLogic>().PopulateBuyButton(bus.baseUpgradeName, bus.toolTIP, bus.BaseCost, false);
            }
        }
        else
        {
            Debug.Log("Upgrade is Max level");
        }
    }

    public void SaveStuffButton()
    {
        mainController.SaveUpgradeValues();
        DeselectAllV2();
    }

    public void UpdatedGoldUI()
    {
        goldText.text = mainController._playerGold.ToString();
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
