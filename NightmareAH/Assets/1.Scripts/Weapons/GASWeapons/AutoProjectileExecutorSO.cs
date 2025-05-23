using UnityEngine;

[CreateAssetMenu(menuName = "GAS/Executor/AutoProjectile")]
public class AutoProjectileExecutorSO : AbilityExecutorSO
{
    public ProjectlieObject ProjectlieObj;
    ObjectPool<ProjectlieObject> ObjPool;
    public LayerMask Enemy;
    float WaitTime;
    public override void Execute(AbilityContext context)
    {
        WaitTime += Time.deltaTime;
        var State = context.Definition.LevelState[context.AbilityLevel];
        if (WaitTime < State.Cooldown)
            return;
        if(ObjPool == null)
            ObjPool = new ObjectPool<ProjectlieObject>(ProjectlieObj, 10);

        var Target = FindClosestEnemy(context.Caster.transform, State.MaxRange);

        if (Target == null)
        {
            WaitTime = State.Cooldown;
            return;
        }
        ProjectlieObject bullet = ObjPool.GetObject();
        var pos = (Target.transform.position - context.Caster.transform.position).normalized * State.MaxRange + Target.transform.position;
        bullet.transform.position = context.Caster.transform.position;
        bullet.Init(pos, 3f, 1);
        float damage = context.Attributes.GetValue(context.Definition.DamageAttribute) * State.CharactorDamageMultiplier + State.Damage;
        bullet.HitAction = (other) =>
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.GetComponent<MonsterEntity>().Hit(context.Definition.DamagetargetAttribute,damage);
                bullet.PierceCount--;
                if (bullet.PierceCount < 0)
                {
                    ObjPool.ReturnObject(bullet);
                }
            }
        };
        WaitTime -= State.Cooldown;
    }
    public GameObject FindClosestEnemy(Transform Caster, float radius)
    {
        Collider[] hits = Physics.OverlapSphere(Caster.localPosition, radius, Enemy);

        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider col in hits)
        {
            float distance = Vector3.SqrMagnitude(Caster.position - col.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = col.gameObject;
            }
        }
        return closest;
    }

}
