using System.Collections.Generic;
using UnityEngine;

public class MonsterFactory : MonoBehaviour
{
    // Inspector�� ���� ������ Melee �� Ranged ���� prefab�� �Ҵ��մϴ�.
    public MonsterEntity[] monsterPrefabs;
    public int initialPoolSize = 50;

    // �׷캰 Ǯ: �ϳ��� Melee Ǯ, �ϳ��� Ranged Ǯ�� ����
    private ObjectPool<MonsterEntity> monsterPool;

    public void Init()
    {
        // ���� ������ �ʱ�ȭ 
        var meleeData = GoogleSheetLoader.Instance.GetDataList<MeleeMonsterSheetData>();
        var rangeData = GoogleSheetLoader.Instance.GetDataList<RangedMonsterSheetData>();

        
        for (int i = 0; i < monsterPrefabs.Length; i++)
        {
            switch (monsterPrefabs[i].monster.AttackType)
            {
                case MonsterAttackType.Melee:
                    {
                        var m = meleeData.Find(x => x.monster.MonsterID == monsterPrefabs[i].monster.MonsterID);

                        if (m != null)
                        {
                            monsterPrefabs[i].monster = new MeleeMonster(m.monster);
                        }
                    }
                    break;
                case MonsterAttackType.Range:
                    {
                        var m = rangeData.Find(x => x.monster.MonsterID == monsterPrefabs[i].monster.MonsterID);

                        if (m != null)
                        {
                            monsterPrefabs[i].monster = new RangedMonster(m.monster);
                        }
                    }
                    break;
            }
           
        }
        // Ǯ ���� �� �⺻ prefab�� �ƹ��ų� ����մϴ�.
        if (monsterPrefabs.Length > 0)
        {
            monsterPool = new ObjectPool<MonsterEntity>(monsterPrefabs[0], 0, transform);
            // �� Melee prefab�� ���� initialPoolSize ��ŭ �����Ͽ� Ǯ�� �߰�
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

    // Spawn �������� id�� Ȱ���Ͽ�, �ش� �׷쿡�� MonsterID�� ��ġ�ϴ� ��ü�� ��ȯ�մϴ�.
    public void CreateMonster(Spawn spawnData)
    {
        for (int i = 0; i < spawnData.count; i++)
        {
            // Ǯ���� MonsterID�� spawnData.id�� ��ġ�ϴ� ��ü�� ã���ϴ�.
            var monster = monsterPool.GetObject(m => m.monster.MonsterID == spawnData.id);
            PositionMonster(monster.transform, spawnData.innerRadius, spawnData.outerRadius);
        }
    }

    // ���� ��� ����(���� �ݰ� ~ �ܺ� �ݰ�) ���� ��ġ�� �����ϰ� �����ϴ� �Լ�
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
