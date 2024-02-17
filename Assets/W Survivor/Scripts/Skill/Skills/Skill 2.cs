using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 번개 소환 
/// 랜덤으로 소환된 적 선택해서 공격
/// </summary>
public class Skill2 : MonoBehaviour, ISkill
{
    #region 구현부
    [field: SerializeField]
    public int SkillID { get; set ; }
    public int PoolNum { get; set ; }
    public int[] PoolIndexes { get ; set ; }

    [field: SerializeField]
    public Bullet[] SkillBullets { get; set ; }
    public Transform[] BulletContainers { get ; set  ; }
    public BulletPoolManager BulletPoolM { get; set ; }

    public float Damage { get; set; }
    public int Count { get; set; }
    public float Speed { get; set; }
    public int Pierce { get; set; }
    public float Cooldown { get; set; }
    #endregion

    EnemyDataManager enemyDataManager;
    private Bullet _currBullet;


    public void InitSkill()
    {
        enemyDataManager = FindObjectOfType<EnemyDataManager>();
    }

    public void UpgradeSkill()
    {
    }

    public void UseSkill()
    {
        Lightning();
    }

    private void Lightning()
    {
        /* 소환된 몬스터의 데이터를 가져야함
         * 
         * 
         * 
         */
        
            // 랜덤 적 게임 오브젝트 선택
            GameObject target = enemyDataManager.enemyPrefabs[Random.Range(0,
                enemyDataManager.enemyPrefabs.Length)];

            // 적의 Transform 컴포넌트를 통해 좌표 얻기
            Vector3 targetPosition = target.transform.position;

            // 적위치에 소환
            _currBullet = BulletPoolM.BulletSpawn(PoolIndexes[0]);
            _currBullet.transform.position = targetPosition;
        
    }
}
