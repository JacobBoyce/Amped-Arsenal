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
        MainMenuController.Instance.LoadScene(sceneName);
    }

    public void SaveGame()
    {
        MainMenuController.Instance.SaveUpgradeValues();
    }
}
