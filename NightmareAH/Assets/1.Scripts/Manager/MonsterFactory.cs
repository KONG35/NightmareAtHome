using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterFactory : MonoBehaviour
{
    // Inspector�� ���� prefab�� �Ҵ�
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
                monsterPrefabs[i].monster = new MeleeMonster(melee); // ���� ������
            }
            else if (m.monster is RangedMonster range)
            {
                monsterPrefabs[i].monster = new RangedMonster(range); // ���� ������
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
        // ���۽�Ʈ���� �޾ƿ´�� spawn
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
    /// Spawn �������� id�� Ȱ���Ͽ�, �ش� �׷쿡�� MonsterID�� ��ġ�ϴ� ��ü�� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="spawnData"></param>
    public void CreateMonster(Spawn spawnData)
    {
        // Ǯ���� MonsterID�� spawnData.id�� ��ġ�ϴ� ��ü�� ã���ϴ�.
        int idx = monsterDataList.FindIndex(x => x.monster.MonsterID == spawnData.id);
        for (int i = 0; i < spawnData.count; i++)
        {
            var m= poolList[idx].GetObject();
        }
    }

    // ��� �� ���͸� ��ȯ�� ��
    // ��ȯ �� ������
    public void ReturnMonster(MonsterEntity monster)
    {
        poolList[monster.PoolIndx].ReturnObject(monster);
        poolList[monster.PoolIndx].GetObject();

    }
}
