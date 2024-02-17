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
        /* ��ȯ�� ������ �����͸� ��������
         * 
         * 
         * 
         */
        
            // ���� �� ���� ������Ʈ ����
            GameObject target = enemyDataManager.enemyPrefabs[Random.Range(0,
                enemyDataManager.enemyPrefabs.Length)];

            // ���� Transform ������Ʈ�� ���� ��ǥ ���
            Vector3 targetPosition = target.transform.position;

            // ����ġ�� ��ȯ
            _currBullet = BulletPoolM.BulletSpawn(PoolIndexes[0]);
            _currBullet.transform.position = targetPosition;
        
    }
}
