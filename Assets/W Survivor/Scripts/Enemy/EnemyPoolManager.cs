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

    // Ǯ���� ������Ʈ ����Ʈ
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
        // ����Ʈ�� �߰�
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
        // ����Ʈ�� �߰�
        spawnedEnemies.Add(newEnemy);
        newEnemy.enemyRigid.position = pos;
        
        return newEnemy;
    }

    public void EnemyDespawn(int poolIndex, Enemy enemyClone)
    {
        // ����Ʈ���� ����
        spawnedEnemies.Remove(enemyClone);
        ReturnPool<Enemy>(poolIndex, enemyClone);
    }

    /// <summary>
    /// ����Ʈ ���Ͽ�
    /// </summary>
    /// <returns></returns>
    public List<Enemy> GetSpawnedEnemies()
    {
        return spawnedEnemies;
    }
}
