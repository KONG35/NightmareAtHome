using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterAttackType
{
    Melee,
    Range,
}
public enum MonsterState
{
    None=0,
    Chase,
    Attack,
    Hit,
    Dead
}
public interface IBaseMonster
{
    void Chase();
    void Attack();
    void Hit(float dmg);
    void Dead();
}

public class baseMonster : MonoBehaviour, IBaseMonster, IPoolable
{
    public string MonsterID;
    public string Name;
    public MonsterAttackType AttackType;
    public float MaxHp;
    public float CurHp;
    public float AttackDamage;
    public float Speed;
    public float Frequency;
    public bool isDie;

    public Transform target;

    protected baseMonster(string _monsterID, string _name, MonsterAttackType _attackType, float _maxHp, float _attackDmg, float _speed, float _frequency)
    {
        this.MonsterID = _monsterID;
        this.Name = _name;
        this.AttackType = _attackType;
        this.MaxHp = _maxHp;
        this.CurHp = _maxHp;
        this.AttackDamage = _attackDmg;
        this.Speed = _speed;
        this.Frequency = _frequency;
        this.isDie = false;
        this.target = null;
    }

    // 오브젝트가 풀에서 꺼내질 때 호출됨
    public virtual void OnSpawn()
    {
        // !수정하기
        if(target==null)
            target = FindObjectOfType<PlayerCharacter>().transform;

        gameObject.SetActive(true);
    }
    // 오브젝트가 풀로 반환될 때 호출됨
    public virtual void OnDespawn()
    {
        gameObject.SetActive(false);
    }
    public virtual void Update()
    {
        if (target == null)
            return;

        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * Speed);

        if (CurHp <= 0)
        {
            Dead();
        }

    }
    public virtual void Chase()
    {
    }

    public virtual void Attack()
    {
    }

    public virtual void Hit(float dmg)
    {
    }
    public virtual void Dead()
    {
    }
}
