using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.Pool;

public class MonsterFactory : MonoBehaviour
{
    // Inspector에 할당할 프리팹 (PooledObject를 상속받은 몬스터 프리팹)
    public M01MeleeMonster M01MeleePrefab;
    public R01RangedMonster R01RangedPrefab;

    // 각 몬스터 타입별 풀
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

        // initialPoolSize 만큼 미리 만들어둠
        if (M01MeleePrefab != null)
        {
            meleePool = new ObjectPool<MeleeMonster>(M01MeleePrefab, initialPoolSize, transform);
        }
        if (R01RangedPrefab != null)
        {
            rangePool = new ObjectPool<RangedMonster>(R01RangedPrefab, initialPoolSize, transform);
        }
    }
    // 팩토리 메서드: 요청된 몬스터 타입에 따라 오브젝트 풀에서 오브젝트를 가져옵니다.
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
    // 팩토리 메서드: 요청된 몬스터 타입에 따라 오브젝트 풀에서 오브젝트를 가져옵니다.
    public baseMonster CreateMonster(string _id)
    {
        switch (_id)
        {
            case "m01":
                return meleePool.GetObject();
            case "r01":
                return rangePool.GetObject();
            default:
                return null;
        }
    }
}


