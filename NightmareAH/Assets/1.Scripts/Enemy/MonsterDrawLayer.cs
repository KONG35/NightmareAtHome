using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MonsterDrawLayer : MonoBehaviour
{
    private MonsterEntity monster;
    private SpriteRenderer spriteRenderer;

    private Vector3 prevTargetPos = Vector3.zero;

    [Header("���� ����")]
    [Space(10)]
    [Header("�׸��� ����")]
    [SerializeField]
    private float gridSize = 0.1f;         // �׸��� ����
    
    [Header("���� ���� ����")]
    [SerializeField]
    private int baseOrder = 1000;          // ���� ���� ����
    
    [Header("�׸��� 1ĭ�� sortingOrder ����")]
    [SerializeField]
    private int orderPerLevel = 10;        // �׸��� 1ĭ�� sortingOrder ����

    private Vector3 diableVec = new Vector3(99f, 99f, 99f); // despawn�� �� �δ� ��
    private void Awake()
    {
        monster = gameObject.GetComponent<MonsterEntity>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

    }
    private void OnDisable()
    {
        prevTargetPos = diableVec;
    }
    void LateUpdate()
    {
        if (monster == null)
            return;

        if (monster.target == null)
            return;

        if (monster.isDead) 
            return;

        if (prevTargetPos == diableVec)
        {
            prevTargetPos = monster.target.position;
            return;
        }
        float offsetY = transform.position.y - prevTargetPos.y;
        int level = Mathf.FloorToInt(offsetY / gridSize);

        spriteRenderer.sortingOrder = baseOrder - (level * orderPerLevel);

        float offsetX = transform.position.x - prevTargetPos.x;
        spriteRenderer.flipX = (offsetX >= 0);

        prevTargetPos = monster.target.position;
    }
}
