using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseMonster
{
    void ChaseUpdate();
    void AttackInit();
    void KnockInit();
    void Hit(float dmg);
    void DeadInit();
}