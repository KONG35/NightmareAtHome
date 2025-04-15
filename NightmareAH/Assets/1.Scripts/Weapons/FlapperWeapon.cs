using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlapperWeapon : MeleeWeapon
{
    public PlayerCharacter my;
    float WaitTime;
    public FlapperWeapon(PlayerCharacter my,float Range, float Damage, float frequency,float scale,int MaxLv,DataTableManager.WeaponIconData icon) : base(Range,Damage, frequency,scale,MaxLv,icon)
    {
        this.my = my;
    }

    public override void Action()
    {
        WaitTime += Time.deltaTime;

        if (Frequency < WaitTime)
        {
            var Target = my.FindClosestEnemy(Range);

            if (Target == null)
            {
                WaitTime = Frequency;
                return;
            }
            WaitTime -= Frequency;
        }
    }
}
