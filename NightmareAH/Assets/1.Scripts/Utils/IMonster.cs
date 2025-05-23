public interface IBaseMonster
{
    void ChaseUpdate();
    void AttackInit();
    void KnockInit();
    //void Hit(float dmg);

    void Hit(AttributeDefSO TargetAttribute, float dmg);
    void DeadInit();
}