using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    private Queue<IPoolable> _pool;

    public void InitPool(Transform parent, int size = 0)
    {
        transform.SetParent(parent);
        _pool = new Queue<IPoolable>(size);
    }

    public T Spawn<T>(GameObject prefab, Transform parent) where T : MonoBehaviour, IPoolable
    {
        var go = Instantiate(prefab, parent);
        return go.GetComponent<T>();
    }

    public T Activate<T>() where T : IPoolable
    {
        var poolable = (T)_pool.Dequeue();
        poolable.OnActivate();
        return poolable;
    }

    public void Deactivate<T>(T obj) where T : MonoBehaviour, IPoolable
    {
        _pool.Enqueue(obj);
        obj.OnDeactivate();
    }
}
