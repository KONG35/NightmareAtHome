using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using UnityEngine;

public class MonsterEntity : MonoBehaviour, IBaseMonster, IPoolable
{
    public baseMonster monster;
    public Transform target { get; protected set; }
    public bool isDead { get; protected set; }

    public MonsterState curState { get; private set; }
    public MonsterState prevState { get; private set; }

    private MonsterAnimController animCtlr;

    private SpawnManager spawnManager;

    private float DropExpChance = 0.3f;
    public int poolIndx { get; private set; }

    private float innerRadius = 10f;
    private float outerRadius = 15f;
    private void Awake()
    {
        isDead = false;
        curState = MonsterState.None;
        prevState = MonsterState.None;

        animCtlr = gameObject.GetComponent<MonsterAnimController>();

        spawnManager = SpawnManager.Instance;
    }
    /// <summary>
    /// 스폰 Init문
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
        monster.CurHp -= dmg;
        if (monster.CurHp <= 0)
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
    /// 오브젝트가 풀에서 꺼내질 때 호출됨
    /// </summary>
    void IPoolable.OnSpawn()
    {
        Init();

        // !수정하기
        if (target == null)
            target = FindObjectOfType<PlayerCharacter>().transform;

        gameObject.transform.position = SetPosition(target.transform.position);
        gameObject.SetActive(true);
    }
    /// <summary>
    /// 오브젝트가 풀로 반환될 때 호출됨
    /// </summary>
    void IPoolable.OnDespawn()
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
            spawnManager.SpawnExp(this.monster.Exp, this.transform.position);
        }
    }
    public void SetMonsterState(MonsterState state)
    {
        curState = state;
    }
    public void OnReturn()
    {
        spawnManager.DespawnMonster(this);
    }
    public void SetPoolIndex(int _idx)
    {
        poolIndx = _idx;
    }

    private Vector3 SetPosition(Vector3 target)
    {
        float angle = UnityEngine.Random.Range(0f, 2f * Mathf.PI);
        float distance = UnityEngine.Random.Range(innerRadius, outerRadius);
        
        return new Vector3(target.x + Mathf.Cos(angle) * distance, target.y + Mathf.Sin(angle) * distance, 0f);
    }
}
