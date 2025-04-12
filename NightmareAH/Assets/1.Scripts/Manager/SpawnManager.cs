using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn
{
    public int round;
    public string id;
    public float spawnTime;
    public float innerRadius;
    public float outerRadius;
    public int count;
    public Spawn(int _round, string _id, float _spawnTime, float _inner, float _outer, int _cnt)
    {
        round = _round;
        id = _id;
        spawnTime = _spawnTime;
        innerRadius = _inner;
        outerRadius = _outer;
        count = _cnt;
    }
}
public class SpawnManager : Singleton<SpawnManager>
{

    public MonsterFactory monsterFactory;


    private List<SpawnSheetData> spawnList;

    private ObjectPool<ItemBase> ItemPool;

    private void Start()
    {
        //    spawnList = GoogleSheetLoader.Instance.GetDataList<SpawnData>();
        //    monsterFactory.Init();
    }

    [Button]
    private void InitEdit()
    {

        spawnList = GoogleSheetLoader.Instance.GetDataList<SpawnSheetData>();
        monsterFactory.Init();
    }
    [Button]
    private void SpawnStartEdit()
    {
        // 구글시트에서 받아온대로 spawn
        StartCoroutine(CreateMonsterCor());

    }
    IEnumerator CreateMonsterCor()
    {
        for(int i = 0; i < spawnList.Count; i++)
        {
            yield return new WaitForSeconds(spawnList[i].spawn.spawnTime);

            monsterFactory.CreateMonster(spawnList[i].spawn);
        }
        yield return null;

    }
    // 풀로 반환하는 코루틴
    IEnumerator ReturnToPoolAfterDelay(MonsterAttackType type, baseMonster monster, float delay)
    {
        yield return new WaitForSeconds(delay);
    }

    public void SpawnExp(ItemBase.eItemType ItemType,float Value,Vector3 SpawnPos)
    {
        var Item = ItemPool.GetObject(x => x.Type == ItemType);
    }


}
