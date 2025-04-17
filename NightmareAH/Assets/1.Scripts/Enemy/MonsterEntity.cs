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
    [SerializeField]
    private Transform target;
    public bool isDead { get; protected set; }

    public MonsterState curState;
    public MonsterState prevState;
    public Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody rigid;

    private Coroutine aniCor;
    private bool isAnimating;

    [Header("���� ����")]
    public float gridSize = 0.1f;         // �׸��� ����
    public int baseOrder = 1000;          // ���� ���� ����
    public int orderPerLevel = 10;        // �׸��� 1ĭ�� sortingOrder ����

    public float DropExpChance = 0.3f;

    private void Awake()
    {
        isDead = false;
        isAnimating = false;
        curState = MonsterState.None;
        prevState = MonsterState.None;
        aniCor = null;

        rigid = gameObject.GetComponent<Rigidbody>();
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    /// <summary>
    /// ���� Init��
    /// </summary>
    protected void Init()
    {
        isDead = false;
        isAnimating = false;
        curState = MonsterState.Chase;
        aniCor = null;
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
    void LateUpdate()
    {
        if (target == null)
            return;

        if (isDead) return;
        float offsetY = transform.position.y - target.position.y;
        int level = Mathf.FloorToInt(offsetY / gridSize);

        spriteRenderer.sortingOrder = baseOrder - (level * orderPerLevel);

        float offsetX = transform.position.x - target.position.x;
        spriteRenderer.flipX = (offsetX >= 0);
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
        monster.CurHp -= dmg;
        if (monster.CurHp <= 0)
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
        DropItem();
        animator.CrossFade("IsDead", 0.05f);
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
    private void StartAnimTrigger(string stateName)
    {
        isAnimating = true;
        animator.SetTrigger(stateName);

        aniCor = StartCoroutine(WaitForAnimation(stateName, () =>
        {
            isAnimating = false;
            curState = MonsterState.Chase;
            aniCor = null;
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

    private void DropItem()
    {
        if(Random.Range(0f,1f)< DropExpChance)
        {
            SpawnManager.Instance.SpawnExp(this.monster.Exp, this.transform.position);
        }
    }
}
