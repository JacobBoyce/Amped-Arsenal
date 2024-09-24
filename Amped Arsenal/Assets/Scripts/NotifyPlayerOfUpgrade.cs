using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotifyPlayerOfUpgrade : MonoBehaviour
{
    public GameObject normalUpButton, activeUpButton;
    public GameObject keyboardImage, gamepadImage;

    public void ChangeButtonImage(bool toggle)
    {
        normalUpButton.SetActive(!toggle);
        activeUpButton.SetActive(toggle);
        ChangeInputButtonImage();
    }


    public void OnEnable()
    {
        GameSceneManager.OnInputModeChanged += ChangeInputButtonImage;
    }

    public void OnDisable()
    {
        GameSceneManager.OnInputModeChanged -= ChangeInputButtonImage;
    }

    public void ChangeInputButtonImage(GameSceneManager.InputMode mode)
    {
        if(activeUpButton.activeInHierarchy)
        {
            if(mode == GameSceneManager.InputMode.Keyboard)
            {
                keyboardImage.SetActive(true);
                gamepadImage.SetActive(false);
            }
            else
            {
                keyboardImage.SetActive(false);
                gamepadImage.SetActive(true);
            }
        }
    }

    public void ChangeInputButtonImage()
    {
        if(activeUpButton.activeInHierarchy)
        {
            if(GameSceneManager.instance._currentInputMode == GameSceneManager.InputMode.Keyboard)
            {
                keyboardImage.SetActive(true);
                gamepadImage.SetActive(false);
            }
            else
            {
                keyboardImage.SetActive(false);
                gamepadImage.SetActive(true);
            }
        }
    }
}
