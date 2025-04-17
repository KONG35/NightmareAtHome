using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
// Ǯ�� �������̽�: ������Ʈ�� ������ ���� ��ȯ�� �� ȣ��Ǵ� �޼��带 ����
public interface IPoolable
{
    void OnSpawn();
    void OnDespawn();
}
public class MonsterEntity : MonoBehaviour, IBaseMonster, IPoolable
{
    public baseMonster monster;
    public Transform target { get; protected set; }
    public bool isDead { get; protected set; }

    public MonsterState curState { get; private set; }
    public MonsterState prevState { get; private set; }

    private MonsterAnimController animCtlr;
    
    public float DropExpChance = 0.3f;

    private void Awake()
    {
        isDead = false;
        curState = MonsterState.None;
        prevState = MonsterState.None;

        animCtlr = gameObject.GetComponent<MonsterAnimController>();
    }
    /// <summary>
    /// ���� Init��
    /// </summary>
    protected void Init()
    {
        isDead = false;
        curState = MonsterState.Chase;
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
    public virtual void FixedUpdate()
    {
        if (curState != MonsterState.Chase)
            return;

        if (monster.AttackType == MonsterAttackType.Range && IsInAttackRange())
        {
            curState = MonsterState.Attack;
            return;
        }
        else
        {
            curState = MonsterState.Chase;
            transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * monster.Speed);
        }
    }
    public virtual void ChaseUpdate()
    {
    }

    public virtual void AttackInit()
    {
        animCtlr.OnMonsterAnim(curState);
    }
    public virtual void KnockInit()
    {
        animCtlr.OnMonsterAnim(curState);
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
        animCtlr.OnMonsterAnim(curState);
        DropItem();
    }

    /// <summary>
    /// ������Ʈ�� Ǯ���� ������ �� ȣ���
    /// </summary>
    public virtual void OnSpawn()
    {
        Init();

        // !�����ϱ�
        if (target == null)
            target = FindObjectOfType<PlayerCharacter>().transform;

        gameObject.SetActive(true);
    }
    /// <summary>
    /// ������Ʈ�� Ǯ�� ��ȯ�� �� ȣ���
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
    

    private void DropItem()
    {
        if(Random.Range(0f,1f)< DropExpChance)
        {
            SpawnManager.Instance.SpawnExp(this.monster.Exp, this.transform.position);
        }
    }
    public void SetMonsterState(MonsterState state)
    {
        curState = state;
    }
}
