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
    private Queue<T> pool = new Queue<T>();
    private T prefab;
    private Transform parent;

    // ���� �� �ʱ� Ǯ ũ�⸸ŭ �̸� �����صӴϴ�.
    public ObjectPool(T prefab, int initialSize, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;
        for (int i = 0; i < initialSize; i++)
        {
            T obj = GameObject.Instantiate(prefab, parent);
            obj.OnDespawn(); // ���� �� ��Ȱ��ȭ
            pool.Enqueue(obj);
        }
    }

    // Ǯ���� ������Ʈ�� ��������, Ǯ�� ������ �������� �� ������Ʈ�� �����մϴ�.
    public T GetObject()
    {
        T obj;
        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            // �ʿ�� ���� �߰�
            obj = GameObject.Instantiate(prefab, parent);
        }
        obj.OnSpawn();
        return obj;
    }

    // ����� ���� ������Ʈ�� Ǯ�� ��ȯ
    public void ReturnObject(T obj)
    {
        obj.OnDespawn();
        pool.Enqueue(obj);
    }
}