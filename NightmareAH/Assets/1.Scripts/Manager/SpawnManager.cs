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


    private ObjectPool<ItemBase> ItemPool;
    public List<ItemBase> ItemPrefabList;

    public ParticleSystem bloodParticle;
    private void Start()
    {
        ItemPool = new ObjectPool<ItemBase>(ItemPrefabList[0],100,false, this.transform);
    }

    // 풀로 반환하는 코루틴
    IEnumerator ReturnToPoolAfterDelay(MonsterAttackType type, baseMonster monster, float delay)
    {
        yield return new WaitForSeconds(delay);
    }

    public void SpawnExp(float Value,Vector3 SpawnPos)
    {
        var Item = ItemPool.GetObject(x => x.Type == ItemBase.eItemType.Exp);
        Item.SetValue(Value);
        Item.transform.position = SpawnPos;
    } 
    [Button]

    public void SpawnExpbtn()
    {
        var Item = ItemPool.GetObject(x => x.Type == ItemBase.eItemType.Exp);
        Item.SetValue(10);
        Item.transform.position = this.transform.position;

    }

    public void DespawnItem(ItemBase item)
    {
        ItemPool.ReturnObject(item);
    }
    public void DespawnMonster(MonsterEntity monster)
    {
        monsterFactory.ReturnMonster(monster);
    }
}
