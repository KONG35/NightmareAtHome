using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 풀링 인터페이스: 오브젝트가 스폰될 때와 반환될 때 호출되는 메서드를 정의
public interface IPoolable
{
    void OnSpawn();
    void OnDespawn();
}

// 제네릭 오브젝트 풀 클래스
public class ObjectPool<T> where T : Component, IPoolable
{
    private Queue<T> pool = new Queue<T>();
    private T prefab;
    private Transform parent;

    // 생성 시 초기 풀 크기만큼 미리 생성해둡니다.
    public ObjectPool(T prefab, int initialSize, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;
        for (int i = 0; i < initialSize; i++)
        {
            T obj = GameObject.Instantiate(prefab, parent);
            obj.OnDespawn(); // 생성 후 비활성화
            pool.Enqueue(obj);
        }
    }

    // 풀에서 오브젝트를 가져오며, 풀에 없으면 동적으로 새 오브젝트를 생성합니다.
    public T GetObject()
    {
        T obj;
        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            // 필요시 동적 추가
            obj = GameObject.Instantiate(prefab, parent);
        }
        obj.OnSpawn();
        return obj;
    }

    // 사용이 끝난 오브젝트를 풀에 반환
    public void ReturnObject(T obj)
    {
        obj.OnDespawn();
        pool.Enqueue(obj);
    }
}