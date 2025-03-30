using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ǯ�� �������̽�: ������Ʈ�� ������ ���� ��ȯ�� �� ȣ��Ǵ� �޼��带 ����
public interface IPoolable
{
    void OnSpawn();
    void OnDespawn();
}

// ���׸� ������Ʈ Ǯ Ŭ����
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
            obj.OnDespawn(); // ���� �� ��Ȱ��ȭ
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