using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Redcode.Pools;
public class PoolingManager : MonoBehaviour
{
    private List<IPool<Component>> _pools;
    public List<IPool<Component>> Pools => _pools;

    public GridManager gridManager;

    public int PoolNum => _pools.Count;

    protected virtual void Awake()
    {
        _pools = new();
    }
    
    public IPool<T> AddPool<T>(T source, int count, Transform container, bool nonLazy = false)
        where T : Component
    {
        IPool<T> newPool = Pool.Create(source, count, container);
        if (nonLazy)
            newPool.NonLazy();
        _pools.Add(newPool);

        return newPool;
    }
    
    public IPool<T> GetPool<T>(int index) where T : Component => (IPool<T>)_pools[index];
    
    public T GetFromPool<T>(int index) where T : Component => (T)_pools[index].Get();
    
    //spawn objects and move to pos
    public T GetFromPool<T>(int index, Vector3 pos) where T : Component => (T)_pools[index].Get(pos);

    public void ReturnPool<T>(int index, T clone) where T : Component
    {
        _pools[index].Take(clone);
    }
    
    public IPool<T> ChangePool<T>(IPool<T> pool, Component newSource, bool deactivateClones = false) where T : Component
    {
        return pool.SetSource(newSource, deactivateClones);
    }

    public IPool<T> ChangePool<T>(int index, Component newSource, bool deactivateClones) where T : Component
    {
        return ChangePool<T>(_pools[index] as IPool<T>, newSource, deactivateClones);
    }

    
    
}
