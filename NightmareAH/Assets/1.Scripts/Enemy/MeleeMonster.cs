using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMonster: baseMonster
{
    public MeleeMonster(string _monsterID, string _name, MonsterAttackType _attackType, float _maxHp, float _attackDmg, float _speed, float _frequency) : base(_monsterID, _name, _attackType, _maxHp, _attackDmg, _speed, _frequency)
    {
    }

    public void Init(MeleeMonster m)
    {
        this.MonsterID = m.MonsterID;
        this.Name = m.name;
        this.AttackType = m.AttackType;
        this.MaxHp = m.MaxHp;
        this.CurHp = m.MaxHp;
        this.AttackDamage = m.AttackDamage;
        this.Speed = m.Speed;
        this.Frequency = m.Frequency;
        this.isDie = false;
    }
}
