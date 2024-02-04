using System;
using System.Collections;
using System.Collections.Generic;
using Redcode.Pools;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolObject
{
    [SerializeField]
    private int slotIndex;
    [SerializeField]
    private int poolIndex;
    [SerializeField]
    private BulletPoolManager poolManager;
    [SerializeField]
    
    private int pierce;
    [SerializeField]
    private int currPierce;

    private float lifeTime;
    private WaitForSeconds waitLife;
    private Coroutine lifeRoutine;

    [SerializeField]
    private Enums.BulletType bulletType;
    
    [SerializeField]
    private Rigidbody2D bulletRigid;
    private void Awake()
    {
        bulletRigid = GetComponent<Rigidbody2D>();
    }

    #region Getter Setter

    public int SlotIndex
    {
        get { return slotIndex; }
        set { slotIndex = value; }
    }
    
    public int PoolIndex
    {
        get { return poolIndex; }
        set { poolIndex = value; }
    }
    
    public int Pierce
    {
        get { return pierce; }
        set { pierce = value; }
    }

    public float LifeTime
    {
        get { return lifeTime; }
        set
        {
            lifeTime = value; 
            waitLife = new WaitForSeconds(lifeTime); 
        }
    }

    public BulletPoolManager PoolManager
    {
        get { return poolManager; }
        set { poolManager = value; }
    }

    public Rigidbody2D BulletRigid
    {
        get { return bulletRigid; }
        set { bulletRigid = value; }
    }

    public Enums.BulletType BulletType
    {
        get { return bulletType; }
        set { bulletType = value; }
    }

    #endregion

    public void InitBullet(BulletPoolManager poolManager, int slotIndex, int poolIndex, int pierce, float lifeTime)
    {
        PoolManager = poolManager;
        SlotIndex = slotIndex;
        PoolIndex = poolIndex;
        Pierce = pierce;
        LifeTime = lifeTime;
    }
    
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (pierce == -1000 || !coll.CompareTag("Enemy")) 
            return;
        
        if (--currPierce == 0)
        {
            bulletRigid.velocity = Vector2.zero;
            KillBullet();
        }
        
    }

    public void StartLifeTimer()
    {
        StopLifeTimer();
        lifeRoutine = StartCoroutine(LifeTimerRoutine());
    }

    public void StopLifeTimer()
    {
        if (lifeRoutine != null)
        {
            StopCoroutine(lifeRoutine);
            lifeRoutine = null;
        }
    }
    
    IEnumerator LifeTimerRoutine()
    {
        yield return waitLife;
        KillBullet();
    }

    public void KillBullet()
    {
        if (gameObject.activeSelf)
        {
            poolManager.BulletDespawn(poolIndex, this);
        }
    }
    
    public void OnCreatedInPool()
    {
        this.gameObject.layer = LayerMask.NameToLayer("BulletPlayer");
        ReuseBullet();
    }

    public void OnGettingFromPool()
    {
        ReuseBullet();
    }

    public void ReuseBullet()
    {
        currPierce = pierce;
    }
}