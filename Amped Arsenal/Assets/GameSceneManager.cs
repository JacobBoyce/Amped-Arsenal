using System.Collections;
using System.Collections.Generic;
//using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
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
    public GameObject loadingScreen;
    public Image progressBar;

    public bool startLoadingBuffer = false, loadingBufferDone, triggerEndLoadingScreen = false;
    public float loadBuffCD = 0, loadBuffCDMax;
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

    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    public void LoadGame()
    {
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
    }
}