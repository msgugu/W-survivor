using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    private float timer;

    private void Update()
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
        Enemy newEnemy = InGameManager.Instance.enemyPoolManager.EnemySpawn();

        if (newEnemy is null)
        {
            
        }
        else
        {
            newEnemy.transform.position = spawnPoints[UnityEngine.Random.Range(1, spawnPoints.Length)].position;
        }
    }
}
