using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDataManager : MonoBehaviour
{
    [SerializeField]
    public Component[] skills;

    public ISkill GetSkill(int index)
    {
        return skills[index] as ISkill;
    }
 }
