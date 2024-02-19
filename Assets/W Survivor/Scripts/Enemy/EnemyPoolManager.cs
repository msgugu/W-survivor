using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public class EnemyPoolManager : PoolingManager
{
    public List<IPool<Component>> EnemyPools => Pools;

    public Enemy defaultEnemy;
    public Transform enemyContainer;

    // 풀링된 오브젝트 리스트
    private List<Enemy> spawnedEnemies = new List<Enemy>();

    protected override void Awake()
    {
        base.Awake();
        AddEnemyPool(defaultEnemy, 500, enemyContainer, false);
    }
    
    // count zero means no limits
    public IPool<Enemy> AddEnemyPool(Enemy enemy, int count, Transform container, bool nonLazy)
    {
        return AddPool<Enemy>(enemy, count, container, nonLazy);
    }

    public IPool<Enemy> GetEnemyPool(int poolIndex)
    {
        return GetPool<Enemy>(poolIndex);
    }

    // Get Enemy from pool
    public Enemy EnemySpawn(int poolIndex)
    {
        Enemy newEnemy = GetFromPool<Enemy>(poolIndex);
        if (newEnemy is null)
            return null;
        
        newEnemy.PoolID = poolIndex;
        // 리스트에 추가
        spawnedEnemies.Add(newEnemy);
        return newEnemy;
    }
    
    // Get Enemy from pool and place it on pos
    public Enemy EnemySpawn(int poolIndex, Vector2 pos)
    {
        Enemy newEnemy = GetFromPool<Enemy>(poolIndex);
        if (newEnemy is null)
            return null;
        
        newEnemy.PoolID = poolIndex;
        // 리스트에 추가
        spawnedEnemies.Add(newEnemy);
        newEnemy.enemyRigid.position = pos;
        
        return newEnemy;
    }

    public void EnemyDespawn(int poolIndex, Enemy enemyClone)
    {
        // 리스트에서 삭제
        spawnedEnemies.Remove(enemyClone);
        ReturnPool<Enemy>(poolIndex, enemyClone);
    }

    /// <summary>
    /// 리스트 리턴용
    /// </summary>
    /// <returns></returns>
    public List<Enemy> GetSpawnedEnemies()
    {
        return spawnedEnemies;
    }
}
