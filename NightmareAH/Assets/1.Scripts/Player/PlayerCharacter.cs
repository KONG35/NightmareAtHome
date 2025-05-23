using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(GASTagComponent))]
[RequireComponent(typeof(GASAttributeSetComponent))]
[RequireComponent(typeof(GASAbilityComponent))]
[RequireComponent(typeof(GASCueComponent))]
public class PlayerCharacter : Singleton<PlayerCharacter>
{
    public float MaxHp = 100f;
    private float Hp;
    public float CurHp
    {
        get
        {
            return Hp;
        }
        set
        {
            Hp = value;
            if (MaxHp < Hp)
                Hp = MaxHp;
        }
    }
    private int Lv = 0;

    public int curLv
    {
        get
        {
            return Lv;
        }
    }
    public void LvUp()
    {
        {
            Lv++;
            Exp -= ExpLvTable[curLv-1];
            if (ExpLvTable.Length <= Lv)
                return;
            if (ExpLvTable[curLv] <= Exp)
                StartCoroutine(DelayLvUp());
            if (InGameUI.Instance)
                InGameUI.Instance.ExpGaugeUI.SetBar(Exp, ExpLvTable[curLv]);
        }
    }
    public float[] ExpLvTable = { 10,20,30,40,50,60,70,80};
    private float Exp = 0;
    public float CurExp
    {
        get
        {
            return Exp;
        }
        set
        {
            Exp = value;
            if (ExpLvTable.Length <= Lv)
                return;
            if (InGameUI.Instance)
                InGameUI.Instance.ExpGaugeUI.SetBar(Exp, ExpLvTable[curLv]);
            if (ExpLvTable[curLv]<=Exp&& InGameUI.Instance&&!InGameUI.Instance.SkillPickUI.gameObject.activeSelf)
            {
                InGameUI.Instance.SkillPickUI.SetItems();
                InGameUI.Instance.SkillPickUI.gameObject.SetActive(true);
            }
        }
    }
    public float CurGold = 0f;


    public List<baseWeapon> Weapons;
    public LayerMask Enemy;
    public ProjectlieObject TestProjectObj;
    public TestSpawner spawner;
    public Rigidbody rigid;
    public Animator anim;
    public SpriteRenderer myRender;


    public List<WeaponData> weaponList;

    bool isDead = false;


    GASTagComponent tagComponent;
    GASAttributeSetComponent _state;
    GASAbilityComponent abilityComponent;
    GASCueComponent cueComponent;


    public void Start()
    {
        if (Weapons == null)
            Weapons = new List<baseWeapon>();

        if (spawner == null)
            spawner = FindObjectOfType<TestSpawner>();
        if (rigid == null)
            rigid = gameObject.GetComponent<Rigidbody>();
        if (anim == null)
            anim = gameObject.GetComponent<Animator>();
        if (myRender == null)
            myRender = gameObject.GetComponent<SpriteRenderer>();


        if (tagComponent == null)
            tagComponent = FindObjectOfType<GASTagComponent>();
        if (_state == null)
            _state = gameObject.GetComponent<GASAttributeSetComponent>();
        if (abilityComponent == null)
            abilityComponent = gameObject.GetComponent<GASAbilityComponent>();
        if (cueComponent == null)
            cueComponent = gameObject.GetComponent<GASCueComponent>();


        CurExp = 0;
    }
    [Button]
    public void KitchenKnifeWeaponAddWeapon()
    {
        if (Weapons == null)
            Weapons = new List<baseWeapon>();
        var weapon = new KitchenKnifeWeapon(TestProjectObj, 8, 5, 1, 0, 1, 40, 0.5f, 5,DataTableManager.Instance.weaponIconList.Find(x => x.name == "Kitchenknife"));
        weapon.EquipWeapon(this);
        Weapons.Add(weapon);
    }
    public int GetWeaponLv(AbilityDefSO weapon)
    {
        return abilityComponent.GetAbilityLv(weapon);
    }
    public void AddWeapon(AbilityDefSO weapon)
    {
        abilityComponent.AddAbility(weapon);
    }
    public void AddWeapon(baseWeapon weapon)
    {

        if (Weapons == null)
            Weapons = new List<baseWeapon>();
        if (Weapons.Exists(x => x == weapon))
        {
            weapon.LvUp();
        }
        else
        {
            weapon.EquipWeapon(this);
            weapon.lv = 1;
            Weapons.Add(weapon);
        }
    }

    public void Update()
    {
        if (isDead)
            return;


        foreach (var weapon in Weapons)
            weapon.Action();

        anim.SetFloat("Speed", rigid.velocity.magnitude);
        if(rigid.velocity.x!=0)
            myRender.flipX = rigid.velocity.x < 0;

    }
    public GameObject FindClosestEnemy(float radius)
    {
        Collider[] hits = Physics.OverlapSphere(transform.localPosition, radius, Enemy);

        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider col in hits)
        {
            float distance = Vector3.SqrMagnitude(transform.position - col.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = col.gameObject;
            }
        }
        return closest;
    }

    public void RunCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    public void Hit(float dmg)
    {
        CurHp -= dmg;
        if (CurHp <= 0)
        {
            anim.SetBool("Dead", true);
            rigid.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
    IEnumerator DelayLvUp()
    {
        while (InGameUI.Instance.SkillPickUI.gameObject.activeSelf)
            yield return new WaitForEndOfFrame();
        if (ExpLvTable[curLv] <= Exp && InGameUI.Instance)
        {
            InGameUI.Instance.SkillPickUI.SetItems();
            InGameUI.Instance.SkillPickUI.gameObject.SetActive(true);
        }
    }
}
