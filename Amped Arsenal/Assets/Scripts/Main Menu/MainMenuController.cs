using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DataManagement;
using Unity.VisualScripting;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class MainMenuController : MonoBehaviour
{
    public Data data;
    public UpgradeController upController;
    public int _playerGold = 0;
    public int stageDifficulty = 1;
    public TextMeshProUGUI goldText;

    public int PlayerGold
    {
        get{return _playerGold;}
        set
        {
            _playerGold = value;          
            UpdateGoldGUI();
        }
    }

    public int returnedFromPlaying = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        stageDifficulty = 1;
 
        data = new Data("MainSave");

        //load save and add to player prefs gold value
        LoadData();
        upController.LoadUpgradeList();
        UpdatePlayerPrefs();
        

        //if just played bool is true then grab gold from player pref to save to playerGold
        if(PlayerPrefs.HasKey("Returned"))
        {
            returnedFromPlaying = PlayerPrefs.GetInt("Returned");
            PlayerPrefs.SetInt("Returned", 0);
        } 
        else
        {
            Debug.Log("returned doesnt exist, creating var now");
            PlayerPrefs.SetInt("Returned", 0);
        }

        //create gold saving between scenes
        if(returnedFromPlaying == 1)
        {
            PlayerGold += PlayerPrefs.GetInt("Gold");
            SaveUpgradeValues();
            returnedFromPlaying = 0;
        }
    }

    public void LoadData()
    {
        //init base upgrade list
        //upController.InitUpgradeList();
    
        if (data.HasData("UpgradeList") && data.HasData("PlayerGold") && data.HasData("StageDifficulty"))
        {
            //has save data
            
            upController.upgradeList = data.GetData<List<BaseUpgrade>>("UpgradeList");
            PlayerGold = data.GetData<int>("PlayerGold");
            stageDifficulty = data.GetData<int>("StageDifficulty");
        }
        else
        {
            Debug.Log("no save data found");
            data.SetData("StageDifficulty", stageDifficulty);
            data.SetData("PlayerGold", PlayerGold);
            PlayerGold = 0;
        }

        //load upvalues to the upgrade list
        /*for(int i = 0; i < upController.upgradeList.Count; i++)
        {
            //upController.upgradeList[i].upValues = new int[5];
            for(int k = 0; k < upController.upgradeList[i].upValues.Length; k++)
            {
                upController.upgradeList[i].upValues[k] = upController.upVals[j].upValues[k];
            }
            //System.Array.Copy(upController.upVals[j].upValues, bu.upValues, 5);
            j++;
        }*/

        for(int i = 0; i < upController.upgradeList.Count; i++)
        {
            for(int j = 0; j < upController.upVals[i].upValues.Length; j++)
            {
                upController.upgradeList[i].upValues[j] = upController.upVals[i].upValues[j];
            }
            
        }
       
    }

    public void UpdatePlayerPrefs()
    {
        foreach(BaseUpgrade bu in upController.upgradeList)
        {
            //Debug.Log(upController.upVals[bu.upgradeLevel].upValues[bu.upgradeLevel]);
            if(bu.IsMaxLevel())
            {
                PlayerPrefs.SetInt(bu.upgradeName, bu.upValues[bu.UpgradeLevel-1]);
            }
            else
            {
                PlayerPrefs.SetInt(bu.upgradeName, bu.upValues[bu.UpgradeLevel]);
            }
        }
        PlayerPrefs.SetInt("StageDifficulty", stageDifficulty);
    }

    public void SaveUpgradeValues()
    {
        data.SetData("UpgradeList", upController.upgradeList);
        data.SetData("PlayerGold", PlayerGold);
        data.SetData("StageDifficulty", stageDifficulty);
        data.SaveProfile();
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void UpdateGoldGUI()
    {
        goldText.text = PlayerGold.ToString();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    public void Open()
    {
        data.OpenProfileLocation();
    }

    public void DeleteFile()
    {
        data.DeleteProfile();
    }

    [System.Serializable]
    public class BaseUpgrade
    {
        [SerializeField]
        public string upgradeName;

        private int _upgradeLevel;
        public int UpgradeLevel
        {
            get{return _upgradeLevel;}
            set
            {
                if(value > 5)
                {
                    _upgradeLevel = 5;
                }
                else if (value < 0 )
                {
                    _upgradeLevel = 0;
                }
                else
                {
                    _upgradeLevel = value;
                }
            }
        }

        public int[] upValues = new int[5];

        public bool IsMaxLevel()
        {
            if(UpgradeLevel == 5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
