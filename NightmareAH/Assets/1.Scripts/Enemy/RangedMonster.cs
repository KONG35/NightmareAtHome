using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedMonster: baseMonster
{
    public int PierceCount;
    [SerializeField] 
    private GameObject bulletPrefab;

    public RangedMonster(string _monsterID, string _name, MonsterAttackType _attackType, float _maxHp, float _attackDmg, float _speed, float _frequency, int _pierceCount) :base(_monsterID, _name, _attackType, _maxHp, _attackDmg, _speed, _frequency)
    {
        PierceCount = _pierceCount;
    }
    public void Init(RangedMonster m)
    {
        this.MonsterID = m.MonsterID;
        this.Name = m.name;
        this.AttackType = m.AttackType;
        this.MaxHp = m.MaxHp;
        this.CurHp = m.MaxHp;
        this.AttackDamage = m.AttackDamage;
        this.Speed = m.Speed;
        this.Frequency = m.Frequency;
        this.PierceCount = m.PierceCount;
        this.isDie = false;
    }
}
