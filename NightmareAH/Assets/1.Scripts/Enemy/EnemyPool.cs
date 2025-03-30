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
        // 기본 prefab을 이용해 초기 풀을 만들지만,
        // 실제 초기화는 MonsterFactory에서 개별로 AddObject()로 진행합니다.
        for (int i = 0; i < initialSize; i++)
        {
            T obj = GameObject.Instantiate(prefab, parent);
            obj.OnDespawn();
            pool.Enqueue(obj);
        }
    }

    // 외부에서 미리 생성한 객체를 추가할 때 사용합니다.
    public void AddObject(T obj)
    {
        pool.Enqueue(obj);
    }

    // 단순 GetObject() – 조건 없이 하나를 꺼냅니다.
    public T GetObject()
    {
        T obj = pool.Count > 0 ? pool.Dequeue() : GameObject.Instantiate(prefab, parent);
        obj.OnSpawn();
        return obj;
    }

    // 조건에 맞는 객체를 찾습니다.
    public T GetObject(System.Predicate<T> predicate)
    {
        T found = null;
        int count = pool.Count;
        // 임시로 순회하면서 조건에 맞는 객체를 찾습니다.
        for (int i = 0; i < count; i++)
        {
            T obj = pool.Dequeue();
            if (found == null && predicate(obj))
            {
                found = obj;
                // 조건에 맞는 객체는 꺼내고 나머지는 다시 넣습니다.
            }
            else
            {
                pool.Enqueue(obj);
            }
        }
        if (found != null)
        {
            found.OnSpawn();
            return found;
        }
        else
        {
            // 조건에 맞는 객체가 없다면 기본 prefab으로 새로 생성합니다.
            T obj = GameObject.Instantiate(prefab, parent);
            obj.OnSpawn();
            return obj;
        }
    }

    public void ReturnObject(T obj)
    {
        obj.OnDespawn();
        pool.Enqueue(obj);
    }
}