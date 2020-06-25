using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pool : MonoBehaviour
{
    private Queue<IPoolable> _pool;

    public void InitPool(Transform parent, int size = 0)
    {
        transform.SetParent(parent);
        _pool = new Queue<IPoolable>(size);
    }

    public T Spawn<T>(GameObject prefab, Transform parent, Vector3 localPosition = default,
        Vector3 localRotation = default) where T : MonoBehaviour, IPoolable
    {
        GameObject go;

        if (localPosition == default && localRotation == default)
        {
            go = Instantiate(prefab, parent);
        }
        else
        {
            go = Instantiate(prefab, localPosition, Quaternion.Euler(localRotation), parent);
        }

        return go.GetComponent<T>();
    }

    public T Activate<T>(Vector3 localPosition = default, Vector3 localRotation = default)
        where T : MonoBehaviour, IPoolable
    {
        var poolable = (T) _pool.Dequeue();

        poolable.transform.position = localPosition;
        poolable.transform.rotation = Quaternion.Euler(localRotation);
        poolable.OnActivate();

        return poolable;
    }

    public void Deactivate<T>(T obj) where T : MonoBehaviour, IPoolable
    {
        _pool.Enqueue(obj);
        obj.OnDeactivate();
    }

    public int GetCount()
    {
        return _pool.Count;
    }
}