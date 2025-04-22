using System.Collections.Generic;
using UnityEngine;

public class MonsterFactory : MonoBehaviour
{
    // Inspector에 몬스터 prefab을 할당
    [SerializeField]
    private MonsterEntity[] monsterPrefabs;
    private int initialPoolSize = 50;

    private ObjectPool<MonsterEntity> monsterPool;

    public void Init()
    {
        // 몬스터 데이터 초기화 
        var monsterData = GoogleSheetLoader.Instance.GetDataList<MonsterSheetData>();

        for (int i = 0; i < monsterPrefabs.Length; i++)
        {
            var m = monsterData.Find(x => x.monster.MonsterID == monsterPrefabs[i].monster.MonsterID);
            if (m.monster is MeleeMonster melee)
            {
                monsterPrefabs[i].monster = new MeleeMonster(melee); // 복사 생성자
            }
            else if (m.monster is RangedMonster range)
            {
                monsterPrefabs[i].monster = new RangedMonster(range); // 복사 생성자
            }
            
        }
        // 풀 생성 시 기본 prefab은 아무거나 사용합니다.
        if (monsterPrefabs.Length > 0)
        {
            monsterPool = new ObjectPool<MonsterEntity>(monsterPrefabs[0], 0, transform);
            // 각 prefab에 대해 initialPoolSize 만큼 생성하여 풀에 추가
            foreach (MonsterEntity prefab in monsterPrefabs)
            {
                for (int i = 0; i < initialPoolSize; i++)
                {
                    MonsterEntity monster = Instantiate(prefab, transform);
                    monster.OnDespawn();
                    monsterPool.AddObject(monster);
                }
            }
        }

    }

    /// <summary>
    /// Spawn 데이터의 id를 활용하여, 해당 그룹에서 MonsterID가 일치하는 객체를 반환하는 함수
    /// </summary>
    /// <param name="spawnData"></param>
    public void CreateMonster(Spawn spawnData)
    {
        for (int i = 0; i < spawnData.count; i++)
        {
            // 풀에서 MonsterID가 spawnData.id와 일치하는 객체를 찾습니다.
            var monster = monsterPool.GetObject(m => m.monster.MonsterID == spawnData.id);
            PositionMonster(monster.transform, spawnData.innerRadius, spawnData.outerRadius);
        }
    }

    /// <summary>
    /// 도넛 모양 영역(내부 반경 ~ 외부 반경) 내에 위치를 랜덤하게 지정하는 함수
    /// </summary>
    /// <param name="t"></param>
    /// <param name="innerRadius"></param>
    /// <param name="outerRadius"></param>
    private void PositionMonster(Transform t, float innerRadius, float outerRadius)
    {
        float angle = Random.Range(0f, 2f * Mathf.PI);
        float distance = Random.Range(innerRadius, outerRadius);
        t.position = new Vector3(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance, 0f);
    }

    // 사용 후 몬스터를 반환할 때 (필요하다면 사용)
    public void ReturnMonster(MonsterEntity monster)
    {
        monsterPool.ReturnObject(monster);
    }
}
