using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public class EnemyPoolManager : MonoBehaviour

{
    private PoolManager _poolManager;
    public int currEnemyNum = 0;

    void Awake()
    {
        _poolManager = GetComponent<PoolManager>();
    }

    public Enemy EnemySpawn()
    {
        // if (currEnemyNum >= 30) 
        //     return null;
        // currEnemyNum++;
        return _poolManager.GetFromPool<Enemy>(0);
    }

    public void EnemyReturnPool(Enemy enemyClone)
    {
        //currEnemyNum--;
        _poolManager.TakeToPool<Enemy>(enemyClone);
    }
    
}
