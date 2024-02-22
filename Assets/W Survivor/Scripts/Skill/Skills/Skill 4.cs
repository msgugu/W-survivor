using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 지뢰설치
/// </summary>
public class Skill4 : MonoBehaviour ,ISkill
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
    public float Duration { get; set; }

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
        Mine();
    }

    private void Mine()
    {
        _currBullet = BulletPoolM.BulletSpawn(PoolIndexes[1]);
        _currBullet.transform.position = _player.transform.position;
    }
}

    
