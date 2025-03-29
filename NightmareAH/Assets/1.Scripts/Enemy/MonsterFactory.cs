using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.Pool;

public class MonsterFactory : MonoBehaviour
{
    // Inspector�� �Ҵ��� ������ (PooledObject�� ��ӹ��� ���� ������)
    public M01MeleeMonster M01MeleePrefab;
    public R01RangedMonster R01RangedPrefab;

    // �� ���� Ÿ�Ժ� Ǯ
    private ObjectPool<MeleeMonster> meleePool;
    private ObjectPool<RangedMonster> rangePool;

    public int initialPoolSize = 50;

    private List<MeleeMonsterData> meleeDataList;
    private List<RangedMonsterData> rangedDataList;


    public void Init()
    {
        meleeDataList = new List<MeleeMonsterData>();
        rangedDataList = new List<RangedMonsterData>();

        meleeDataList = GoogleSheetLoader.Instance.GetDataList<MeleeMonsterData>();
        rangedDataList = GoogleSheetLoader.Instance.GetDataList<RangedMonsterData>();

        foreach(var d in meleeDataList)
        {
            if (d.Key == "m01")
            {
                M01MeleePrefab.Init(d.monster);
            }
        }
        foreach(var d in rangedDataList)
        {
            if (d.Key == "r01")
            {
                //R01RangedPrefab = new R01RangedMonster(d.monster);
                R01RangedPrefab.Init(d.monster);
            }
        }

        // initialPoolSize ��ŭ �̸� ������
        if (M01MeleePrefab != null)
        {
            meleePool = new ObjectPool<MeleeMonster>(M01MeleePrefab, initialPoolSize, transform);
        }
        if (R01RangedPrefab != null)
        {
            rangePool = new ObjectPool<RangedMonster>(R01RangedPrefab, initialPoolSize, transform);
        }
    }
    // ���丮 �޼���: ��û�� ���� Ÿ�Կ� ���� ������Ʈ Ǯ���� ������Ʈ�� �����ɴϴ�.
    public baseMonster CreateMonster(MonsterAttackType type)
    {
        switch (type)
        {
            case MonsterAttackType.Melee:
                return meleePool.GetObject();
            case MonsterAttackType.Range:
                return rangePool.GetObject();
            default:
                return null;
        }
    }
    // ���丮 �޼���: ��û�� ���� Ÿ�Կ� ���� ������Ʈ Ǯ���� ������Ʈ�� �����ɴϴ�.
    public void CreateMonster(Spawn _data)
    {
        switch (_data.id)
        {
            case "m01":
                {
                    for (int i=0;i< _data.count; i++)
                    {
                        var obj = meleePool.GetObject();
                        float angle = Random.Range(0f, 2f * Mathf.PI);
                        float distance = Random.Range(_data.innerRadius, _data.outerRadius);
                        obj.transform.position = new Vector3(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance, 0f);
                    }
                        break;
                }
            case "r01":
                {
                    for (int i = 0; i < _data.count; i++)
                    {
                        var obj = rangePool.GetObject();
                        float angle = Random.Range(0f, 2f * Mathf.PI);
                        float distance = Random.Range(_data.innerRadius, _data.outerRadius);
                        obj.transform.position = new Vector3(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance, 0f);
                    }
                    break;
                }

            default:
                break;
        }

    }
}


