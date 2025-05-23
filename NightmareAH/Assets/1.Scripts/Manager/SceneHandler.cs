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
    /// �� ���� Ȯ�� �� load async ��û �Լ�
    /// </summary>
    /// <param name="_scName"></param> 
    /// <param name="_mode"></param>
    public void AsyncLoadScene(string _scName, LoadSceneMode _mode)
    {
        if (string.IsNullOrEmpty(_scName) || IsSceneExists(_scName) == false)
        {
            Debug.LogError("Error : " + _scName + " �̶�� �� �̸��� �������� �ʽ��ϴ�.");
            // !! �κ�� ���ư���
            return;
        }

        if (loadCor != null)
            StopCoroutine(loadCor);

        loadCor = StartCoroutine(AsyncLoadSceneCor(_scName, _mode));
    }
    /// <summary>
    /// �� ���� Ȯ�� �� unload , async ��û �Լ�
    /// </summary>
    /// <param name="_scName"></param> 
    public void AsyncUnLoadScene(string _scName)
    {
        if (string.IsNullOrEmpty(_scName) || IsSceneExists(_scName) == false)
        {
            Debug.LogError("Error : " + _scName + " �̶�� �� �̸��� �������� �ʽ��ϴ�.");
            // !! �κ�� ���ư���
            return;
        }

        if (unloadCor != null)
            StopCoroutine(unloadCor);

        unloadCor = StartCoroutine(AsyncUnLoadSceneCor(_scName));

    }
    /// <summary>
    /// �񵿱� �� �ε� �Լ� 
    /// </summary>
    /// <param name="_scName"></param>
    /// <param name="_mode"></param>
    /// <returns></returns>
    IEnumerator AsyncLoadSceneCor(string _scName, LoadSceneMode _mode)
    {
        // LoadingData ����Ʈ ��������
        if (loadingDataList.Count == 0)
            loadingDataList = GoogleSheetLoader.Instance.GetDataList<LoadingSheetTxt>();

        int ranIdx = UnityEngine.Random.Range(0, loadingDataList.Count);

        yield return null;

        float elapsedTime = 0f;
        float displayProgress = 0f; // �ε��� 
        AsyncOperation asyncOper = SceneManager.LoadSceneAsync(_scName.ToString(), _mode);
        asyncOper.allowSceneActivation = false; // �� �ڵ� ��ȯ ����

        while (!asyncOper.isDone && elapsedTime < WAITTIME)
        {
            elapsedTime += Time.deltaTime; // �����Ӵ� ��� �ð� ����
            float targetProgress = asyncOper.progress < 0.9f ? asyncOper.progress : 1f; // ���� �����

            // �ε巯�� ������ ���� Lerp ���
            displayProgress = Mathf.Lerp(displayProgress, targetProgress, Time.deltaTime * 3f);

            Debug.Log($"�ε� ����� (ǥ��): {displayProgress * 100}% / (����): {asyncOper.progress * 100}%");

            // ���� �غ�Ǿ���, �ּ� 5�ʰ� ����ϸ� �� ��ȯ
            if (asyncOper.progress >= 0.9f && elapsedTime >= MinTime)
            {
                asyncOper.allowSceneActivation = true;
            }
            FadeInoutUI.Instance.FadeIn();
            yield return null;
        }
    }
    /// <summary>
    /// �񵿱� �� ��ε� �Լ�
    /// </summary>
    /// <param name="_scName"></param>
    /// <returns></returns>
    IEnumerator AsyncUnLoadSceneCor(string _scName)
    {
        // LoadingData ����Ʈ ��������
        List<LoadingSheetTxt> languageDataList = GoogleSheetLoader.Instance.GetDataList<LoadingSheetTxt>();
        int ranIdx = UnityEngine.Random.Range(0, languageDataList.Count);

        yield return null;

        float elapsedTime = 0f;
        float displayProgress = 0f; // �ε��� 
        AsyncOperation asyncOper = SceneManager.UnloadSceneAsync(_scName);
        asyncOper.allowSceneActivation = false; // �� �ڵ� ��ȯ ����

        while (!asyncOper.isDone && elapsedTime < WAITTIME)
        {
            elapsedTime += Time.deltaTime; // �����Ӵ� ��� �ð� ����
            float targetProgress = asyncOper.progress < 0.9f ? asyncOper.progress : 1f; // ���� �����

            // �ε巯�� ������ ���� Lerp ���
            displayProgress = Mathf.Lerp(displayProgress, targetProgress, Time.deltaTime * 3f);

            Debug.Log($"�ε� ����� (ǥ��): {displayProgress * 100}% / (����): {asyncOper.progress * 100}%");

            // ���� �غ�Ǿ���, �ּ� minT �� ����ϸ� �� ��ȯ
            if (asyncOper.progress >= 0.9f && elapsedTime >= MinTime)
            {
                asyncOper.allowSceneActivation = true;
            }
            yield return null;
        }
    }
    /// <summary>
    /// �ش� �̸��� ���� �����ϴ��� Ȯ���ϴ� �Լ�
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
