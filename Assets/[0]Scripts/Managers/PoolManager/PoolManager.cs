using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolManager", menuName = "Managers/PoolManager")]
public class PoolManager : BaseInjectable
{
    private static Dictionary<Type,Pool> _pools = new Dictionary<Type, Pool>();
    private GameObject _poolsGO;
    public void AddPool(Type type, int poolSize = 0)
    {
         if(_poolsGO == null) _poolsGO = GameObject.Find("[POOLS]") ?? new GameObject("[POOLS]");
        
        if(!_pools.ContainsKey(type))
        {
            var poolGO = new GameObject("Pool: " + type.ToString().ToUpper());
            poolGO.transform.SetParent(_poolsGO.transform);
            var pool = poolGO.AddComponent<Pool>();
            pool.InitPool(poolGO.transform, poolSize);
            _pools.Add(type, pool);
        }
    }

    public Pool GetPool(Type type)
    {
        if (_pools.TryGetValue(type, out var pool))
        {
            return pool;
        }
        
        return null;
    }

    public T Create<T>(GameObject prefab, Transform parent,int poolSize = 0) where T : MonoBehaviour, IPoolable
    {
        AddPool(typeof(T), poolSize);
        return GetPool(typeof(T)).Spawn<T>(prefab, parent);
    }

    public void Deactivate<T>(T poolable) where T : MonoBehaviour, IPoolable
    {
        GetPool(typeof(T)).Deactivate(poolable);
    }
}
