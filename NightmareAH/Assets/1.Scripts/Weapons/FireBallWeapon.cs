using Unity.VisualScripting;
using UnityEngine;

public class FireBallWeapon : ProjectlieWeapon
{
    public ProjectlieObject ProjectlieObj;
    public PlayerCharacter my;
    float WaitTime;

    public FireBallWeapon(ProjectlieObject obj,PlayerCharacter my, float MaxRange, float MinRange, float speed, int pierceCount, float piercePer, float Damage, float frequency) : base(MaxRange, MinRange, speed, pierceCount, piercePer, Damage, frequency)
    {
        ProjectlieObj = obj;
        this.my = my;
    }

    public override void Action()
    {
        WaitTime += Time.deltaTime;
        if (Frequency < WaitTime)
        {
            var Target = my.FindClosestEnemy(MinRange);

            if (Target == null)
            {
                WaitTime = Frequency;
                return;
            }
            ProjectlieObject bullet = GameObject.Instantiate(ProjectlieObj, my.transform.position, Quaternion.identity).GetComponent<ProjectlieObject>();

            //var pos = (my.transform.position - Target.transform.position).normalized * MaxRange + my.transform.position;
            var pos = (Target.transform.position - my.transform.position).normalized * MaxRange + Target.transform.position;
            Debug.Log($" Ÿ�� ������  : {Target.transform.position}   �ƽ� ������  {(my.transform.position - Target.transform.position).normalized * MaxRange + my.transform.position}  �߰���� {(my.transform.position - Target.transform.position).normalized * MaxRange}");
            Debug.Log($" Ÿ�� ������  : {Target.transform.position}   �ƽ� ������  {(Target.transform.position - my.transform.position).normalized * MaxRange + Target.transform.position}  �߰���� {(my.transform.position - Target.transform.position).normalized * MaxRange}");
            bullet.Init(pos, 3f,1);
            bullet.HitAction = (other) =>
            {
                Debug.Log($"{other.gameObject.name} ��Ʈ");
                if (other.gameObject.CompareTag("Enemy"))
                {
                    other.gameObject.GetComponent<Enemy>().HitDamage(Damage);
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
