using System;
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
    Knock,
    Dead
}

[Serializable]
public class baseMonster
{
    public string MonsterID;
    public string Name;
    public MonsterAttackType AttackType;
    public float MaxHp;
    public float CurHp;
    public float AttackDamage;
    public float Speed;
    public float Frequency;
    public float AttackRange;
    public int Exp;

    protected baseMonster(string _monsterID, string _name, MonsterAttackType _attackType, float _maxHp, float _attackDmg, float _speed, float _frequency, float _attackRange, int _exp)
    {
        this.MonsterID = _monsterID;
        this.Name = _name;
        this.AttackType = _attackType;
        this.MaxHp = _maxHp;
        this.CurHp = _maxHp;
        this.AttackDamage = _attackDmg;
        this.Speed = _speed;
        this.Frequency = _frequency;
        this.AttackRange = _attackRange;
        this.Exp = _exp;
    }
    public baseMonster(baseMonster m)
    {
        this.MonsterID = m.MonsterID;
        this.Name = m.Name;
        this.AttackType = m.AttackType;
        this.MaxHp = m.MaxHp;
        this.CurHp = m.MaxHp;
        this.AttackDamage = m.AttackDamage;
        this.Speed = m.Speed;
        this.Frequency = m.Frequency;
        this.AttackRange = m.AttackRange;
        this.Exp = m.Exp;
    }
}
