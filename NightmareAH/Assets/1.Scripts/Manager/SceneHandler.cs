using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneHandler : Singleton<SceneHandler>
{
    private List<LoadingSheetTxt> loadingDataList = new List<LoadingSheetTxt>();
    private Coroutine unloadCor;
    private Coroutine loadCor;

    const int WAITTIME = 15;
    const int MinTime = 1;
    protected override void Awake()
    {
        base.Awake();

        unloadCor = null;
        loadCor = null;
        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
    }

    /// <summary>
    /// 씬 존재 확인 후 load async 요청 함수
    /// </summary>
    /// <param name="_scName"></param> 
    /// <param name="_mode"></param>
    public void AsyncLoadScene(string _scName, LoadSceneMode _mode)
    {
        if (string.IsNullOrEmpty(_scName) || IsSceneExists(_scName) == false)
        {
            Debug.LogError("Error : " + _scName + " 이라는 씬 이름이 존재하지 않습니다.");
            // !! 로비로 돌아가기
            return;
        }

        if (loadCor != null)
            StopCoroutine(loadCor);

        loadCor = StartCoroutine(AsyncLoadSceneCor(_scName, _mode));
    }
    /// <summary>
    /// 씬 존재 확인 후 unload , async 요청 함수
    /// </summary>
    /// <param name="_scName"></param> 
    public void AsyncUnLoadScene(string _scName)
    {
        if (string.IsNullOrEmpty(_scName) || IsSceneExists(_scName) == false)
        {
            Debug.LogError("Error : " + _scName + " 이라는 씬 이름이 존재하지 않습니다.");
            // !! 로비로 돌아가기
            return;
        }

        if (unloadCor != null)
            StopCoroutine(unloadCor);

        unloadCor = StartCoroutine(AsyncUnLoadSceneCor(_scName));

    }
    /// <summary>
    /// 비동기 씬 로드 함수 
    /// </summary>
    /// <param name="_scName"></param>
    /// <param name="_mode"></param>
    /// <returns></returns>
    IEnumerator AsyncLoadSceneCor(string _scName, LoadSceneMode _mode)
    {
        // LoadingData 리스트 가져오기
        if (loadingDataList.Count == 0)
            loadingDataList = GoogleSheetLoader.Instance.GetDataList<LoadingSheetTxt>();

        int ranIdx = UnityEngine.Random.Range(0, loadingDataList.Count);

        yield return null;

        float elapsedTime = 0f;
        float displayProgress = 0f; // 로딩바 
        AsyncOperation asyncOper = SceneManager.LoadSceneAsync(_scName.ToString(), _mode);
        asyncOper.allowSceneActivation = false; // 씬 자동 전환 방지

        while (!asyncOper.isDone && elapsedTime < WAITTIME)
        {
            elapsedTime += Time.deltaTime; // 프레임당 경과 시간 누적
            float targetProgress = asyncOper.progress < 0.9f ? asyncOper.progress : 1f; // 실제 진행률

            // 부드러운 증가를 위해 Lerp 사용
            displayProgress = Mathf.Lerp(displayProgress, targetProgress, Time.deltaTime * 3f);

            Debug.Log($"로딩 진행률 (표시): {displayProgress * 100}% / (실제): {asyncOper.progress * 100}%");

            // 씬이 준비되었고, 최소 5초가 경과하면 씬 전환
            if (asyncOper.progress >= 0.9f && elapsedTime >= MinTime)
            {
                asyncOper.allowSceneActivation = true;
            }
            FadeInoutUI.Instance.FadeIn();
            yield return null;
        }
    }
    /// <summary>
    /// 비동기 씬 언로드 함수
    /// </summary>
    /// <param name="_scName"></param>
    /// <returns></returns>
    IEnumerator AsyncUnLoadSceneCor(string _scName)
    {
        // LoadingData 리스트 가져오기
        List<LoadingSheetTxt> languageDataList = GoogleSheetLoader.Instance.GetDataList<LoadingSheetTxt>();
        int ranIdx = UnityEngine.Random.Range(0, languageDataList.Count);

        yield return null;

        float elapsedTime = 0f;
        float displayProgress = 0f; // 로딩바 
        AsyncOperation asyncOper = SceneManager.UnloadSceneAsync(_scName);
        asyncOper.allowSceneActivation = false; // 씬 자동 전환 방지

        while (!asyncOper.isDone && elapsedTime < WAITTIME)
        {
            elapsedTime += Time.deltaTime; // 프레임당 경과 시간 누적
            float targetProgress = asyncOper.progress < 0.9f ? asyncOper.progress : 1f; // 실제 진행률

            // 부드러운 증가를 위해 Lerp 사용
            displayProgress = Mathf.Lerp(displayProgress, targetProgress, Time.deltaTime * 3f);

            Debug.Log($"로딩 진행률 (표시): {displayProgress * 100}% / (실제): {asyncOper.progress * 100}%");

            // 씬이 준비되었고, 최소 minT 가 경과하면 씬 전환
            if (asyncOper.progress >= 0.9f && elapsedTime >= MinTime)
            {
                asyncOper.allowSceneActivation = true;
            }
            yield return null;
        }
    }
    /// <summary>
    /// 해당 이름의 씬이 존재하는지 확인하는 함수
    /// </summary>
    /// <param name="_scName"></param> 
    /// <returns></returns>
    private bool IsSceneExists(string _scName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scPath = SceneUtility.GetScenePathByBuildIndex(i);
            string scFileName = System.IO.Path.GetFileNameWithoutExtension(scPath);
            if (scFileName == _scName)
            {
                return true;
            }
        }
        return false;
    }
}
