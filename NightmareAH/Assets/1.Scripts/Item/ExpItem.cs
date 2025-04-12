using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpItem : ItemBase
{
    float ExpValue = 1f;
    public override void Action(PlayerCharacter other)
    {
        other.CurExp += ExpValue;
    }

    private void Awake()
    {
        Type = eItemType.Exp;
    }
    public abstract void SetValue(float v)
    {
        ExpValue = v;
    }

}
