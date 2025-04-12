using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golditem : ItemBase
{
    public override void Action(PlayerCharacter other)
    {
        throw new System.NotImplementedException();
    }

    private void Awake()
    {
        Type = eItemType.Gold;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
