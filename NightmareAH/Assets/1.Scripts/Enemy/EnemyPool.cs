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
    private Queue<T> pool;
    private T prefab;
    private Transform parent;

    public ObjectPool(T prefab, int initialSize, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;
        pool = new Queue<T>();
        for (int i = 0; i < initialSize; i++)
        {
            T obj = GameObject.Instantiate(prefab, parent);
            obj.OnDespawn(); // 생성 후 비활성화
            pool.Enqueue(obj);
        }
    }

    public T GetObject()
    {
        T obj = pool.Count > 0 ? pool.Dequeue() : GameObject.Instantiate(prefab, parent);
        obj.OnSpawn();
        return obj;
    }

    public void ReturnObject(T obj)
    {
        obj.OnDespawn();
        pool.Enqueue(obj);
    }
}