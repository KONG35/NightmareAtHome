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
public class SpawnManager : MonoBehaviour
{
    public MonsterFactory monsterFactory;


    [SerializeField]
    private float nowTime;
    private List<SpawnData> spawnList;
    private void Start()
    {
        //spawnList = new List<Spawn>();

        //var sData = GoogleSheetLoader.Instance.GetDataList<SpawnData>();
        //for(int i = 0; i < sData.Count; i++)
        //{
        //    spawnList.Add(sData[i].spawn);
        //}
        spawnList = GoogleSheetLoader.Instance.GetDataList<SpawnData>();
        monsterFactory.Init();
    }

    [Button]
    private void SpawnStartEdit()
    {
        // 구글시트에서 받아온대로 spawn
        nowTime = 0f;
        StartCoroutine(CreateMonsterCor());

    }
    IEnumerator CreateMonsterCor()
    {
        for(int i = 0; i < spawnList.Count; i++)
        {
            yield return new WaitForSeconds(spawnList[i].spawn.spawnTime);

            monsterFactory.CreateMonster(spawnList[i].spawn);
            //baseMonster meleeMonster = monsterFactory.CreateMonster(spawnList[i].id);
            //meleeMonster.transform.position = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
        }
        yield return null;

    }
    // 풀로 반환하는 코루틴
    IEnumerator ReturnToPoolAfterDelay(MonsterAttackType type, baseMonster monster, float delay)
    {
        yield return new WaitForSeconds(delay);
    }
}
