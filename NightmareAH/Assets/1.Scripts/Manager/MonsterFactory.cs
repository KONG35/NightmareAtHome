using System.Collections.Generic;
using UnityEngine;

public class MonsterFactory : MonoBehaviour
{
    // Inspector�� ���� ������ Melee �� Ranged ���� prefab�� �Ҵ��մϴ�.
    //public List<MeleeMonster> meleePrefabs;  // ��: M01MeleePrefab, M02MeleePrefab ��
    public MeleeMonster[] meleePrefabs;  // ��: M01MeleePrefab, M02MeleePrefab ��
    public RangedMonster[] rangedPrefabs; // ��: R01RangedPrefab ��
    public int initialPoolSize = 50;

    // �׷캰 Ǯ: �ϳ��� Melee Ǯ, �ϳ��� Ranged Ǯ�� ����
    private ObjectPool<MeleeMonster> meleePool;
    private ObjectPool<RangedMonster> rangedPool;

    public void Init()
    {
        // ���� ������ �ʱ�ȭ 
        var meleeData = GoogleSheetLoader.Instance.GetDataList<MeleeMonsterData>();
        var rangeData = GoogleSheetLoader.Instance.GetDataList<RangedMonsterData>();

        for (int i = 0; i < meleePrefabs.Length; i++)
        {
            var m = meleeData.Find(x => x.monster.MonsterID == meleePrefabs[i].MonsterID);
            if(m != null)
            {
                meleePrefabs[i].Init(m.monster);
            }
        }
        for (int i = 0; i < rangedPrefabs.Length; i++)
        {
            var m = rangeData.Find(x => x.monster.MonsterID == rangedPrefabs[i].MonsterID);
            if (m != null)
            {
                rangedPrefabs[i].Init(m.monster);
            }
        }

        // Ǯ ���� �� �⺻ prefab�� �ƹ��ų� ����մϴ�.
        if (meleePrefabs.Length > 0)
        {
            meleePool = new ObjectPool<MeleeMonster>(meleePrefabs[0], 0, transform);
            // �� Melee prefab�� ���� initialPoolSize ��ŭ �����Ͽ� Ǯ�� �߰�
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

        if (rangedPrefabs.Length > 0)
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

    // Spawn �������� id�� Ȱ���Ͽ�, �ش� �׷쿡�� MonsterID�� ��ġ�ϴ� ��ü�� ��ȯ�մϴ�.
    public void CreateMonster(Spawn spawnData)
    {
        if (spawnData.id.StartsWith("m")) // Melee ����
        {
            for (int i = 0; i < spawnData.count; i++)
            {
                // Ǯ���� MonsterID�� spawnData.id�� ��ġ�ϴ� ��ü�� ã���ϴ�.
                var monster = meleePool.GetObject(m => m.MonsterID == spawnData.id);
                PositionMonster(monster.transform, spawnData.innerRadius, spawnData.outerRadius);
            }
        }
        else if (spawnData.id.StartsWith("r")) // Ranged ����
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

    // ���� ��� ����(���� �ݰ� ~ �ܺ� �ݰ�) ���� ��ġ�� �����ϰ� �����ϴ� �Լ�
    private void PositionMonster(Transform t, float innerRadius, float outerRadius)
    {
        float angle = Random.Range(0f, 2f * Mathf.PI);
        float distance = Random.Range(innerRadius, outerRadius);
        t.position = new Vector3(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance, 0f);
    }

    // ��� �� ���͸� ��ȯ�� �� (�ʿ��ϴٸ� ���)
    public void ReturnMonster(baseMonster monster)
    {
        if (monster is MeleeMonster m)
            meleePool.ReturnObject(m);
        else if (monster is RangedMonster r)
            rangedPool.ReturnObject(r);
    }
}
