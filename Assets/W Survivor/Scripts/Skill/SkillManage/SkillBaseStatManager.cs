using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBaseStatManager : MonoBehaviour
{
    public TextAsset skillBaseDataText;
    private SkillBaseStats _skillBaseStats;

    private void Awake()
    {
        TempSet();
    }

    private void TempSet()
    {
        _skillBaseStats = new SkillBaseStats();
        
        _skillBaseStats.SKillBaseStatTable = new SkillBaseStat[]
        {
            new SkillBaseStat
            {
                SkillID = 0,
                PoolNum = 1,
                Damage = 1,
                Count = 6,
                Speed = 150,
                Pierce = -1000,
                Cooldown = 0,
            },
            new SkillBaseStat
            {
                SkillID = 1,
                PoolNum = 1,
                Damage = 1,
                Count = 1,
                Speed = 15,
                Pierce = 2,
                Cooldown = 0.3f,
            },
            new SkillBaseStat
            {
                SkillID = 2,
                PoolNum = 1,
                Damage = 20,
                Count = 1,
                Speed = 0,
                Pierce = 2,
                Cooldown = 0.1f,
            },
            new SkillBaseStat
            {
                SkillID = 3,
                PoolNum = 1,
                Damage = 20,
                Count = 1,
                Speed = 0,
                Pierce = 2,
                Cooldown = 0.1f,
            },
        };
    }

    public SkillBaseStat[] GetSkillBaseStats()
    {
        return _skillBaseStats.SKillBaseStatTable;
    }

    public SkillBaseStat GetSkillBaseStat(int index)
    {
        return _skillBaseStats.SKillBaseStatTable[index];
    }
}

[System.Serializable]
public class SkillBaseStats
{
    public SkillBaseStat[] SKillBaseStatTable;
}

[System.Serializable]
public class SkillBaseStat
{
    public int SkillID;
    public int PoolNum;
    public int Damage;
    public int Count;
    public int Speed;
    public int Pierce;
    public float Cooldown;
}
