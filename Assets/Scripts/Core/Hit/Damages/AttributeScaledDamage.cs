using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttributeScaledDamage : Damage
{
    public enum IndependentAttributeType
    {
        AttackPower,
        DefensePower,
        Health,
        MaxHealth
    }
    [Tooltip("依赖的属性类型")]
    public IndependentAttributeType independentAttributeType;
    [Tooltip("伤害值与该属性的比例")]
    public float scaleRate = 1;

    public override float GetBasicDamage(CharacterStats characterStats)
    {
        float value = 0;
        if (independentAttributeType == IndependentAttributeType.AttackPower)
        {
            value = characterStats.AttackPower;
        }
        else if (independentAttributeType == IndependentAttributeType.DefensePower)
        {
            value = characterStats.DefensePower;
        }
        else if (independentAttributeType == IndependentAttributeType.Health)
        {
            value = characterStats.Health;
        }
        else if (independentAttributeType == IndependentAttributeType.MaxHealth)
        {
            value = characterStats.MaxHealth;
        }
        return value * scaleRate;
    }

    
}
