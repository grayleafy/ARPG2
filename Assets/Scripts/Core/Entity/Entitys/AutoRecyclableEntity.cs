using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 一段时间后自动回收的实体
/// </summary>
public class AutoRecyclableEntity : PoolEntity
{
    public float survivalTime;
    private float leftSurvivalTime;

    public override void EntityOnEnable()
    {
        base.EntityOnEnable();
        leftSurvivalTime = survivalTime;
    }

    public override void EntityUpdate(float dt)
    {
        base.EntityUpdate(dt);
        leftSurvivalTime -= dt;
        if (leftSurvivalTime < 0)
        {
            PushSelfInPool();
        }
    }
}
