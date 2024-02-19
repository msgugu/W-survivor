using System;
using System.Collections;
using System.Collections.Generic;
using Redcode.Pools;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public class Skill0 : MonoBehaviour, ISkill
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
    public float Duration { get ; set ; }

    private Transform _currBulletTrans;

    public void InitSkill()
    {
        PlaceBullets();
    }

    public void UseSkill()
    {
        RotateBullets();
    }

    public void UpgradeSkill()
    {
        PlaceBullets();
    }

    private void PlaceBullets()
    {
        _currBulletTrans = null;
        for (int i = 0; i < Count; i++)
        {
            if (i < BulletContainers[0].childCount)
            {
                _currBulletTrans = BulletContainers[0].GetChild(i);
            }
            else
            {
                //_currBulletTrans = BulletPools[0].Get().transform;
                _currBulletTrans = BulletPoolM.BulletSpawn(PoolIndexes[0]).transform;
                _currBulletTrans.parent = BulletContainers[0];
            }

            _currBulletTrans.localPosition = Vector3.zero;
            _currBulletTrans.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * (360 * i / Count);
            _currBulletTrans.Rotate(rotVec);
            _currBulletTrans.Translate(_currBulletTrans.up * 1.5f, Space.World);
        }
     
    }

    private void RotateBullets()
    {
        BulletContainers[0].Rotate(Vector3.back * (Speed * Time.deltaTime));
    }
}
