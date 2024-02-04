using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public CharacterStat Health;
    public CharacterStat Movement;
    public CharacterStat AD;
    public CharacterStat AP;
    public CharacterStat Armor;
    public CharacterStat MagicArmor;
    public CharacterStat CriticalRate;
    public CharacterStat CriticalDamage;

    private void Awake()
    {
        Health.BaseValue = 10;
        Movement.BaseValue = 3;
        AD.BaseValue = 1;
    }
}
