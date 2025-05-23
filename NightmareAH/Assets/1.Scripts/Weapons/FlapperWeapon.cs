using System.Collections;
using UnityEngine;

public class FlapperWeapon : MeleeWeapon
{
    public MeleeObject SlashObject;

    public ObjectPool<MeleeObject> SlashList;

    float WaitTime;
    public FlapperWeapon(MeleeObject obj ,float Range, float Damage, float frequency,float scale,int MaxLv,DataTableManager.WeaponIconData icon) : base(Range,Damage, frequency,scale,MaxLv,icon)
    {
        SlashObject = obj;
        SlashList= new ObjectPool<MeleeObject>(SlashObject,MaxLv, false);
    }

    public override void Action()
    {
        WaitTime += Time.deltaTime;

        if (Frequency < WaitTime)
        {
            my.RunCoroutine(Slash());
            WaitTime -= Frequency;
        }
    }

    IEnumerator Slash()
    {
        int LoopCount = 0;
        int front = my.myRender.flipX ? -1 : 1;
        while(LoopCount < lv)
        {
            var obj = SlashList.GetObject();
            obj.transform.parent = my.transform;
            obj.transform.position = my.transform.position;
            obj.transform.localScale = LoopCount % 2 == 0 ? new Vector3(1*Scale* front, 1 * Scale, 1 * Scale) : new Vector3(-1 * Scale* front, 1 * Scale, 1 * Scale);
            obj.DisableAction = () => { SlashList.ReturnObject(obj); };
            obj.HitAction = (other) => {
                if (other.gameObject.CompareTag("Enemy"))
                {
                    //other.gameObject.GetComponent<MonsterEntity>().Hit(Damage);
                }
            };
            yield return new WaitForSeconds(0.4f);
            LoopCount++;
        }
    }
}
