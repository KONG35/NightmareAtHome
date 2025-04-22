using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MonsterDrawLayer : MonoBehaviour
{
    private MonsterEntity monster;
    private SpriteRenderer spriteRenderer;

    private Vector3 prevTargetPos = Vector3.zero;

    [Header("정렬 설정")]
    [Space(10)]
    [Header("그리드 간격")]
    [SerializeField]
    private float gridSize = 0.1f;         // 그리드 간격
    
    [Header("기준 정렬 순서")]
    [SerializeField]
    private int baseOrder = 1000;          // 기준 정렬 순서
    
    [Header("그리드 1칸당 sortingOrder 차이")]
    [SerializeField]
    private int orderPerLevel = 10;        // 그리드 1칸당 sortingOrder 차이

    private Vector3 diableVec = new Vector3(99f, 99f, 99f); // despawn될 때 두는 곳
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
