using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R01RangedMonster : MonsterEntity
{
    public RangedMonster rangeData => (RangedMonster)this.monster;

    // Update is called once per frame
    override public void Update()
    {
        base.Update();

    }
}
