using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R01RangedMonster : RangedMonster
{
    public R01RangedMonster(string _monsterID, string _name, MonsterAttackType _attackType, float _maxHp, float _attackDmg, float _speed, float _frequency, int _pierceCount) : base(_monsterID, _name, _attackType, _maxHp, _attackDmg, _speed, _frequency, _pierceCount)
    {
    }
    //public R01RangedMonster(RangedMonster m) : base(m.MonsterID, m.Name, m.AttackType, m.MaxHp, m.AttackDamage, m.Speed, m.Frequency, m.PierceCount)
    //{
    //}


    // Update is called once per frame
    override public void Update()
    {
        base.Update();

    }
}
