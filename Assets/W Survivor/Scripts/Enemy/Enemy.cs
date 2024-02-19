using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public class Enemy : MonoBehaviour, IPoolObject
{
    public int poolID;

    public int PoolID
    {
        get { return poolID; }
        set { poolID = value; }
    }
    
    public int enemyID;
    public float enemySpeed = 2.5f;
    public float enemyHealth;
    public float enemyMaxHealth;
    
    public Animator enemyAnimator;
    public Rigidbody2D enemyRigid;
    public SpriteRenderer enemySprite;
    public CapsuleCollider2D enemyCollider;
    public Transform enemyTransform;

    public bool isAlive;
    public Rigidbody2D target;
    private int _currEnemyStageLevel;
    private WaitForFixedUpdate _waitfixed;

    public HealthBar healthBar;
    
    void Awake()
    {
        enemyTransform = GetComponent<Transform>();
        enemyRigid = GetComponent<Rigidbody2D>();
        enemySprite = GetComponent<SpriteRenderer>();
        enemyCollider = GetComponent<CapsuleCollider2D>();
        enemyAnimator = GetComponent<Animator>();
        isAlive = true;
        _currEnemyStageLevel = 0;
        _waitfixed = new WaitForFixedUpdate();
    }

    private void Start()
    {
        target = InGameManager.Instance.player.PlayerRigid;
        healthBar = GetComponentInChildren<HealthBar>();
    }

    private void FixedUpdate()
    {
        if (InGameManager.Instance.isPaused)
            return;
        
        if (!isAlive || enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            return;
        }

        Purchase();
    }
    private void LateUpdate()
    {
        if (InGameManager.Instance.isPaused)
            return;
        
        if (!isAlive) return;
        enemySprite.flipX = target.position.x < enemyRigid.position.x;
    }
    
    private void Purchase()
    {
        float speedPerTime = enemySpeed * Time.fixedDeltaTime;
        Vector2 currentPosition = enemyRigid.position;
        Vector2 dirVec = target.position - currentPosition;
        Vector2 nextVec = dirVec.normalized * speedPerTime;
        enemyRigid.MovePosition(currentPosition + nextVec);
        enemyRigid.velocity = Vector2.zero;
    }

    private IEnumerator Attacked()
    {
        yield return _waitfixed;
        Vector3 playerPos = InGameManager.Instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        enemyRigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse); // 3 is magic num
    }

    public void EnemyDead()
    {
        isAlive = false;
        enemyCollider.enabled = false;
        enemyRigid.simulated = false;
        enemyAnimator.SetBool("Dead", true);
        enemySprite.sortingOrder = 1;
        InGameManager.Instance.killCount++;
        InGameManager.Instance.gemPoolManager.GemSpawn(transform.position);
    }

    public void EnemyReset()
    {
        isAlive = true;
        enemyHealth = enemyMaxHealth;
        
        enemyCollider.enabled = true;
        enemyRigid.simulated = true;
        enemyAnimator.SetBool("Dead", false);
        enemySprite.sortingOrder = 2;
        target = InGameManager.Instance.player.PlayerRigid;
    }

    private void EnemyReturnPool()
    {
        InGameManager.Instance.enemyPoolManager.EnemyDespawn(poolID, this);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isAlive)
            return;
        
        enemyAnimator.SetTrigger("Hit");
        StartCoroutine(Attacked());
        
        int skillSlotIndex = collision.GetComponent<Bullet>().SlotIndex;
        enemyHealth -= InGameManager.Instance.damageManager.EnemyGetDamage(skillSlotIndex);

        healthBar.SetHealthBar(enemyHealth / enemyMaxHealth);
        if (enemyHealth <= 0)
        {
            EnemyDead();
        }
    }
    
    public void ChangeEnemyComponent()
    {
        enemyID = EnemySpawnDataManager.Instance.GetEnemySpawnDatas()[_currEnemyStageLevel].enemyID;
        enemyMaxHealth = EnemySpawnDataManager.Instance.GetEnemySpawnDatas()[_currEnemyStageLevel].enemyMaxHP;

        enemyTransform.localScale = InGameManager.Instance.EnemyDatas[enemyID].enemyScale;
        enemyAnimator.runtimeAnimatorController =
            InGameManager.Instance.EnemyDatas[enemyID].enemyAnimatorController;
        enemyCollider.size = InGameManager.Instance.EnemyDatas[enemyID].enemyCollider.size;
    }

    public void OnCreatedInPool()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Enemy");
    }
    
    public void OnGettingFromPool()
    {
        int newStageLevel = InGameManager.Instance.stageLevel;
        if (_currEnemyStageLevel != newStageLevel)
        {
            _currEnemyStageLevel = newStageLevel;
            ChangeEnemyComponent();
        }
        EnemyReset();
    }

    
}