using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 제네릭 오브젝트 풀 클래스
public class ObjectPool<T> where T : Component, IPoolable
{
    private Queue<T> pool;
    private HashSet<T> active; 
    private T prefab;
    private Transform parent;
    public int maxSize { get;  private set; }
    private bool isContinusPool;
    public ObjectPool(T prefab, int initialSize, bool _isContinus, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;
        pool = new Queue<T>();
        active = new HashSet<T>();
        maxSize = initialSize;
        isContinusPool = _isContinus;

        // 기본 prefab을 이용해 초기 풀을 만듬
        for (int i = 0; i < initialSize; i++)
        {
            T obj = GameObject.Instantiate(prefab, parent);
            obj.OnDespawn();
            pool.Enqueue(obj);
        }
    }
    public void Setprefab(T p, Transform parent = null)
    {
        prefab = p;
        this.parent = parent;
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
        active.Add(obj);
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

            active.Add(found);
            return found;
        }
        else
        {
            // 조건에 맞는 객체를 pool 에서 못찾으면 go spawn
            T obj = GameObject.Instantiate(prefab, parent);
            obj.OnSpawn();

            active.Add(obj);
            return obj;
        }
    }

    public void ReturnObject(T obj)
    {
        obj.OnDespawn();
        pool.Enqueue(obj);
        active.Remove(obj);

        if(isContinusPool)
        {
            obj.OnSpawn();
            pool.Dequeue();
            active.Add(obj);
        }
    }
    public void SetContinusPool(bool _isContinus)
    {
        isContinusPool = _isContinus;
    }
}