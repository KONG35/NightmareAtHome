using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[Serializable]
public class MonsterAnimClips
{
    public AnimationClip walkClip;
    public AnimationClip attackClip;
    public AnimationClip knockClip;
    public AnimationClip deathClip;
}

public class MonsterAnimController : MonoBehaviour
{
    private MonsterEntity entity;
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private MonsterAnimClips animClips;
    
    private Coroutine aniCor;
    
    private bool isAnimating;
    
    private void Awake()
    {
        entity = gameObject.GetComponent<MonsterEntity>();
        animator = gameObject.GetComponent<Animator>();
        isAnimating = false;
        aniCor = null;

        ApplyAnimationOverride();
    }
    private void OnDisable()
    {
        isAnimating = false;
        aniCor = null;

    }
    private void ApplyAnimationOverride()
    {
        AnimatorOverrideController overrideController = new AnimatorOverrideController(AnimationManager.Instance.monsterAnimCtlr);

        // 상태 이름에 맞춰서 클립 매핑
        var overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        overrideController.GetOverrides(overrides);

        for (int i = 0; i < overrides.Count; i++)
        {
            string stateName = overrides[i].Key.name;
            AnimationClip newClip = GetClipForState(stateName);
            if (newClip != null)
                overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(overrides[i].Key, newClip);
        }

        overrideController.ApplyOverrides(overrides);
        animator.runtimeAnimatorController = overrideController;
    }
    AnimationClip GetClipForState(string stateName)
    {
        return stateName switch
        {
            "Walk" => animClips.walkClip,
            "IsAttack" => animClips.attackClip,
            "IsKnock" => animClips.knockClip,
            "IsDead" => animClips.deathClip,
            _ => null
        };
    }
    public void OnMonsterAnim(MonsterState state)
    {
        switch (state)
        {
            case MonsterState.Attack:
                {
                    // 원거리 공격만 attack 모션 있음
                    StartAnimTrigger("IsAttack", () => EndAction());
                }
                break;
            case MonsterState.Knock:
                {
                    StartAnimTrigger("IsKnock", () => EndAction());
                }
                break;
            case MonsterState.Dead:
                {
                    DeadAction();
                }
                break;
        }
    }
    private void StartAnimTrigger(string stateName,System.Action onEnd)
    {
        isAnimating = true;
        animator.SetTrigger(stateName);

        aniCor = StartCoroutine(WaitForAnimation(stateName, () =>
        {
            onEnd();
            
        }));
    }
    IEnumerator WaitForAnimation(string stateName, System.Action onEnd)
    {
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
        {
            if (entity.isDead) yield break;
            yield return null;
        }

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            if (entity.isDead) yield break;
            yield return null;
        }

        if (!entity.isDead)
            onEnd?.Invoke();
    }
    private void EndAction()
    {
        entity.SetMonsterState(MonsterState.Chase);
        isAnimating = false;
        aniCor = null;
    }
    private void DeadAction()
    {
        if (aniCor != null)
        {
            StopCoroutine(aniCor);
            aniCor = null;
        }
        // 현재 진행하던 애니메이션 끊고 즉시 죽는 애니메이션 진행
        animator.CrossFade("IsDead", 0.05f);
    }
}
