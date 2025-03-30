using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFactory : MonoBehaviour
{
    // Inspector에 각 몬스터 prefab을 할당합니다.
    public M01MeleeMonster M01MeleePrefab;
    public M02MeleeMonster M02MeleePrefab;
    public R01RangedMonster R01RangedPrefab;
    public int initialPoolSize = 50;

    private ObjectPool<M01MeleeMonster> m01MeleePool;
    private ObjectPool<R01RangedMonster> r01RangedPool;

    public void Init()
    {
        m01MeleePool = new ObjectPool<M01MeleeMonster>(M01MeleePrefab, initialPoolSize, transform);
        r01RangedPool = new ObjectPool<R01RangedMonster>(R01RangedPrefab, initialPoolSize, transform);
    }

    // Spawn 데이터의 id에 따라 해당 풀에서 몬스터를 꺼내고, 도넛 모양의 영역 내에 위치시킵니다.
    public void CreateMonster(Spawn spawnData)
    {
        switch (spawnData.id)
        {
            case "m01":
                for (int i = 0; i < spawnData.count; i++)
                {
                    var monster = m01MeleePool.GetObject();
                    PositionMonster(monster.transform, spawnData.innerRadius, spawnData.outerRadius);
                }
                break;
            case "r01":
                for (int i = 0; i < spawnData.count; i++)
                {
                    var monster = r01RangedPool.GetObject();
                    PositionMonster(monster.transform, spawnData.innerRadius, spawnData.outerRadius);
                }
                break;
            default:
                Debug.LogWarning("Unknown spawn id: " + spawnData.id);
                break;
        }
    }

    // 도넛 모양 영역(내부 반경 ~ 외부 반경)에 위치를 랜덤하게 지정하는 함수
    private void PositionMonster(Transform t, float innerRadius, float outerRadius)
    {
        float angle = Random.Range(0f, 2f * Mathf.PI);
        float distance = Random.Range(innerRadius, outerRadius);
        t.position = new Vector3(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance, 0f);
    }

    // 반환 메서드 (사용 후 풀로 돌려보낼 때)
    public void ReturnMonster(baseMonster monster)
    {
        if (monster is M01MeleeMonster m01)
            m01MeleePool.ReturnObject(m01);
        else if (monster is R01RangedMonster r01)
            r01RangedPool.ReturnObject(r01);
    }
}
