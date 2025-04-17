using System;
using UnityEngine;

public class ProjectlieObject : MonoBehaviour ,IPoolable
{
    public Action<Collider> HitAction;
    public Action MaxRangeAction;
    public Vector3 TargetPos;
    public float Speed;
    public int PierceCount;
    public SpriteRenderer sr;
    public bool DirectionImage;
    public bool DirectionXFlip;
    public float DirectionRot = 45f;

    public void Init(Vector3 TargetPos,float speed,int pierceCount)
    {
        TargetPos.z= transform.position.z;
        this.TargetPos = TargetPos;
        this.Speed = speed;
        this.PierceCount = pierceCount;
        if (DirectionImage)
        {
            if (transform.position.x < TargetPos.x)
            {
                sr.flipX = DirectionXFlip ? true : false;
                sr.transform.rotation = DirectionXFlip ? Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + -DirectionRot) : Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + DirectionRot);

                float angle = Vector3.SignedAngle(DirectionXFlip ? Vector3.right : Vector3.left, TargetPos - transform.position, Vector3.back);
                transform.rotation = Quaternion.Euler(0, 0, -angle);
            }
            if (transform.position.x > TargetPos.x)
            {
                sr.flipX = DirectionXFlip ? false : true;
                sr.transform.rotation = DirectionXFlip ? Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + DirectionRot) : Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + -DirectionRot);

                float angle = Vector3.SignedAngle(DirectionXFlip ? Vector3.left : Vector3.right, TargetPos - transform.position, Vector3.back);
                transform.rotation = Quaternion.Euler(0, 0, -angle);
            }
        }
    }
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, TargetPos, Time.deltaTime * Speed);
        
        if (Vector3.Distance(transform.position,TargetPos) < 0.01f)
            MaxRangeAction?.Invoke();
    }
    private void OnTriggerEnter(Collider other)
    {
        HitAction?.Invoke(other);
    }

    void IPoolable.OnSpawn()
    {
        gameObject.SetActive(true);
    }

    void IPoolable.OnDespawn()
    {
        gameObject.SetActive(false);
    }
}
