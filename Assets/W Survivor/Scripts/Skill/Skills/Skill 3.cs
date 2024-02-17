using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


/// <summary>
/// 파이어 로드
/// 플레이어가 지나가는 길에 지속 딜 받는 불 소환
/// </summary>
public class Skill3 : MonoBehaviour, ISkill
{
    #region 구현부
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
    #endregion

    private Player _player;
    private Bullet _currBullet;


    public void InitSkill()
    {
        _player = InGameManager.Instance.player;
    }

    public void UpgradeSkill()
    {
    }

    public void UseSkill()
    {
        FireRode();
    }

    private void FireRode()
    {
        _currBullet = BulletPoolM.BulletSpawn(PoolIndexes[0]);
        _currBullet.transform.position = _player.transform.position;
    }

}
