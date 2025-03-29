using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn
{
    public int round;
    public string id;
    public float spawnTime;
    public float radius;
    public float range;
    public int count;
    public Spawn(int _round, string _id, float _spawnTime, float _radius, float _range, int _cnt)
    {
        round = _round;
        id = _id;
        spawnTime = _spawnTime;
        radius = _radius;
        range = _range;
        count = _cnt;
    }
}
public class SpawnManager : MonoBehaviour
{
    public MonsterFactory monsterFactory;


    [SerializeField]
    private float nowTime;
    private List<Spawn> spawnList;
    private void Start()
    {
        spawnList = new List<Spawn>();

        var sData = GoogleSheetLoader.Instance.GetDataList<SpawnData>();
        for(int i = 0; i < sData.Count; i++)
        {
            spawnList.Add(sData[i].spawn);
        }
        monsterFactory.Init();
    }
    private void Update()
    {
        // 숫자 1 키를 누르면 Melee 몬스터 스폰
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            baseMonster meleeMonster = monsterFactory.CreateMonster(MonsterAttackType.Melee);
            meleeMonster.transform.position = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
            StartCoroutine(ReturnToPoolAfterDelay(MonsterAttackType.Melee, meleeMonster, 5f));
        }
        // 숫자 2 키를 누르면 Range 몬스터 스폰
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            baseMonster rangeMonster = monsterFactory.CreateMonster(MonsterAttackType.Range);
            rangeMonster.transform.position = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
            StartCoroutine(ReturnToPoolAfterDelay(MonsterAttackType.Range, rangeMonster, 5f));
        }
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
            yield return new WaitForSeconds(spawnList[i].spawnTime);

            baseMonster meleeMonster = monsterFactory.CreateMonster(MonsterAttackType.Melee);
            meleeMonster.transform.position = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
        }
        yield return null;

    }
    // 풀로 반환하는 코루틴
    IEnumerator ReturnToPoolAfterDelay(MonsterAttackType type, baseMonster monster, float delay)
    {
        yield return new WaitForSeconds(delay);
    }
}
