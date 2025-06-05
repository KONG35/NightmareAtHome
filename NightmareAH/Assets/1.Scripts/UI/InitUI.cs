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

        //// �κ� �� ���� �� ��ȯ �� ���̵�ƿ�
        FadeInoutUI.Instance.FadeOut(() =>
        {
            // �����̳� ���� �� �ε� ����
            SceneHandler.Instance.AsyncUnLoadScene("0.Init");
            SceneHandler.Instance.AsyncLoadScene("1.Lobby", LoadSceneMode.Additive);
        });
    }
    private IEnumerator AnimateDots()
    {
        int dotCount = 0;
        while (true)
        {
            // 1~3 ��ȯ
            dotCount = (dotCount % 3) + 1;

            // "loading" + dotCount ���� '.'
            loadingTxt.text = "Loading" + new string('.', dotCount);

            // 0.5�� ���
            yield return new WaitForSeconds(0.5f);
        }
    }
}
