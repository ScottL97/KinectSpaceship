using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour
{
    public Text LoadingText;
    public Slider Process;
    private AsyncOperation asyn;
    void Start()
    {
        StartCoroutine(BeginLoading("Game"));
    }
    IEnumerator BeginLoading(string SceneName)
    {
        asyn = SceneManager.LoadSceneAsync(SceneName);
        yield return asyn;
    }
    void Update()
    {
        Process.value = asyn.progress;
        LoadingText.text = "加载进度：" + (Process.value * 100) + "%";
    }
}
