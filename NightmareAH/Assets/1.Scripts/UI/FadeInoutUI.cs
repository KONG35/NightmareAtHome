using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeInoutUI:Singleton<FadeInoutUI>
{
    [Tooltip("Canvas Image with transparent background fade sprite")]
    public Image fadeImg;

    [Tooltip("���̵� ��/�ƿ��� �ɸ� �ð� (��)")]
    public float duration = 1f;

    // ȭ���� ������ ������ ���� �ʿ��� ���� scale
    private float maxScale;

    protected override void Awake()
    {
        base.Awake();

        RectTransform rt = fadeImg.rectTransform;
        RectTransform canvasRT = fadeImg.canvas.GetComponent<RectTransform>();

        // ȭ�� �밢�� ���� ��� (UI ��ǥ��)
        float w = canvasRT.rect.width;
        float h = canvasRT.rect.height;
        float diag = Mathf.Sqrt(w * w + h * h);
        diag *= 1.2f;

        // Image�� ũ�⸦ �밢�� ���̷� ����
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(diag, diag);
        rt.pivot = new Vector2(0.5f, 0.5f);

        // ���� scale 1�̸� ȭ�� �� ����
        maxScale = 1f;
    }
    private void OnEnable()
    {
        //FadeOut(() => FadeIn());
        //FadeIn();
    }
    /// <summary>
    /// �߾ӿ������� image Ŀ���� ��ü�� ���� ���̵�ƿ�
    /// </summary>
    public void FadeOut(Action onComplete = null)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateScale(0f, maxScale, duration, onComplete));
    }

    /// <summary>
    /// image �۾����� ������� ���̵���
    /// </summary>
    public void FadeIn(Action onComplete = null)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateScale(maxScale, 0f, duration, onComplete));
    }

    private IEnumerator AnimateScale(float from, float to, float time, Action onComplete)
    {
        var rt = fadeImg.rectTransform;
        float elapsed = 0f;

        // �ʱⰪ ����
        rt.localScale = Vector3.one * from;
        fadeImg.gameObject.SetActive(true);

        while (elapsed < time)
        {
            float t = elapsed / time;
            float s = Mathf.Lerp(from, to, t);
            rt.localScale = Vector3.one * s;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // �� �� ����
        rt.localScale = Vector3.one * to;

        if (to == 0f)
            fadeImg.gameObject.SetActive(false);  // ������ ������� ��Ȱ��ȭ

        onComplete?.Invoke();
    }
}
