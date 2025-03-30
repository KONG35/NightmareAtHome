using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M02MeleeMonster : MeleeMonster
{
    public M02MeleeMonster(string _monsterID, string _name, MonsterAttackType _attackType, float _maxHp, float _attackDmg, float _speed, float _frequency) : base(_monsterID, _name, _attackType, _maxHp, _attackDmg, _speed, _frequency)
    {
    }
    override public void Update()
    {
        base.Update();

    }
}
