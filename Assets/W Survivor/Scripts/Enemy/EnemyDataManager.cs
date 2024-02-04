using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDataManager : MonoBehaviour
{
    //private static EnemyDataManager instance;
    private EnemyData[] enemyDataArray;
    public GameObject[] enemyPrefabs;

    #region singleton version
    
    // public static EnemyDataManager Instance
    // {
    //     get
    //     {
    //         if (null == instance)
    //         {
    //             return null;
    //         }
    //         return instance;
    //     }
    // }
    //
    // private void Awake()
    // {
    //     if (instance == null)
    //     {
    //         instance = this;
    //         
    //         ExtractEnemyData();
    //         
    //         DontDestroyOnLoad(this.gameObject);
    //     }
    //     else
    //     {
    //         Destroy(this.gameObject);
    //     }
    //     
    // }

    #endregion

    private void Awake()
    {
        ExtractEnemyData();
    }

    private void ExtractEnemyData()
    {
        enemyDataArray = new EnemyData[enemyPrefabs.Length];
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            GameObject enemyPrefab = enemyPrefabs[i];
            EnemyData enemyData = new EnemyData(enemyPrefab.GetComponent<Transform>().localScale,enemyPrefab.GetComponent<SpriteRenderer>(), enemyPrefab.GetComponent<Animator>().runtimeAnimatorController, enemyPrefab.GetComponent<CapsuleCollider2D>());
            enemyDataArray[enemyPrefabs[i].GetComponent<Enemy>().enemyID] = enemyData;
        }
    }

    public EnemyData[] EnemyDataArray => enemyDataArray;
    
}
