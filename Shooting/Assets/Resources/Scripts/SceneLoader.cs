using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader: MonoBehaviour
{
    private static Action onLoaderCallback;
    private static AsyncOperation loadingAsyncOperation;
    private class LoadingMonoBehavior : MonoBehaviour { }

    public static void Load(SceneType scene)
    {
        GameObject loadingGameObject = new GameObject("LoadingObj");
        loadingGameObject.AddComponent<LoadingMonoBehavior>().StartCoroutine(LoadSceneAsync(scene));
        //onLoaderCallback = () =>
        //{
        //    GameObject loadingGameObject = new GameObject("LoadingObj");
        //    loadingGameObject.AddComponent<LoadingMonoBehavior>().StartCoroutine(LoadSceneAsync(scene));           
        //};
    }

    public static float GetLoadingProgress()
    {
        if(loadingAsyncOperation != null)
        {
            return loadingAsyncOperation.progress;
        }
        else
        {
            return 1.0f;
        }
    }

    private static IEnumerator LoadSceneAsync(SceneType scene)
    {
        yield return null;
        loadingAsyncOperation = SceneManager.LoadSceneAsync(scene.ToString());
        while(!loadingAsyncOperation.isDone)
        {
            yield return null;
        }
    }
    //public static void LoaderCallback()
    //{
    //    if (onLoaderCallback != null)
    //    {
    //        onLoaderCallback();
    //        onLoaderCallback= null;
    //    }
    //}
}
