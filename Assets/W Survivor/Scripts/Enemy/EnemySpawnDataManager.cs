using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnDataManager : MonoBehaviour
{
    public static EnemySpawnDataManager instance;
    public TextAsset enemySpawnDataText;
    private EnemySpawnDatas enemySpawnDatas;

    public static EnemySpawnDataManager Instance
    {
        get
        {
            if (instance is null)
            {
                return null;
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance is null)
        {
            instance = this;
            enemySpawnDatas = JsonUtility.FromJson<EnemySpawnDatas>(enemySpawnDataText.text);

            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public EnemySpawnData[] GetEnemySpawnDatas()
    {
        return enemySpawnDatas.EnemySpawnTable;
    }
}


[System.Serializable]
public class EnemySpawnDatas
{
    public EnemySpawnData[] EnemySpawnTable;
}

[System.Serializable]
public class EnemySpawnData
{
    public int stageLevel;
    public int enemyID;
    public float enemyMaxHP;
    public float enemySpawnTimer;
}
