using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;


public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GridManager gridManager;
    
    private float timer;
    private int spawnRange = 15;

    private int playerX = 0;
    private int playerY = 0;

    private void Start()
    {
        // observe change of player position to renew spawn position
        InGameManager.Instance.player.NotifyPlayerCell += RenewPlayerCellPos;
    }

    private void FixedUpdate()
    {
        if (InGameManager.Instance.isPaused)
            return;
        
        timer += Time.deltaTime;
        if (timer > EnemySpawnDataManager.Instance.GetEnemySpawnDatas()[InGameManager.Instance.stageLevel]
                .enemySpawnTimer)
        {
            timer = 0;
            Spawn();
        }
    }

    private void Spawn()
    {
        InGameManager.Instance.enemyPoolManager.EnemySpawn(0, GetRandomPosition(GetRandomPosNum()));
    }
    
    private void RenewPlayerCellPos(Vector3Int newCellPos)
    {
        playerX = newCellPos.x;
        playerY = newCellPos.y;
    }
    
    // return position of cell from posNum
    // ex: posNum = 19 -> position of 19th cell among outer cells
    private Vector2 GetRandomPosition(int posNum)
    {
        int quot = posNum / spawnRange;
        int rem = posNum % spawnRange;

        int x = 0, y = 0;
        switch (quot)
        {
            case 0:
            case 4:
                x = rem;
                y = spawnRange;
                break;
            case 1:
            case 5:
                x = spawnRange;
                y = rem + 1;
                break;
            case 2:
            case 6:
                x = spawnRange;
                y = rem;
                break;
            case 3:
            case 7:
                x = rem + 1;
                y = spawnRange;
                break;
        }

        if (InScope(4, 7, quot)) 
            x *= -1;
        
        if (InScope(2, 5, quot)) 
            y *= -1;

        x *= gridManager.cellSizeX;
        y *= gridManager.cellSizeY;

        x += playerX;
        y += playerY;

        return new Vector2(x, y);
    }
    
    // get random number from outer square cells
    // total cell number of cells on outer square is 8 * range
    private int GetRandomPosNum()
    {
        return UnityEngine.Random.Range(0, spawnRange * 8);
    }
    
    // helper func for GetSpawnPosition
    // Return (min <= input <= max)
    bool InScope(int min, int max, int input)
    {
        return (input >= min && input <= max);
    }
}
