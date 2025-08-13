using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    private AsyncOperation async; // 로딩
    private bool canOpen = true;

    void Start()
    {
        StartCoroutine("Load");
    }

    // 로딩
    IEnumerator Load()
    {
        async = SceneManager.LoadSceneAsync("GamePlay"); // 열고 싶은 씬
        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            yield return true;

            if (canOpen)
                async.allowSceneActivation = true;
        }
    }
}