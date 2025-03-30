using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFactory : MonoBehaviour
{
    // Inspector�� �� ���� prefab�� �Ҵ��մϴ�.
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

    // Spawn �������� id�� ���� �ش� Ǯ���� ���͸� ������, ���� ����� ���� ���� ��ġ��ŵ�ϴ�.
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

    // ���� ��� ����(���� �ݰ� ~ �ܺ� �ݰ�)�� ��ġ�� �����ϰ� �����ϴ� �Լ�
    private void PositionMonster(Transform t, float innerRadius, float outerRadius)
    {
        float angle = Random.Range(0f, 2f * Mathf.PI);
        float distance = Random.Range(innerRadius, outerRadius);
        t.position = new Vector3(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance, 0f);
    }

    // ��ȯ �޼��� (��� �� Ǯ�� �������� ��)
    public void ReturnMonster(baseMonster monster)
    {
        if (monster is M01MeleeMonster m01)
            m01MeleePool.ReturnObject(m01);
        else if (monster is R01RangedMonster r01)
            r01RangedPool.ReturnObject(r01);
    }
}
