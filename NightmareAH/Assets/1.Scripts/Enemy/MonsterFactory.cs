using System.Collections.Generic;
using UnityEngine;

public class MonsterFactory : MonoBehaviour
{
    // Inspector에 여러 종류의 Melee 및 Ranged 몬스터 prefab을 할당합니다.
    public List<MeleeMonster> meleePrefabs;  // 예: M01MeleePrefab, M02MeleePrefab 등
    public List<RangedMonster> rangedPrefabs; // 예: R01RangedPrefab 등
    public int initialPoolSize = 50;

    // 그룹별 풀: 하나의 Melee 풀, 하나의 Ranged 풀로 관리
    private ObjectPool<MeleeMonster> meleePool;
    private ObjectPool<RangedMonster> rangedPool;

    public void Init()
    {
        // 풀 생성 시 기본 prefab은 아무거나 사용합니다.
        if (meleePrefabs.Count > 0)
        {
            meleePool = new ObjectPool<MeleeMonster>(meleePrefabs[0], 0, transform);
            // 각 Melee prefab에 대해 initialPoolSize 만큼 생성하여 풀에 추가
            foreach (MeleeMonster prefab in meleePrefabs)
            {
                for (int i = 0; i < initialPoolSize; i++)
                {
                    MeleeMonster monster = Instantiate(prefab, transform);
                    monster.OnDespawn();
                    meleePool.AddObject(monster);
                }
            }
        }

        if (rangedPrefabs.Count > 0)
        {
            rangedPool = new ObjectPool<RangedMonster>(rangedPrefabs[0], 0, transform);
            foreach (RangedMonster prefab in rangedPrefabs)
            {
                for (int i = 0; i < initialPoolSize; i++)
                {
                    RangedMonster monster = Instantiate(prefab, transform);
                    monster.OnDespawn();
                    rangedPool.AddObject(monster);
                }
            }
        }
    }

    // Spawn 데이터의 id를 활용하여, 해당 그룹에서 MonsterID가 일치하는 객체를 반환합니다.
    public void CreateMonster(Spawn spawnData)
    {
        if (spawnData.id.StartsWith("m")) // Melee 몬스터
        {
            for (int i = 0; i < spawnData.count; i++)
            {
                // 풀에서 MonsterID가 spawnData.id와 일치하는 객체를 찾습니다.
                var monster = meleePool.GetObject(m => m.MonsterID == spawnData.id);
                PositionMonster(monster.transform, spawnData.innerRadius, spawnData.outerRadius);
            }
        }
        else if (spawnData.id.StartsWith("r")) // Ranged 몬스터
        {
            for (int i = 0; i < spawnData.count; i++)
            {
                var monster = rangedPool.GetObject(r => r.MonsterID == spawnData.id);
                PositionMonster(monster.transform, spawnData.innerRadius, spawnData.outerRadius);
            }
        }
        else
        {
            Debug.LogWarning("Unknown spawn id: " + spawnData.id);
        }
    }

    // 도넛 모양 영역(내부 반경 ~ 외부 반경) 내에 위치를 랜덤하게 지정하는 함수
    private void PositionMonster(Transform t, float innerRadius, float outerRadius)
    {
        float angle = Random.Range(0f, 2f * Mathf.PI);
        float distance = Random.Range(innerRadius, outerRadius);
        t.position = new Vector3(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance, 0f);
    }

    // 사용 후 몬스터를 반환할 때 (필요하다면 사용)
    public void ReturnMonster(baseMonster monster)
    {
        if (monster is MeleeMonster m)
            meleePool.ReturnObject(m);
        else if (monster is RangedMonster r)
            rangedPool.ReturnObject(r);
    }
}
