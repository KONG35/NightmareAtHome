using UnityEngine;
using UnityEngine.Pool;

public class KitchenKnifeWeapon : ProjectlieWeapon
{
    public ProjectlieObject ProjectlieObj;
    public ObjectPool<ProjectlieObject> ObjPool;
    float WaitTime;

    public KitchenKnifeWeapon(ProjectlieObject obj, float MaxRange, float MinRange, float speed, int pierceCount, float piercePer, float Damage, float frequency,int MaxLv, DataTableManager.WeaponIconData icon) : base(MaxRange, MinRange, speed, pierceCount, piercePer, Damage, frequency,MaxLv,icon)
    {
        ProjectlieObj = obj;
        ObjPool = new ObjectPool<ProjectlieObject>(ProjectlieObj,10, false);
    }

    public override void Action()
    {
        WaitTime += Time.deltaTime;
        if (Frequency < WaitTime)
        {
            var Target = my.FindClosestEnemy(MaxRange);

            if (Target == null)
            {
                WaitTime = Frequency;
                return;
            }
            ProjectlieObject bullet = ObjPool.GetObject();//= GameObject.Instantiate(ProjectlieObj, my.transform.position, Quaternion.identity).GetComponent<ProjectlieObject>();
            var pos = (Target.transform.position - my.transform.position).normalized * MaxRange + Target.transform.position;
            bullet.transform.position = my.transform.position;
            //bullet.transform.parent = my.transform;
            bullet.Init(pos, 3f,1);
            bullet.HitAction = (other) =>
            {
                if (other.gameObject.CompareTag("Enemy"))
                {
                    other.gameObject.GetComponent<MonsterEntity>().Hit(Damage);
                    PierceCount--;
                    if (PierceCount < 0)
                    {
                        //GameObject.Destroy(bullet.gameObject);
                        ObjPool.ReturnObject(bullet);
                    }
                }
            };
            bullet.MaxRangeAction = () =>
            {
                ObjPool.ReturnObject(bullet);
            };
            WaitTime -= Frequency;
        }
    }
}
