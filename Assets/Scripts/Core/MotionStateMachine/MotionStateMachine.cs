using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using System;

[Serializable]
public abstract class MotionStateMachine : StateMachine<MotionState, MotionTrigger>
{


    public override void SetBlackboardInitNeedValueList()
    {
        base.SetBlackboardInitNeedValueList();
        AddInitNeedValue<Entity>("entity");
    }


}
