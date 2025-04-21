using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // 실제 초기화는 MonsterFactory에서 개별로 AddObject()로 초기화함
        for (int i = 0; i < initialSize; i++)
        {
            T obj = GameObject.Instantiate(prefab, parent);
            obj.OnDespawn();
            pool.Enqueue(obj);
        }
    }

    /// <summary>
    /// 외부에서 미리 생성한 객체를 추가할 때 사용하는 함수
    /// </summary>
    /// <param name="obj"></param>
    public void AddObject(T obj)
    {
        pool.Enqueue(obj);
    }
    /// <summary>
    /// 조건 없이 하나를 꺼내는 함수
    /// </summary>
    /// <returns></returns>
    public T GetObject()
    {
        T obj = pool.Count > 0 ? pool.Dequeue() : GameObject.Instantiate(prefab, parent);
        obj.OnSpawn();
        return obj;
    }

    /// <summary>
    /// 조건에 맞는 객체를 찾는 함수
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public T GetObject(System.Predicate<T> predicate)
    {
        T found = null;
        int count = pool.Count;
        // 임시로 순회하면서 조건에 맞는 객체를 찾고,
        for (int i = 0; i < count; i++)
        {
            T obj = pool.Dequeue();
            if (found == null && predicate(obj))
            {
                // 조건에 맞는 객체는 꺼내고 나머지는 다시 넣는다.
                found = obj;
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
            // 조건에 맞는 객체가 없다면 기본 prefab으로 새로 생성.
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