using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeInoutUI:Singleton<FadeInoutUI>
{
    [Tooltip("Canvas Image with transparent background fade sprite")]
    public Image fadeImg;

    [Tooltip("페이드 인/아웃에 걸릴 시간 (초)")]
    public float duration = 1f;

    // 화면을 완전히 가리기 위해 필요한 최종 scale
    private float maxScale;

    protected override void Awake()
    {
        base.Awake();

        RectTransform rt = fadeImg.rectTransform;
        RectTransform canvasRT = fadeImg.canvas.GetComponent<RectTransform>();

        // 화면 대각선 길이 계산 (UI 좌표계)
        float w = canvasRT.rect.width;
        float h = canvasRT.rect.height;
        float diag = Mathf.Sqrt(w * w + h * h);
        diag *= 1.2f;

        // Image의 크기를 대각선 길이로 설정
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(diag, diag);
        rt.pivot = new Vector2(0.5f, 0.5f);

        // 이제 scale 1이면 화면 꽉 덮음
        maxScale = 1f;
    }
    private void OnEnable()
    {
        //FadeOut(() => FadeIn());
        //FadeIn();
    }
    /// <summary>
    /// 중앙에서부터 image 커지며 전체를 덮는 페이드아웃
    /// </summary>
    public void FadeOut(Action onComplete = null)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateScale(0f, maxScale, duration, onComplete));
    }

    /// <summary>
    /// image 작아지며 사라지는 페이드인
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

        // 초기값 설정
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

        // 끝 점 보정
        rt.localScale = Vector3.one * to;

        if (to == 0f)
            fadeImg.gameObject.SetActive(false);  // 완전히 사라지면 비활성화

        onComplete?.Invoke();
    }
}
