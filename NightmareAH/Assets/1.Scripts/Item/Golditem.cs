using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golditem : ItemBase
{
    float addGold;
    public override void Action(PlayerCharacter other)
    {
        other.CurGold += addGold;
    }

    public override void SetValue(float v)
    {
        addGold = v;
    }

    private void Awake()
    {
        Type = eItemType.Gold;
    }

}
