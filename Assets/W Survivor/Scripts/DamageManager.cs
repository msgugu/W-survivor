using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public SkillManager skillManager;
    public PlayerStat playerStat;
    private ISkill _currSkill;

    public float EnemyGetDamage(int slotIndex)
    {
        float damage = 0;
        _currSkill = skillManager.playerSkillSlots[slotIndex];
        damage = _currSkill.Damage + playerStat.AD.Value;

        return damage;
    }

    public float PlayerGetDamage(Collider2D coll)
    {
        float damage = 0;
        
        damage = 1;
        
        return damage;
    }
}
