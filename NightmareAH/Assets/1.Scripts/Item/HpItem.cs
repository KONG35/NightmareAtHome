using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpItem : ItemBase
{
    float AddValue;
    public override void Action(PlayerCharacter other)
    {
        other.CurHp += AddValue;
    }

    private void Awake()
    {
        Type = eItemType.Hp;
    }
    public void Init()
    {
        AddValue = 30f;
    }
}
