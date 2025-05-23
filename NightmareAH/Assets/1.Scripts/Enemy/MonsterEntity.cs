using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using UnityEngine;

public class MonsterEntity : MonoBehaviour, IBaseMonster, IPoolable
{
    public baseMonster monster;
    public Transform target { get; protected set; }
    
    [SerializeField]
    private bool isDead;
    public bool IsDead
    {
        get => isDead;
        private set
        {
            isDead = value;
        }
    }

    [SerializeField]
    private MonsterState curState;
    public MonsterState CurState
    {
        get => curState;
        private set
        {
            curState = value;
        }
    }
    [SerializeField]
    private MonsterState prevState;
    public MonsterState PrevState
    {
        get => prevState;
        private set
        {
            prevState = value;
        }
    }
    [SerializeField]
    private int poolIndx;
    public int PoolIndx
    {
        get => poolIndx;
        private set
        {
            poolIndx = value;
        }
    }
    private MonsterAnimController animCtlr;

    private SpawnManager spawnManager;

    private PlayerCharacter player;
    
    private Rigidbody rigid;
    private BoxCollider col;

    private float DropExpChance = 0.3f;
    private float innerRadius = 10f;
    private float outerRadius = 15f;
    private void Awake()
    {
        IsDead = false;
        curState = MonsterState.None;
        PrevState = MonsterState.None;

        animCtlr = gameObject.GetComponent<MonsterAnimController>();
        rigid = gameObject.GetComponent<Rigidbody>();
        col = gameObject.GetComponent<BoxCollider>();

        if (player == null)
            player = FindObjectOfType<PlayerCharacter>();

        target = player.transform;
        
        spawnManager = SpawnManager.Instance;
    }
    /// <summary>
    /// 스폰 Init문
    /// </summary>
    protected void Init()
    {
        IsDead = false;
        col.enabled = true;
        rigid.isKinematic = false;
        curState = MonsterState.Chase;
    }
    public virtual void Update()
    {
        if (target == null)
            return;
        
        if (IsDead) return;


        if(PrevState != curState)
        {
            PrevState = curState;

            
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
        IsDead = true;
        col.enabled = false;
        rigid.isKinematic = true;
        rigid.velocity = Vector3.zero;
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
            target = player.transform;

        gameObject.transform.position = SetPosition(target.position);
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
        PoolIndx = _idx;
    }

    private Vector3 SetPosition(Vector3 target)
    {
        float angle = UnityEngine.Random.Range(0f, 2f * Mathf.PI);
        float distance = UnityEngine.Random.Range(innerRadius, outerRadius);
        
        return new Vector3(target.x + Mathf.Cos(angle) * distance, target.y + Mathf.Sin(angle) * distance, 0f);
    }
    
}
