using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterFactory : MonoBehaviour
{
    // Inspector에 몬스터 prefab을 할당
    [SerializeField]
    private MonsterEntity[] monsterPrefabs;

    private List<SpawnSheetData> spawnList;
    private List<MonsterSheetData> monsterDataList;

    private List<ObjectPool<MonsterEntity>> poolList;
    private int initialPoolSize = 50;

    [Button]
    public void Init()
    {
        poolList = new List<ObjectPool<MonsterEntity>>();
        spawnList = GoogleSheetLoader.Instance.GetDataList<SpawnSheetData>();
        monsterDataList = GoogleSheetLoader.Instance.GetDataList<MonsterSheetData>();

        for (int i = 0; i < monsterDataList.Count; i++)
        {
            poolList.Add(new ObjectPool<MonsterEntity>(null, 0, true));
        }


        for (int i = 0; i < monsterPrefabs.Length; i++)
        {
            var m = monsterDataList.Find(x => x.monster.MonsterID == monsterPrefabs[i].monster.MonsterID);
            if (m.monster is MeleeMonster melee)
            {
                monsterPrefabs[i].monster = new MeleeMonster(melee); // 복사 생성자
            }
            else if (m.monster is RangedMonster range)
            {
                monsterPrefabs[i].monster = new RangedMonster(range); // 복사 생성자
            }

        }

        for (int i = 0; i < monsterPrefabs.Length; i++)
        {
            int index = monsterDataList.FindIndex(x => x.monster.MonsterID == monsterPrefabs[i].monster.MonsterID);

            monsterPrefabs[i].SetPoolIndex(index);
            poolList[index].Setprefab(monsterPrefabs[i], gameObject.transform);
        }

    }
    [Button]
    private void SheetSpawnStartEdit()
    {
        // 구글시트에서 받아온대로 spawn
        StartCoroutine(CreateMonsterCor());

    }
    IEnumerator CreateMonsterCor()
    {
        for (int i = 0; i < spawnList.Count; i++)
        {
            yield return new WaitForSeconds(spawnList[i].spawn.spawnTime);

            CreateMonster(spawnList[i].spawn);
        }
        yield return null;

    }
    /// <summary>
    /// Spawn 데이터의 id를 활용하여, 해당 그룹에서 MonsterID가 일치하는 객체를 반환하는 함수
    /// </summary>
    /// <param name="spawnData"></param>
    public void CreateMonster(Spawn spawnData)
    {
        // 풀에서 MonsterID가 spawnData.id와 일치하는 객체를 찾습니다.
        int idx = monsterDataList.FindIndex(x => x.monster.MonsterID == spawnData.id);
        for (int i = 0; i < spawnData.count; i++)
        {
            var m= poolList[idx].GetObject();
        }
    }

    // 사용 후 몬스터를 반환할 때
    // 반환 후 리스폰
    public void ReturnMonster(MonsterEntity monster)
    {
        poolList[monster.PoolIndx].ReturnObject(monster);
        poolList[monster.PoolIndx].GetObject();

    }
}
