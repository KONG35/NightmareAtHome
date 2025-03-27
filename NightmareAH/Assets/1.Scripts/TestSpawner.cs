using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    public PlayerCharacter Player;
    public Enemy prefEnemy;

    public List<Enemy> EnemyList;
    void Start()
    {
        InvokeRepeating("SpawnMonster", 0f, 1f);
    }
    public void SpawnMonster()
    {
        if (Player == null) 
            return;
        Vector3 spawnPosition = GetRandomPosition(Player.transform.position, 15f, 20f);
        EnemyList.Add(Instantiate(prefEnemy, spawnPosition, Quaternion.identity));
    }

    private Vector3 GetRandomPosition(Vector3 center, float minRadius, float maxRadius)
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float rad = Random.Range(minRadius, maxRadius);

        float x = center.x + Mathf.Cos(angle) * rad;
        float y = center.y + Mathf.Sin(angle) * rad;

        return new Vector3(x, y, center.z);
    }
}
