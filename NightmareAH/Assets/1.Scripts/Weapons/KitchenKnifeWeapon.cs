using UnityEngine;

public class KitchenKnifeWeapon : ProjectlieWeapon
{
    public ProjectlieObject ProjectlieObj;
    float WaitTime;

    public KitchenKnifeWeapon(ProjectlieObject obj, float MaxRange, float MinRange, float speed, int pierceCount, float piercePer, float Damage, float frequency,int MaxLv, DataTableManager.WeaponIconData icon) : base(MaxRange, MinRange, speed, pierceCount, piercePer, Damage, frequency,MaxLv,icon)
    {
        ProjectlieObj = obj;
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
            ProjectlieObject bullet = GameObject.Instantiate(ProjectlieObj, my.transform.position, Quaternion.identity).GetComponent<ProjectlieObject>();

            var pos = (Target.transform.position - my.transform.position).normalized * MaxRange + Target.transform.position;
            bullet.Init(pos, 3f,1);
            bullet.HitAction = (other) =>
            {
                if (other.gameObject.CompareTag("Enemy"))
                {
                    other.gameObject.GetComponent<MonsterEntity>().Hit(Damage);
                    PierceCount--;
                    if (PierceCount < 0)
                    {
                        GameObject.Destroy(bullet.gameObject);
                    }
                }
            };
            WaitTime -= Frequency;
        }
    }
}
