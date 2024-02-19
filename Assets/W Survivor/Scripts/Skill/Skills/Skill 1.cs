using System;
using System.Collections;
using System.Collections.Generic;
using Redcode.Pools;
using UnityEngine;

public class Skill1 : MonoBehaviour, ISkill
{
    [field: SerializeField]
    public int SkillID { get; set; }
    public int PoolNum { get; set; }
    public int[] PoolIndexes { get; set; }

    [field: SerializeField]
    public Bullet[] SkillBullets { get; set; }
    public Transform[] BulletContainers { get; set; }
    public BulletPoolManager BulletPoolM { get; set; }

    public float Damage { get; set; }
    public int Count { get; set; }
    public float Speed { get; set; }
    public int Pierce { get; set; }
    public float Cooldown { get; set; }
    public float Duration { get; set; }


    private Player _player;
    private Bullet _currBullet;  
    private Transform _currBulletTrans;
    private Vector3 _currBulletPosition;

    public void InitSkill()
    {
        _player = InGameManager.Instance.player;
    }

    public void UseSkill()
    {
        ShootBullet();
    }

    public void UpgradeSkill()
    {
        
    }
    
    private void ShootBullet()
    {
        if (!_player.enemyScanner.nearestTarget)
            return;
        
        Vector3 targetPos = _player.enemyScanner.nearestTarget.position;
        _currBulletPosition = BulletContainers[0].position;
        Vector3 dir = (targetPos - _currBulletPosition).normalized;

        _currBullet = BulletPoolM.BulletSpawn(PoolIndexes[0]);
        _currBulletTrans = _currBullet.transform;
        _currBulletTrans.position = _currBulletPosition;
        _currBulletTrans.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        _currBullet.BulletRigid.velocity = dir * Speed;
    }
    
}
