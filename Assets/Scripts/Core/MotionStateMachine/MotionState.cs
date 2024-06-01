using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using System;

[Serializable]
public abstract class MotionState : StateBase
{
    public Entity GetEntity(StateMachineBase stateMachineBase)
    {
        return stateMachineBase.GetBlackboard<Entity>("entity");
    }
}
