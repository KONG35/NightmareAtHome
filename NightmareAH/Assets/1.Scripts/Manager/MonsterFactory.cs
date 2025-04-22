using System.Collections.Generic;
using UnityEngine;

public class MonsterFactory : MonoBehaviour
{
    // Inspector�� ���� prefab�� �Ҵ�
    [SerializeField]
    private MonsterEntity[] monsterPrefabs;
    private int initialPoolSize = 50;

    private ObjectPool<MonsterEntity> monsterPool;

    public void Init()
    {
        // ���� ������ �ʱ�ȭ 
        var monsterData = GoogleSheetLoader.Instance.GetDataList<MonsterSheetData>();

        for (int i = 0; i < monsterPrefabs.Length; i++)
        {
            var m = monsterData.Find(x => x.monster.MonsterID == monsterPrefabs[i].monster.MonsterID);
            if (m.monster is MeleeMonster melee)
            {
                monsterPrefabs[i].monster = new MeleeMonster(melee); // ���� ������
            }
            else if (m.monster is RangedMonster range)
            {
                monsterPrefabs[i].monster = new RangedMonster(range); // ���� ������
            }
            
        }
        // Ǯ ���� �� �⺻ prefab�� �ƹ��ų� ����մϴ�.
        if (monsterPrefabs.Length > 0)
        {
            monsterPool = new ObjectPool<MonsterEntity>(monsterPrefabs[0], 0, transform);
            // �� prefab�� ���� initialPoolSize ��ŭ �����Ͽ� Ǯ�� �߰�
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
    /// Spawn �������� id�� Ȱ���Ͽ�, �ش� �׷쿡�� MonsterID�� ��ġ�ϴ� ��ü�� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="spawnData"></param>
    public void CreateMonster(Spawn spawnData)
    {
        for (int i = 0; i < spawnData.count; i++)
        {
            // Ǯ���� MonsterID�� spawnData.id�� ��ġ�ϴ� ��ü�� ã���ϴ�.
            var monster = monsterPool.GetObject(m => m.monster.MonsterID == spawnData.id);
            PositionMonster(monster.transform, spawnData.innerRadius, spawnData.outerRadius);
        }
    }

    /// <summary>
    /// ���� ��� ����(���� �ݰ� ~ �ܺ� �ݰ�) ���� ��ġ�� �����ϰ� �����ϴ� �Լ�
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

    // ��� �� ���͸� ��ȯ�� �� (�ʿ��ϴٸ� ���)
    public void ReturnMonster(MonsterEntity monster)
    {
        monsterPool.ReturnObject(monster);
    }
}
