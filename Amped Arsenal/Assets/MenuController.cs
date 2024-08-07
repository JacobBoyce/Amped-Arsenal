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
    }
    public void LoadScene(string sceneName)
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
