using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float MaxHp;
    public float CurHp;
    public float MoveSpeed;
    public Transform target;
    bool isDead;
    public void Awake()
    {
        MaxHp = 100;
        CurHp = MaxHp;
        MoveSpeed = 1f;
        isDead = false;
    }
    public void OnEnable()
    {
        target = FindObjectOfType<PlayerCharacter>().transform;
    }

    public void Update()
    {
        if (target == null)
            return;
        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * MoveSpeed);
    }

    public void HitDamage(float Damage)
    {
        CurHp -= Damage;
        if (CurHp <= 0)
        {
            isDead = true;
            FindObjectOfType<TestSpawner>().EnemyList.Remove(this);
            Destroy(this.gameObject);
        }
    }
}
