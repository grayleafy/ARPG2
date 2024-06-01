using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Damage
{
    /// <summary>
    /// 计算施加的基本伤害
    /// </summary>
    /// <param name="characterStats"></param>
    /// <returns></returns>
    public abstract float GetBasicDamage(CharacterStats characterStats);
}
