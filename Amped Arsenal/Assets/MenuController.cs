using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuController : MonoBehaviour
{
    public Image fadeImage;
    public void Start()
    {
        MainMenuController.Instance._currentFadeImage = fadeImage;
        GameSceneManager.instance.musicMaker.AquireSoundMaker();
        MainMenuController.Instance.opsController = GameSceneManager.instance.musicMaker.soundMaker.opsController;
        MainMenuController.Instance.opsController.InitOptionSettings();
       //MainMenuController.Instance.opsController.LoadOptionMenuValues();
    }
    public void LoadScene()
    {
        //MainMenuController.Instance.LoadScene(sceneName);
        GameSceneManager.instance.LoadGame();
    }

    public void SaveGame()
    {
        MainMenuController.Instance.SaveUpgradeValues();
    }

    public void DeleteGame()
    {
        MainMenuController.Instance.data.DeleteProfile();
    }

    public void OpenFileLocation()
    {
        MainMenuController.Instance.data.OpenProfileLocation();
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
    }
}
