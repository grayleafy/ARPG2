using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FixedDamage : Damage
{
    float damageValue = 0;
    public override float GetBasicDamage(CharacterStats characterStats)
    {
       return damageValue;
    }
}
