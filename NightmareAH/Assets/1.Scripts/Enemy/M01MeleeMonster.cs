using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class M01MeleeMonster : MonsterEntity
{
    public MeleeMonster meleeData => (MeleeMonster)this.monster;
    override public void Update()
    {
        base.Update();

    }
    override public void AttackInit()
    {
        base.AttackInit();

    }
}
