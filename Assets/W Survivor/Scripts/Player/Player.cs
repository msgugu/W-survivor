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

    private Vector3Int lastCellPos;

    [SerializeField] 
    public bool isGhost;
    [SerializeField]
    public bool isAlive;
    
    [Header ("Animation Hash")]
    private int playerSpeedAnimationHash;
    private int playerDeadAnimationHash;

    [Header("Utility")]
    [SerializeField]
    private GridManager gridManager;
    public EnemyScan enemyScanner;
    public HealthBar playerHealthBar;
    private IEnumerator blinkRoutine;
    
    [Header ("Component")]
    private Rigidbody2D playerRigid;
    private SpriteRenderer playerSprite;
    private Animator playerAnimator;
    private Collider2D playerCollider;
    
    // damageDelay: term for being invincible after get damage
    // playerLayer: able to get damgae, ghostLayer: disable to get damage
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
        
        damageDelay = new WaitForSeconds(0.5f);
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
        Init();
        playerSpeedAnimationHash = Animator.StringToHash("PlayerSpeed");
        playerDeadAnimationHash = Animator.StringToHash("PlayerDead");
        
        lastCellPos = gridManager.grid.WorldToCell(playerRigid.position);
    }

    private void FixedUpdate()
    {
        if (InGameManager.Instance.isPaused)
            return;
        
        GetPlayerCellPos();
        MovePlayer();
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
    
    private void Init()
    {
        playerMaxHP = playerStat.Health.Value;
        playerSpeed = playerStat.Movement.Value;
        playerHP = playerMaxHP;
    }

    #region Handle player movement

    // keyboard input => playerInputVec
    private void OnMove(InputValue value)
    {
        playerInputVec = value.Get<Vector2>();
    }

    // move player to the player input vector
    private void MovePlayer()
    {
        float speedPerTime = playerSpeed * Time.fixedDeltaTime;
        Vector2 playerNextVec = playerRigid.position + playerInputVec.normalized * speedPerTime;
        playerRigid.MovePosition(playerNextVec);
    }
    
    //observe change of cell position where player is located
    public delegate void PlayerCellChanged(Vector3Int pos);
    public event PlayerCellChanged NotifyPlayerCell;
    
    private void GetPlayerCellPos()
    {
        Vector3Int newCellPos = gridManager.grid.WorldToCell(playerRigid.position);
        if (lastCellPos != newCellPos)
        {
            lastCellPos = newCellPos;
            if (NotifyPlayerCell == null)
                return;
            NotifyPlayerCell(newCellPos);
        }
    }

    #endregion


    #region Handle when player get damage

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

    #endregion
    
}
