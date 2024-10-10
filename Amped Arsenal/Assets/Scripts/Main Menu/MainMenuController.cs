using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DataManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MainMenuController : MonoBehaviour
{
    public static MainMenuController Instance{get; private set;}

    public Data data;
    public UpgradeController upController;
    public OptionsMenuController opsController;
    public int _playerGold = 0;
    public int stageDifficulty = 1;

    [Header("Fade Stuff"), Space(10)]
    [Range(0.1f, 10f), SerializeField] private float _fadeOutSpeed = 5f;
    [Range(0.1f, 10f), SerializeField] private float _fadeInSpeed = 5f;
    [SerializeField] public Color _fadeOutStartColor;
    [SerializeField] public Image _currentFadeImage;

    public bool IsFadingOut {get; private set;}
    public bool IsFadingIn {get; private set;}

    public enum SaveNames
    {
        PLAYTEST,
        DEMO,
        MAINSAVE
    }
    public SaveNames saveFileToUse;

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (Instance != null && Instance != this) 
        { 
            Destroy(this.gameObject); 
        } 
        else 
        { 
            Instance = this; 
        } 

        if(saveFileToUse == SaveNames.PLAYTEST)
        {
            data = new Data("Playtest");
        }
        else if(saveFileToUse == SaveNames.DEMO)
        {
            data = new Data("Demo");
        }
        else if(saveFileToUse == SaveNames.DEMO)
        {
            data = new Data("MainSave");
        }
        //data = new Data("MainSave");
    }
    
    void Start()
    {
        _fadeOutStartColor.a = 0f;
        
        //load save and add to player prefs gold value
        LoadData();
        UpdatePlayerPrefs();
    }

    public void Update()
    {
        if(IsFadingOut)
        {
            if(_currentFadeImage.color.a < 1f)
            {
                _fadeOutStartColor.a += Time.unscaledDeltaTime * _fadeOutSpeed;
                _currentFadeImage.color = _fadeOutStartColor;
            }
            else
            {
                IsFadingOut = false;

            }
        }

        if(IsFadingIn)
        {
            if(_currentFadeImage.color.a > 0f)
            {
                _fadeOutStartColor.a -= Time.unscaledDeltaTime * _fadeInSpeed;
                _currentFadeImage.color = _fadeOutStartColor;
            }
            else
            {
                IsFadingIn = false;
            }
        }
    }

    public void StartFadeOut()
    {
        _currentFadeImage.color = _fadeOutStartColor;
        IsFadingOut = true;
    }
    public void StartFadeIn()
    {
        if(_currentFadeImage.color.a >= 1f)
        {
            _currentFadeImage.color = _fadeOutStartColor;
            IsFadingIn = true;
        }
    }
    public void LoadUpgrades()
    {
        if(data.HasData("UpgradeList"))
        {
            upController.upgradeList = data.GetData<List<BaseUpgrade>>("UpgradeList");
        }

        for(int i = 0; i < upController.upgradeList.Count; i++)
        {
            for(int j = 0; j < upController.upVals[i].upValues.Length; j++)
            {
                upController.upgradeList[i].upValues[j] = upController.upVals[i].upValues[j];
            }
            
        }
    }

    public void LoadData()
    {
        //init base upgrade list
        //upController.InitUpgradeList();
        #region Upgrade Loading
        if(data.HasData("UpgradeList"))
        {
            upController.upgradeList = data.GetData<List<BaseUpgrade>>("UpgradeList");
        }
        else
        {
            Debug.Log("No Upgrade Data Found");
        }
        #endregion

        #region Player Gold Loading
        if(data.HasData("PlayerGold"))
        {
            _playerGold = data.GetData<int>("PlayerGold");
        }
        else
        {
            Debug.Log("No Gold Data Found");
            data.SetData("PlayerGold", _playerGold);
            data.SaveProfile();
            _playerGold = 0;
        }
        #endregion

        #region Stage Difficulty Loading
        if(data.HasData("StageDifficulty"))
        {
            stageDifficulty = data.GetData<int>("StageDifficulty");
        }
        else
        {
            Debug.Log("No Stage Difficulty Data Found");
            data.SetData("StageDifficulty", 1);
            data.SaveProfile();
        }
        #endregion

        #region Option Settings Loading

        if(data.HasData("OptionSettings"))
        {
            //set options menu slider values
            opsController.optionSaveData = data.GetData<List<OptionData>>("OptionSettings");

            // Set Player Prefs
            PlayerPrefs.SetFloat("MusicVolumeSlider", float.Parse(opsController.optionSaveData[opsController.optionSaveData.FindIndex(opt => opt.optionName == "MusicVolumeSlider")].optionData));

            PlayerPrefs.SetFloat("SFXVolumeSlider", float.Parse(opsController.optionSaveData[opsController.optionSaveData.FindIndex(opt => opt.optionName == "SFXVolumeSlider")].optionData));

            PlayerPrefs.SetInt("FullScreenToggle", int.Parse(opsController.optionSaveData[opsController.optionSaveData.FindIndex(opt => opt.optionName == "FullScreenToggle")].optionData));
        
            PlayerPrefs.SetInt("HowToPlayOnFirstTime", int.Parse(opsController.optionSaveData[opsController.optionSaveData.FindIndex(opt => opt.optionName == "HowToPlayOnFirstTime")].optionData));
        }
        else
        {
            Debug.Log("No Option Data Found");
            PlayerPrefs.SetFloat("MusicVolumeSlider", 1);
            PlayerPrefs.SetFloat("SFXVolumeSlider", 1);
            PlayerPrefs.SetInt("FullScreenToggle", 1);
            PlayerPrefs.SetInt("HowToPlayOnFirstTime", 1);
            data.SaveProfile();
        }

        #endregion

        //sets the unseen values of each upgrade
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
    }

    public void SaveUpgradeValues()
    {
        data.SetData("UpgradeList", upController.upgradeList);
        data.SetData("PlayerGold", _playerGold);
        data.SetData("StageDifficulty", stageDifficulty);
        data.SaveProfile();
    }

    public void SaveProgress()
    {
        data.SetData("StageDifficulty", stageDifficulty);
        data.SaveProfile();
    }

    public void SaveGoldData()
    {
        data.SetData("PlayerGold", _playerGold);
        data.SaveProfile();
    }

    public void SaveOptionsData()
    {
        data.SetData("OptionSettings", GameSceneManager.instance.musicMaker.soundMaker.opsController.optionSaveData);
        data.SaveProfile();
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine("LoadSceneCoroutine", sceneName);
    }

    public IEnumerator LoadSceneCoroutine(string sceneName)
    {
        StartFadeOut();
        while(IsFadingOut)
        {
            yield return null;
        }

        SceneManager.LoadSceneAsync(sceneName);
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
        private int maxLvl = 10;
        public int UpgradeLevel
        {
            get{return _upgradeLevel;}
            set
            {
                if(value > maxLvl)
                {
                    _upgradeLevel = maxLvl;
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

        public int[] upValues;

        public bool IsMaxLevel()
        {
            if(UpgradeLevel == maxLvl)
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
