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
    public float Duration { get; set; }
    #endregion

    EnemyPoolManager enemyPoolManager;
    private Bullet _currBullet;


    public void InitSkill()
    {
        
        enemyPoolManager = FindObjectOfType<EnemyPoolManager>();
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
        // 풀링된 적 리스트 가져오기
        List<Enemy> enemies = enemyPoolManager.GetSpawnedEnemies();

        // 적이 없다면 함수 종료
        if (enemies.Count == 0)
            return;

        // 랜덤 적 선택
        Enemy target = enemies[Random.Range(0, enemies.Count)];

        // 적의 Transform 컴포넌트를 통해 좌표 얻기
        Vector3 targetPosition = target.transform.position;

        // 적 위치에 불릿 소환
        _currBullet = BulletPoolM.BulletSpawn(PoolIndexes[0]);

        // 불릿이 없다면 함수 종료
        if (_currBullet == null)
            return;

        _currBullet.transform.position = targetPosition;

    }
}
