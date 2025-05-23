using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Toggle))]
public class BasicTabToggle : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Ȱ��ȭ�� ������Ʈ")]
    private GameObject activeObj;
    private Toggle tog;
    private void Awake()
    {
        tog = gameObject.GetComponent<Toggle>();
        activeObj.SetActive(tog.isOn);
        tog.onValueChanged.AddListener(isOn => { activeObj.SetActive(isOn); });
    }
}
