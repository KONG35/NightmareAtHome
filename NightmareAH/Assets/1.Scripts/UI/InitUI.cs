using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class InitUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI loadingTxt;
    [SerializeField]
    private Button startBtn;

    private void Awake()
    {
        startBtn.onClick.AddListener(() => StartCoroutine(StartBtnOnCor()));
        startBtn.gameObject.SetActive(false);
        StartCoroutine(AnimateDots());

        SceneManager.LoadSceneAsync("3.Loading", LoadSceneMode.Additive);
    }
    private IEnumerator Start()
    {
        yield return StartCoroutine(GoogleSheetLoader.Instance.LoadAllCSVs());

        loadingTxt.gameObject.SetActive(false);
        startBtn.gameObject.SetActive(true);
    }
    private void OnDisable()
    {
        StopCoroutine("AnimateDots");
    }
    private IEnumerator StartBtnOnCor()
    {
        startBtn.onClick.RemoveAllListeners();

        yield return null;

        //// 로비 → 게임 씬 전환 시 페이드아웃
        FadeInoutUI.Instance.FadeOut(() =>
        {
            // 영상이나 다음 씬 로드 시작
            SceneHandler.Instance.AsyncUnLoadScene("0.Init");
            SceneHandler.Instance.AsyncLoadScene("1.Lobby", LoadSceneMode.Additive);
        });
    }
    private IEnumerator AnimateDots()
    {
        int dotCount = 0;
        while (true)
        {
            // 1~3 순환
            dotCount = (dotCount % 3) + 1;

            // "loading" + dotCount 개의 '.'
            loadingTxt.text = "Loading" + new string('.', dotCount);

            // 0.5초 대기
            yield return new WaitForSeconds(0.5f);
        }
    }
}
