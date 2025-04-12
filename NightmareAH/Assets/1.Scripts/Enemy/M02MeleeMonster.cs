using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M02MeleeMonster : MonsterEntity
{
    public MeleeMonster meleeData => (MeleeMonster)this.monster;

    override public void Update()
    {
        base.Update();

    }
}
