using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Redcode.Pools;

public class SkillManager : MonoBehaviour
{
    [Header("Managers")]
    public SkillDataManager playerSkillDataManager;
    public SkillBaseStatManager playerSkillBaseStatManager;
    public PoolContainerManager playerBulletContainerManager;
    public BulletPoolManager playerBulletPoolManager;
    
    [Header("Skill Infos")]
    public int activeSkillNum;
    public List<ISkill> playerSkillSlots;
    public List<bool> playerSkillSlotStates;
    public List<int> playerSkillIDs;
    
    public List<Coroutine> PlayerSkillCorutines;
    public List<WaitForSeconds> PlayerSkillCooldowns;
    
    
    private void Awake()
    {
        activeSkillNum = 0;
        playerSkillSlots = new();
        playerSkillSlotStates = new();
        playerSkillIDs = new();
        PlayerSkillCorutines = new();
        PlayerSkillCooldowns = new();
    }

    private void Start()
    {
        //for debug
        AddSkill(1);
        //AddSkill(0);
        
    }

    public void AddSkill(int skillID)
    {
        int slotIndex = playerSkillSlots.Count;
        ISkill newSkill = playerSkillDataManager.GetSkill(skillID);
        
        playerSkillSlots.Add(newSkill);
        
        InitSkillBase(skillID, slotIndex);
        
        playerSkillIDs.Add(skillID);
        playerSkillSlotStates.Add(true);
        activeSkillNum++;
        
        newSkill.InitSkill();
        if (playerSkillSlots.Count == playerSkillSlotStates.Count)
        {
            StartSkillCoroutine(slotIndex, true);
        }
        else
        {
            Debug.Log("AddSkill() failed " + slotIndex);
        }
    }

    public void DisableSkill(int slotIndex)
    {
        playerSkillSlotStates[slotIndex] = false;
        StopSkillCoroutine(slotIndex);
        activeSkillNum--;
    }

    public void ChangeSkill(int slotIndex, int skillID)
    {
        DisableSkill(slotIndex);
        AddSkill(skillID);
    }
    
    public void UpgradeSkill(int slotIndex)
    {
        playerSkillSlots[slotIndex].UpgradeSkill();
    }

    private void InitSkillBase(int skillID, int slotIndex)
    {
        ISkill currSkill = playerSkillSlots[slotIndex];
        SkillBaseStat currStat = playerSkillBaseStatManager.GetSkillBaseStat(skillID);
        
        if (currSkill.SkillID == currStat.SkillID)
        {
            int currPoolNum = currSkill.PoolNum = currStat.PoolNum;
            currSkill.Damage = currStat.Damage;
            currSkill.Count = currStat.Count;
            currSkill.Speed = currStat.Speed;
            currSkill.Pierce = currStat.Pierce;
            currSkill.Cooldown = currStat.Cooldown;

            currSkill.BulletPoolM = playerBulletPoolManager;
            currSkill.BulletContainers = new Transform[currPoolNum];
            currSkill.PoolIndexes = new int[currPoolNum];
            
            Transform currContainer;
            int poolIndex = playerBulletPoolManager.BulletPools.Count;
            for (int i = 0; i < currPoolNum; i++)
            {
                currContainer = currSkill.BulletContainers[i] = playerBulletContainerManager.AddContainer();
                currSkill.SkillBullets[i].InitBullet(playerBulletPoolManager, slotIndex, poolIndex + i, currSkill.Pierce, 10f);
                playerBulletPoolManager.AddBulletPool(currSkill.SkillBullets[i], currContainer);
                currSkill.PoolIndexes[i] = poolIndex + i;
            }
        }
        else
        {
            Debug.Log("InitSkillBaseStat() Failed " + currSkill.SkillID + ", " + currStat.SkillID);
        }
        
    }
    
    public void StartSkillCoroutine(int slotIndex, bool isAdd = false)
    {
        if (isAdd)
        {
            PlayerSkillCooldowns.Add(null);
            PlayerSkillCorutines.Add(null);
        }
        
        float newCooldown = playerSkillSlots[slotIndex].Cooldown;
        Coroutine newSkillCoroutine = StartCoroutine(UseSkillRoutine(slotIndex));
        
        PlayerSkillCooldowns[slotIndex] = new WaitForSeconds(newCooldown);
        PlayerSkillCorutines[slotIndex] = newSkillCoroutine;
    }

    public void StopSkillCoroutine(int slotIndex)
    {
        StopCoroutine(PlayerSkillCorutines[slotIndex]);
    }

    public void StopAllSkillCoroutines()
    {
        foreach (Coroutine routine in PlayerSkillCorutines)
        {
            if (routine != null)
            {
                StopCoroutine(routine);
            }
        }
    }

    //나중에 Stat 관리시 변경 필요
    public void ResetSkillCooldown(int slotIndex)
    {
        StopSkillCoroutine(slotIndex);
        StartSkillCoroutine(slotIndex);
    }
    
    private IEnumerator UseSkillRoutine(int slotIndex)
    {
        while (playerSkillSlotStates[slotIndex])
        {
            playerSkillSlots[slotIndex].UseSkill();
            yield return PlayerSkillCooldowns[slotIndex];
        }
    }
    
}
