using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

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
    public int Lv = 0;
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
            Exp += value;
            if (InGameUI.Instance)
                InGameUI.Instance.ExpGaugeUI.SetBar(Exp, ExpLvTable[Lv]);
            if (ExpLvTable[Lv]<Exp&& InGameUI.Instance)
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
            Weapons.Add(weapon);
        }
    }

    public void Update()
    {
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
}
