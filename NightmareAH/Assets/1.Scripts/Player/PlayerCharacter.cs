using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{

    public List<baseWeapon> Weapons;
    public LayerMask Enemy;
    public ProjectlieObject TestProjectObj;
    public TestSpawner spawner;
    public Rigidbody rigid;
    public Animator anim;
    public SpriteRenderer myRender;

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
    public void AddWeapon()
    {
        if (Weapons == null)
            Weapons = new List<baseWeapon>();

        Weapons.Add(new FireBallWeapon(TestProjectObj, this, 8, 5, 1, 0, 1, 40, 0.5f));
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
