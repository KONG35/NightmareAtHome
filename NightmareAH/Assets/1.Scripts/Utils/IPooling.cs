using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 풀링 인터페이스: 오브젝트가 스폰될 때와 반환될 때 호출되는 메서드를 정의
public interface IPoolable
{
    void OnSpawn();
    void OnDespawn();
}
