using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class AnimTabToggle : MonoBehaviour
{
    [SerializeField][Tooltip("Ȱ�� �� ������Ʈ")]
    private GameObject activeObj;

    [Tooltip("�þ ��� �̹���")]
    public RectTransform background;

    [Tooltip("Ȱ�� �� ǥ���� �ؽ�Ʈ")]
    public GameObject label;
    
    [Tooltip("�ִϸ��̼� �ð�")]
    public float animDuration = 0.3f;

    [Tooltip("�þ ������")]
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
        
        // �ؽ�Ʈ�� instant ���
        if (label) label.SetActive(isOn);
        
        // ����� ������ �ִϸ��̼�
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
