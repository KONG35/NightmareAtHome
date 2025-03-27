using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ProjectlieObject : MonoBehaviour
{
    public Action<Collider> HitAction;
    public Vector3 TargetPos;
    public float Speed;
    public int PierceCount;
    public void Init(Vector3 TargetPos,float speed,int pierceCount)
    {
        this.TargetPos = TargetPos;
        this.Speed = speed;
        this.PierceCount = pierceCount;
    }
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, TargetPos, Time.deltaTime * Speed);

        if (transform.position == TargetPos)
            Destroy(this.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        HitAction?.Invoke(other);
    }
}
