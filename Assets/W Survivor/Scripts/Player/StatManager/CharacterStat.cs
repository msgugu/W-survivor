using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[Serializable]
public class CharacterStat
{
    public float BaseValue;

    public virtual float Value
    {
        // _isDirty = true only when changes (add or remove modifiers or change of BaseValue) happen
        get
        {
            if (isDirty || !Mathf.Approximately(BaseValue,lastBaseValue))
            {
                lastBaseValue = BaseValue;
                _value = CalculateFinalValue();
                isDirty = false;
            }

            return _value;
        }
    }
    
    protected bool isDirty = true;
    protected float _value;
    protected float lastBaseValue = float.MinValue;
    
    protected readonly List<StatModifier> statModifiers;
    public readonly ReadOnlyCollection<StatModifier> StatModifiers;

    public CharacterStat()
    {
        statModifiers = new List<StatModifier>();
        StatModifiers = statModifiers.AsReadOnly();
    }
    
    public CharacterStat(float baseValue) : this()
    {
        BaseValue = baseValue;
    }

    public virtual void AddModifier(StatModifier mod)
    {
        isDirty = true;
        statModifiers.Add(mod);
        statModifiers.Sort(CompareModOrder);
    }

    public virtual bool RemoveModifier(StatModifier mod)
    {
        if (statModifiers.Remove(mod))
        {
            isDirty = true;
            return true;
        }

        return false;
    }

    public virtual bool RemoveModifiersFromSource(object source)
    {
        bool removeDone = false;
        
        for (int i = statModifiers.Count - 1; i >= 0; i--)
        {
            if (statModifiers[i].Source == source)
            {
                isDirty = true;
                removeDone = true;
                statModifiers.RemoveAt(i);
            }
        }

        return removeDone;
    }

    protected virtual int CompareModOrder(StatModifier a, StatModifier b)
    {
        if (a.Order < b.Order)
            return -1;
        else if (a.Order > b.Order)
            return 1;
        return 0; // if (a.Order == b.Order)
    }

    protected virtual float CalculateFinalValue()
    {
        float finalValue = BaseValue;
        float sumPerAdd = 0;
        StatModifier mod;
        
        for (int i = 0; i < statModifiers.Count; i++)
        {
            mod = statModifiers[i];

            if (mod.Type == StatModCalcType.Add)
            {
                finalValue += mod.Value;
            }
            else if (mod.Type == StatModCalcType.PerAdd)
            {
                sumPerAdd += mod.Value;

                if ((i + 1 >= statModifiers.Count) || (statModifiers[i + 1].Type != StatModCalcType.PerAdd))
                {
                    finalValue *= 1 + sumPerAdd;
                    sumPerAdd = 0;
                }
            }
            else if (mod.Type == StatModCalcType.PerMul)
            {
                finalValue *= 1 + mod.Value;
            }
        }

        return (float)Math.Round(finalValue, 4);
    }
}

// void exampleFunc()
// {
//     // readonly로 선언했기 때문에 불가능
//     statModifiers = null;
//     statModifiers = new List<StatModifier>();
//     
//     // 가능
//     statModifiers[0] = null;
//     statModifiers.Add(new StatModifier());
// }