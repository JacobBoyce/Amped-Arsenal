using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    public MainMenuController mainController;
    public GameObject upgradeParentObj, upgradeSquarePrefab;
    [SerializeField]
    public List<MainMenuController.BaseUpgrade> upgradeList;

    [Space(10)]

    public int[] upgradeCostValues = new int[5];

    public void Awake()
    {
        upgradeList = new List<MainMenuController.BaseUpgrade>();
    }

    public void LoadUpgradeList()
    {
        foreach(MainMenuController.BaseUpgrade bu in upgradeList)
        {
            GameObject temp;
            temp = Instantiate(upgradeSquarePrefab, upgradeParentObj.transform);
            //set name and level of the prefab
            temp.GetComponent<BaseUpgradeSquare>().SetNameAndLevel(bu.upgradeName, bu.upgradeLevel);
            //set the cost of the prefab
            temp.GetComponent<BaseUpgradeSquare>().BaseCost = upgradeCostValues[bu.upgradeLevel];
        }
    }

}
