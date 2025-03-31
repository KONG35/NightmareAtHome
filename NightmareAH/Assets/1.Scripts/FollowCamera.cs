using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FollowCamera : MonoBehaviour
{
    public GameObject Target;
    public float MoveSpeed =1.0f;

    public void Update()
    {
        Vector3 target = Target.transform.position;
        Vector3 my = transform.position;
        target.z = my.z;
        transform.position = Vector3.MoveTowards(my, target, Time.deltaTime * MoveSpeed);
    }
}
