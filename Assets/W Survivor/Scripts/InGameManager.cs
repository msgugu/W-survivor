using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    private static InGameManager instance;
    private static bool needAdd;
    
    public Player player;
    // public PlayerStat playerStat;   

    public EnemyPoolManager enemyPoolManager;
    public EnemyDataManager enemyDataManager;
    public EnemyData[] EnemyDatas;

    public GemPoolManager gemPoolManager;
    public DamageManager damageManager;

    [Header("# Game Control")] 
    public bool isPaused;
    public float gameTimer;
    public int stageLevel;

    [Header("# Player Info")]
    public int playerLevel;
    public int killCount;
    public int currExp;
    public int[] maxExp;

    public static InGameManager Instance
    {
        get
        {
            if (!needAdd)
            {
                needAdd = true;
                GameObject temp = GameObject.Find("InGameManager");
                if (temp == null)
                {
                    temp = new GameObject() { name = "InGameManager" };
                    temp.AddComponent<InGameManager>();
                    
                }
                DontDestroyOnLoad(temp);
                instance = temp.GetComponent<InGameManager>();
            }

            return instance;
        }
    }
    private void Awake()
    {
        instance = this;
        stageLevel = 0;
        gameTimer = 0;
        
        playerLevel = 0;
        killCount = 0;
        currExp = 0;
        InitExpTable();
    }

    private void Start()
    {
        EnemyDatas = enemyDataManager.EnemyDataArray;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        
        if (isPaused)
            return;
        
        gameTimer += Time.deltaTime;
        stageLevel = Mathf.FloorToInt(gameTimer * 0.1f);
    }

    private void InitExpTable()
    {
        maxExp = new int[500];
        for (int i = 0; i < 500; i++)
        {
            maxExp[i] = (i+1) * 10;
        }
    }

    public void GetExp(int exp)
    {
        currExp += exp;

        if (currExp == maxExp[playerLevel])
        {
            playerLevel++;
            currExp = 0;
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
    }
}
