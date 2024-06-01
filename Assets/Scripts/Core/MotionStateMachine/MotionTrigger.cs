using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
public abstract class MotionTrigger : TriggerBase
{
    /// <summary>
    /// 获取实体
    /// </summary>
    /// <param name="stateMachine"></param>
    /// <returns></returns>
    public Entity GetEntity(StateMachineBase stateMachine)
    {
        return stateMachine.GetBlackboard<Entity>("entity");
    }
}
