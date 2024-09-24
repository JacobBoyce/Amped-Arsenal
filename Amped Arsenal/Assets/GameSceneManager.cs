using System;
using System.Collections;
using System.Collections.Generic;
//using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneManager : MonoBehaviour
{
    public enum SceneIndexes
    {
        MANAGER = 0,
        MAINMENU = 1,
        FIGHTZONE = 2
    }
    public static GameSceneManager instance;
    public MusicMaker musicMaker;
    public GameObject loadingScreen;
    public Image progressBar;

    public bool startLoadingBuffer = false, loadingBufferDone, triggerEndLoadingScreen = false;
    public float loadBuffCD = 0, loadBuffCDMax;
    public bool onSteamDeck, triggerAllowInputSwitching = false;


    public static Action<InputMode> OnInputModeChanged;
    //public ProgressBar pBar
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void LoadPersistentSceneFirst()
    {
        //Scene currentScene = SceneManager.GetActiveScene();
        //SceneManager.UnloadSceneAsync(currentScene.buildIndex);
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        //instance.LoadGame(SceneIndexes.MANAGER);
    }
    private void Awake()
    {
        
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        loadBuffCD = 0;
    }

    // public bool IsOnSteamDeck()
    // {
    //     onSteamDeck = Steamworks.SteamUtils.IsSteamRunningOnSteamDeck();
    //     return onSteamDeck;
    // }

    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    public void LoadGame()
    {
        //deactivate playerinput
        loadingScreen.SetActive(true);
        triggerEndLoadingScreen = false;
        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.MAINMENU));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.FIGHTZONE, LoadSceneMode.Additive));

        totalSceneProgress = 0;
        totalBufferProgress = 0;
        loadBuffCD = 0;
        loadingBufferDone = false;
        startLoadingBuffer = false;

        StartCoroutine(GetSceneLoadProgress());
        StartCoroutine(GetTotalProgress());
    }

    public void LoadMainMenu()
    {
        musicMaker.SwapTrack(musicMaker.mainMenuMusic);
        Time.timeScale = 1;
        loadingScreen.SetActive(true);
        triggerEndLoadingScreen = false;
        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.FIGHTZONE));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.MAINMENU, LoadSceneMode.Additive));

        totalSceneProgress = 0;
        totalBufferProgress = 0;
        loadBuffCD = 0;
        loadingBufferDone = false;
        startLoadingBuffer = false;
        triggerAllowInputSwitching = true;

        StartCoroutine(GetSceneLoadProgress());
        StartCoroutine(GetTotalProgress());
    }

    float totalSceneProgress;
    float totalBufferProgress;
    public IEnumerator GetSceneLoadProgress()
    {
        for(int i = 0; i < scenesLoading.Count; i++)
        {
            while(!scenesLoading[i].isDone)
            {
                totalSceneProgress = 0;
                foreach(AsyncOperation operation in scenesLoading)
                {
                    totalSceneProgress += operation.progress;
                }

                totalSceneProgress /= scenesLoading.Count;
                
                yield return null;
            }
        }
    }
    
    public IEnumerator GetTotalProgress()
    {
        float totalProgress = 0;
        startLoadingBuffer = true;

        while(!loadingBufferDone)
        {
            totalBufferProgress = loadBuffCD / loadBuffCDMax;
            totalProgress = (totalBufferProgress + totalSceneProgress) / 2;
            progressBar.fillAmount = totalProgress;
            yield return null;
        }

        loadingScreen.SetActive(false);
        triggerEndLoadingScreen = true;
    }

    public void Update()
    {
        if(startLoadingBuffer)
        {
            if(loadBuffCD < loadBuffCDMax)
            {
                loadBuffCD += Time.unscaledDeltaTime;
            }
            else
            {
                startLoadingBuffer = false;
                loadingBufferDone = true;
            }
        }

        _currentInputMode = DetectInput();

        if(_currentInputMode != _inputModeLastFrame)
        {
            OnInputModeChanged?.Invoke(_currentInputMode);
        }

        _inputModeLastFrame = DetectInput();
    }

    public enum InputMode{Controller,Keyboard}
    public InputMode _currentInputMode;
    public InputMode _inputModeLastFrame;

    public InputMode DetectInput()
    {
        if(Input.GetJoystickNames().Length == 0)
        {
            return InputMode.Keyboard;
        }

        if(Input.anyKeyDown)
        {
            if(Input.GetKeyDown(KeyCode.JoystickButton0)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton1)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton2)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton3)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton4)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton5)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton6)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton7)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton8)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton9)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton10)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton11)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton12)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton13)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton14)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton15)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton16)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton17)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton18)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton19)) return InputMode.Controller;
            else return InputMode.Keyboard;

            
        }

        return _currentInputMode;
    }
}