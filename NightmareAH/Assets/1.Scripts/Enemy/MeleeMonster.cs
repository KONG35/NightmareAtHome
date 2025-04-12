using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class MeleeMonster: baseMonster
{
    public MeleeMonster(string _monsterID, string _name, MonsterAttackType _attackType, float _maxHp, float _attackDmg, float _speed, float _frequency, float _attackRange, int _exp) : base(_monsterID, _name, _attackType, _maxHp, _attackDmg, _speed, _frequency, _attackRange, _exp)
    {
    }
    public MeleeMonster(MeleeMonster m) : base(m)
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
