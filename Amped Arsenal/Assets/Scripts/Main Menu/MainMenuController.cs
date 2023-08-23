using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public UpgradeController upController;
    private BaseUpgrade tempUpgrade = new BaseUpgrade();
    // Start is called before the first frame update
    void Start()
    {
        //create gold saving between scenes
        PlayerPrefs.SetInt("Gold",0);

        //create player prefs for base stat upgrades and thier level (key, value) = (base stat, stat level) = (strength, 3)
        InitPlayerPrefs();

        //load save and add to player prefs gold value
        LoadBaseUpgradeStats();
        upController.LoadUpgradeList();
    }

    public void LoadBaseUpgradeStats()
    {
        

        //load values to tempUpgrade
        tempUpgrade.upgradeName = "Strength";
        tempUpgrade.upgradeLevel = 1;

        //save it to player prefs so when you start a game it will load the value in the fight zone scene correctly
        //PlayerPrefs.SetInt(tempUpgrade.upgradeName,tempUpgrade.upgradeLevel);
        upController.upgradeList.Add(tempUpgrade);

        //--------------------------------------------
        tempUpgrade.upgradeName = "HP";
        tempUpgrade.upgradeLevel = 1;

        //PlayerPrefs.SetInt(tempUpgrade.upgradeName,tempUpgrade.upgradeLevel);
        upController.upgradeList.Add(tempUpgrade);
        //--------------------------------------------
        tempUpgrade.upgradeName = "Speed";
        tempUpgrade.upgradeLevel = 1;

        //PlayerPrefs.SetInt(tempUpgrade.upgradeName,tempUpgrade.upgradeLevel);
        upController.upgradeList.Add(tempUpgrade);
        //--------------------------------------------
        tempUpgrade.upgradeName = "Magnet";
        tempUpgrade.upgradeLevel = 1;

        //PlayerPrefs.SetInt(tempUpgrade.upgradeName,tempUpgrade.upgradeLevel);
        upController.upgradeList.Add(tempUpgrade);
        //--------------------------------------------
        tempUpgrade.upgradeName = "Inflation";
        tempUpgrade.upgradeLevel = 1;

        //PlayerPrefs.SetInt(tempUpgrade.upgradeName,tempUpgrade.upgradeLevel);
        upController.upgradeList.Add(tempUpgrade);
    }

    public void InitPlayerPrefs()
    {
        PlayerPrefs.SetInt("Strength",0);
        PlayerPrefs.SetInt("HP",0);
        PlayerPrefs.SetInt("Speed",0);
        PlayerPrefs.SetInt("Magnet",0);
        PlayerPrefs.SetInt("Inflation",0);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    [System.Serializable]
    public struct BaseUpgrade
    {
        [SerializeField]
        public string upgradeName;
        [SerializeField]
        public int upgradeLevel;
    }
}
