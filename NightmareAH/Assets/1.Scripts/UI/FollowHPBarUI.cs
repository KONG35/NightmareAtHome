using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHPBarUI : MonoBehaviour
{
    public Transform target; 
    public Vector3 offset;  

    private Camera cam;
    private RectTransform rectTransform;

    void Start()
    {
        cam = Camera.main;
        rectTransform = GetComponent<RectTransform>();
        target = FindObjectOfType<PlayerCharacter>().transform;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 screenPos = cam.WorldToScreenPoint(target.position);
        rectTransform.position = screenPos + offset;
    }
}
