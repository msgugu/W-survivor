using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ���� ��ȯ 
/// �������� ��ȯ�� �� �����ؼ� ����
/// </summary>
public class Skill2 : MonoBehaviour, ISkill
{
    #region ������
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
        // Ǯ���� �� ����Ʈ ��������
        List<Enemy> enemies = enemyPoolManager.GetSpawnedEnemies();

        // ���� ���ٸ� �Լ� ����
        if (enemies.Count == 0)
            return;

        // ���� �� ����
        Enemy target = enemies[Random.Range(0, enemies.Count)];

        // ���� Transform ������Ʈ�� ���� ��ǥ ���
        Vector3 targetPosition = target.transform.position;

        // �� ��ġ�� �Ҹ� ��ȯ
        _currBullet = BulletPoolM.BulletSpawn(PoolIndexes[0]);

        // �Ҹ��� ���ٸ� �Լ� ����
        if (_currBullet == null)
            return;

        _currBullet.transform.position = targetPosition;

    }
}
