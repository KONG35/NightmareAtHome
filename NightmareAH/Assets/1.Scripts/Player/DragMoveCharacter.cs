using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragMoveCharacter : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 startTouchPosition;
    private Vector3 targetPosition;
    private bool isDragging = false;
    public float speed = 1f;
    public float stopDistance = 0.05f;
    private Rigidbody rb;

    void Start()
    {
        mainCamera = Camera.main;
        targetPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            isDragging = true;
            startTouchPosition = GetMouseWorldPosition();
            startTouchPosition.z = transform.position.z;
        }
        if (Input.GetMouseButton(0))
        {
            targetPosition = GetMouseWorldPosition();
            targetPosition.z = transform.position.z;
        }
        if (Input.GetMouseButtonUp(0)) 
        {
            isDragging = false;
        }
    }
    void FixedUpdate()
    {
        float distance = Vector2.Distance(transform.position, targetPosition);

        if (distance > stopDistance)
        {
            Vector2 direction = (targetPosition - transform.position).normalized;
            rb.velocity = direction * speed;
        }
        else
        {
            rb.velocity = Vector2.zero;
            transform.position = targetPosition;
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = transform.position.z;
        return mainCamera.ScreenToWorldPoint(mousePosition);
    }
}
