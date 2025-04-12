using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public interface IBaseMonster
{
    void ChaseUpdate();
    void AttackInit();
    void KnockInit();
    void Hit(float dmg);
    void DeadInit();
}
// 풀링 인터페이스: 오브젝트가 스폰될 때와 반환될 때 호출되는 메서드를 정의
public interface IPoolable
{
    void OnSpawn();
    void OnDespawn();
}
public class MonsterEntity : MonoBehaviour, IBaseMonster, IPoolable
{
    public baseMonster monster;
    [SerializeField]
    private Transform target;
    public bool isDead { get; protected set; }

    public MonsterState curState;
    public MonsterState prevState;
    public Animator animator;

    private Coroutine aniCor;
    private bool isAnimating;
    private void Awake()
    {
        isDead = false;
        isAnimating = false;
        curState = MonsterState.None;
        prevState = MonsterState.None;
        aniCor = null;
        animator = gameObject.GetComponent<Animator>();
    }
    public virtual void Update()
    {
        if (target == null)
            return;
        
        if (isDead) return;


        if(prevState != curState)
        {
            prevState = curState;

            switch (curState)
            {
                case MonsterState.Attack:
                    {
                        AttackInit();
                    }
                    break;
                case MonsterState.Knock:
                    {
                        KnockInit();
                    }
                    break;
                case MonsterState.Dead:
                    {
                        DeadInit();
                    }
                    break;
            }
        }
        else
        {
            switch (curState)
            {
                case MonsterState.Chase:
                    {
                        ChaseUpdate();
                    }
                    break;
            }
        }

    }

    public virtual void ChaseUpdate()
    {
        if (IsInAttackRange())
        {
            curState = MonsterState.Attack;
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * monster.Speed);
    }

    public virtual void AttackInit()
    {
        StartAnimTrigger("IsAttack");
    }
    public virtual void KnockInit()
    {
        StartAnimTrigger("IsKnocked");
    }

    public virtual void Hit(float dmg)
    {
        monster.MaxHp -= dmg;
        if (monster.MaxHp <= 0)
        {
            curState = MonsterState.Dead;
        }
    }
    public virtual void DeadInit()
    {
        isDead = true;
        if(aniCor != null)
        {
            StopCoroutine(aniCor);
            aniCor = null;
        }
        animator.CrossFade("IsDead", 0.05f);
    }

    /// <summary>
    /// 오브젝트가 풀에서 꺼내질 때 호출됨
    /// </summary>
    public virtual void OnSpawn()
    {
        isDead = false;
        curState = MonsterState.Chase;

        // !수정하기
        if (target == null)
            target = FindObjectOfType<PlayerCharacter>().transform;

        gameObject.SetActive(true);
    }
    /// <summary>
    /// 오브젝트가 풀로 반환될 때 호출됨
    /// </summary>
    public virtual void OnDespawn()
    {
        gameObject.SetActive(false);
    }

    private bool IsInAttackRange()
    {
        if (Vector2.Distance(target.position, transform.position) <= monster.AttackRange)
            return true;
        else
            return false;
    }
    private void StartAnimTrigger(string stateName)
    {
        isAnimating = true;
        animator.SetTrigger(stateName);

        aniCor = StartCoroutine(WaitForAnimation(stateName, () =>
        {
            isAnimating = false;
            aniCor = null;
            curState = MonsterState.Chase;
        }));
    }
    IEnumerator WaitForAnimation(string stateName, System.Action onEnd)
    {
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
        {
            if (isDead) yield break;
            yield return null;
        }

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            if (isDead) yield break;
            yield return null;
        }

        if (!isDead)
            onEnd?.Invoke();
    }
}
