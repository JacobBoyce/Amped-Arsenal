using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DataManagement;
using UnityEngine.UI;


public class MainMenuController : MonoBehaviour
{
    public static MainMenuController Instance{get; private set;}
    public Data data;
    public UpgradeController upController;
    public int _playerGold = 0;
    public int stageDifficulty = 1;

    [Header("Fade Stuff"), Space(10)]
    [Range(0.1f, 10f), SerializeField] private float _fadeOutSpeed = 5f;
    [Range(0.1f, 10f), SerializeField] private float _fadeInSpeed = 5f;
    [SerializeField] public Color _fadeOutStartColor;
    [SerializeField] public Image _currentFadeImage;

    public bool IsFadingOut {get; private set;}
    public bool IsFadingIn {get; private set;}

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
        
        data = new Data("MainSave");
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
    
        if (data.HasData("UpgradeList") && data.HasData("PlayerGold") && data.HasData("StageDifficulty"))
        {
            //has save data
            
            upController.upgradeList = data.GetData<List<BaseUpgrade>>("UpgradeList");
            _playerGold = data.GetData<int>("PlayerGold");
            stageDifficulty = data.GetData<int>("StageDifficulty");
        }
        else
        {
            Debug.Log("no save data found");
            data.SetData("StageDifficulty", 1);
            data.SetData("PlayerGold", _playerGold);
            _playerGold = 0;
        }

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
