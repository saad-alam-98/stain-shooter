using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class ObjectPooler<T>
    where T : MonoBehaviour
{
    private T originalPrefab;
    private int initialPoolSize;

    private List<T> poolItems;
    private List<T> activePool;
    private Transform parent;

    public ObjectPooler(T preset, Transform parent, int poolSize)
    {
        this.originalPrefab = preset;
        this.initialPoolSize = poolSize;
        this.parent = parent;

        InitializePool();
    }

    private void InitializePool()
    {
        poolItems = new List<T>(initialPoolSize);
        activePool = new List<T>();

        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewPooledObject();
        }
    }

    private T CreateNewPooledObject()
    {
        T obj = GameObject.Instantiate(originalPrefab, parent);
        obj.gameObject.SetActive(false);
        poolItems.Add(obj);
        return obj;
    }

    public T GetPooledObject()
    {
        if (poolItems.Count > 0)
        {
            T item = poolItems.First();
            poolItems.Remove(item);
            activePool.Add(item);
            item.transform.SetAsLastSibling();
            return item;
        }

        T newItem = CreateNewPooledObject();
        poolItems.Remove(newItem);
        activePool.Add(newItem);
        return newItem;
    }

    public void ReturnToPool(T item)
    {
        if (activePool.Contains(item))
        {
            activePool.Remove(item);
            poolItems.Add(item);
            item.gameObject.SetActive(false);
        }
    }

    public void ClearAllActiveObjects()
    {
        foreach (T item in activePool.ToList())
        {
            ReturnToPool(item);
        }
    }
}
