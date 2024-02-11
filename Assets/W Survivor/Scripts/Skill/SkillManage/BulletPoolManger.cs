using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Redcode.Pools;

public class BulletPoolManager : PoolingManager
{
    public List<IPool<Component>> BulletPools => Pools;
    private WaitForSeconds wait3s;

    protected override void Awake()
    {
        base.Awake();
        wait3s = new WaitForSeconds(3);
    }
        
    public IPool<Bullet> AddBulletPool(Bullet bullet, int count, Transform container, bool nonLazy)
    {
        return AddPool<Bullet>(bullet, count, container, nonLazy);
    }
    
    // 0 means no limits
    public IPool<Bullet> AddBulletPool(Bullet bullet, Transform container)
    {
        return AddBulletPool(bullet, 0, container, false);
    }
    
    public IPool<Bullet> GetBulletPool(int poolIndex)
    {
        return GetPool<Bullet>(poolIndex);
    }
    
    public Bullet BulletSpawn(int poolIndex)
    {
        return GetFromPool<Bullet>(poolIndex);
    }

    public void BulletDespawn(int poolIndex, Bullet bulletClone)
    {
        ReturnPool<Bullet>(poolIndex, bulletClone);
    }

    public IPool<Bullet> ResetBulletPool(int poolIndex, Bullet newBullet, bool cleanPool = false)
    {
        IPool<Bullet> currPool = GetBulletPool(poolIndex);
        if (cleanPool)
        {
            currPool.Clear(destroyClones: true);
        }

        currPool.SetSource(newBullet, false);

        return currPool;
    }
}
