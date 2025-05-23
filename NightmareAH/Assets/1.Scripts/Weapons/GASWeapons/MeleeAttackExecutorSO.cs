using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "GAS/Executor/MeleeAttack")]
public class MeleeAttackExecutorSO : AbilityExecutorSO
{
    public MeleeObject SlashObject;

    public ObjectPool<MeleeObject> SlashList;
    PlayerCharacter Caster;
    float WaitTime;
    public override void Execute(AbilityContext context)
    {
        WaitTime += Time.deltaTime;
        var State = context.Definition.LevelState[context.AbilityLevel];
        if (WaitTime < State.Cooldown)
            return;
        if (SlashList == null)
            SlashList = new ObjectPool<MeleeObject>(SlashObject,5,false,context.Caster.transform);
        if (Caster == null)
            Caster = context.Caster.GetComponent<PlayerCharacter>();
        float damage = context.Attributes.GetValue(context.Definition.DamageAttribute) * State.CharactorDamageMultiplier + State.Damage;
        Caster.RunCoroutine(Slash(context.AbilityLevel+1,State.Scale,damage,context.Definition.DamagetargetAttribute));
        WaitTime -= State.Cooldown;
    }
    IEnumerator Slash(int lv,float Scale,float Damage,AttributeDefSO targetAttribute)
    {
        int LoopCount = 0;
        int front = Caster.myRender.flipX ? -1 : 1;
        while (LoopCount < lv)
        {
            var obj = SlashList.GetObject();
            obj.transform.parent = Caster.transform;
            obj.transform.position = Caster.transform.position;
            obj.transform.localScale = LoopCount % 2 == 0 ? new Vector3(1 * Scale * front, 1 * Scale, 1 * Scale) : new Vector3(-1 * Scale * front, 1 * Scale, 1 * Scale);
            obj.DisableAction = () => { SlashList.ReturnObject(obj); };
            obj.HitAction = (other) => {
                if (other.gameObject.CompareTag("Enemy"))
                {
                    other.gameObject.GetComponent<MonsterEntity>().Hit(targetAttribute,Damage);
                }
            };
            yield return new WaitForSeconds(0.4f);
            LoopCount++;
        }
    }
}