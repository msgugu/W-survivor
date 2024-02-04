using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 playerInputVec;

    [Header("Status")] 
    [SerializeField]
    private PlayerStat playerStat;
    [SerializeField]
    private float playerSpeed;
    [SerializeField]
    private float playerMaxHP;
    [SerializeField]
    private float playerHP;

    [SerializeField] 
    public bool isGhost;
    [SerializeField]
    public bool isAlive;
    
    [Header ("Animation Hash")]
    private int playerSpeedAnimationHash;
    private int playerDeadAnimationHash;
    
    [Header ("Utility")]
    public EnemyScan enemyScanner;
    public HealthBar playerHealthBar;
    private IEnumerator blinkRoutine;
    
    [Header ("Component")]
    private Rigidbody2D playerRigid;
    private SpriteRenderer playerSprite;
    private Animator playerAnimator;
    private Collider2D playerCollider;
    
    private WaitForSeconds damageDelay;
    private int playerLayer;
    private int ghostLayer;
    
    #region Getter Setter

    public PlayerStat PlayerStat
    {
        get { return playerStat; }
        set { playerStat = value; }
    }
    
    public Rigidbody2D PlayerRigid
    {
        get { return playerRigid; }
        set { playerRigid = value; }
    }

    public SpriteRenderer PlayerSprite
    {
        get { return playerSprite; }
        set { playerSprite = value; }
    }

    public Animator PlayerAnimator
    {
        get { return playerAnimator; }
        set { playerAnimator = value; }
    }

    public Collider2D PlayerCollider
    {
        get { return playerCollider; }
        set { playerCollider = value; }
    }

    #endregion

    void Awake()
    {
        isGhost = false;
        isAlive = true;
        
        damageDelay = new WaitForSeconds(3f);
        playerLayer = LayerMask.NameToLayer("Player");
        ghostLayer = LayerMask.NameToLayer("Ghost");
        
        gameObject.layer = playerLayer;
        
        playerRigid = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();
        
        enemyScanner = GetComponent<EnemyScan>();
        blinkRoutine = new SpriteUtil().BlinkRoutine(playerSprite);
    }

    void Start()
    {
        InitStat();
        playerSpeedAnimationHash = Animator.StringToHash("PlayerSpeed");
        playerDeadAnimationHash = Animator.StringToHash("PlayerDead");
    }

    private void FixedUpdate()
    {
        if (InGameManager.Instance.isPaused)
            return;
        
        float speedPerTime = playerSpeed * Time.fixedDeltaTime;
        Vector2 playerNextVec = playerInputVec.normalized * speedPerTime;
        playerRigid.MovePosition(playerRigid.position + playerNextVec);
    }

    void OnMove(InputValue value)
    {
        playerInputVec = value.Get<Vector2>();
    }

    void LateUpdate()
    {
        if (InGameManager.Instance.isPaused)
            return;
        
        playerAnimator.SetFloat(playerSpeedAnimationHash, playerInputVec.magnitude);
        
        if (playerInputVec.x != 0)
        {
            playerSprite.flipX = playerInputVec.x < 0;
        }
    }

    public void InitStat()
    {
        playerMaxHP = playerStat.Health.Value;
        playerSpeed = playerStat.Movement.Value;
        playerHP = playerMaxHP;
    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
        if (!coll.collider.CompareTag("Enemy") || isGhost)
            return;
        
        StartCoroutine(GhostModeRoutine(damageDelay));
        PlayerAttacked(InGameManager.Instance.damageManager.PlayerGetDamage(coll.collider));
    }

    public void PlayerAttacked(float damage)
    {
        playerHP -= damage;
        playerHealthBar.SetHealthBar(playerHP / playerMaxHP);
    }
    
    IEnumerator GhostModeRoutine(WaitForSeconds wfs)
    {
        isGhost = true;
        gameObject.layer = ghostLayer;
        StartCoroutine(blinkRoutine);
        yield return wfs;
        StopCoroutine(blinkRoutine);
        gameObject.layer = playerLayer;
        isGhost = false;
    }
}
