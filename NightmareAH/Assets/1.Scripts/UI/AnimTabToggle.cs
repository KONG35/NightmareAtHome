using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class AnimTabToggle : MonoBehaviour
{
    [SerializeField][Tooltip("활성 시 오브젝트")]
    private GameObject activeObj;

    [Tooltip("늘어날 배경 이미지")]
    public RectTransform background;

    [Tooltip("활성 시 표시할 텍스트")]
    public GameObject label;
    
    [Tooltip("애니메이션 시간")]
    public float animDuration = 0.3f;

    [Tooltip("늘어날 스케일")]
    public float targetScale = 1.3f;

    private Toggle tog;
    private Coroutine _anim;
    private void Awake()
    {
        tog = gameObject.GetComponent<Toggle>();
        OnToggle(tog.isOn);
        tog.onValueChanged.AddListener(isOn => OnToggle(isOn));
    }
    public void OnToggle(bool isOn)
    {
        activeObj.SetActive(isOn);
        
        // 텍스트는 instant 토글
        if (label) label.SetActive(isOn);
        
        // 배경은 스케일 애니메이션
        if (_anim != null) StopCoroutine(_anim);
        _anim = StartCoroutine(ScaleBG(isOn ? targetScale : 1f));
    }

    private IEnumerator ScaleBG(float _scale)
    {
        float start = background.localScale.x;
        float elapsed = 0f;
        while (elapsed < animDuration)
        {
            float t = elapsed / animDuration;
            float s = Mathf.Lerp(start, _scale, t);
            background.localScale = Vector3.one * s;
            elapsed += Time.deltaTime;
            yield return null;
        }
        background.localScale = Vector3.one * _scale;
    }
}
