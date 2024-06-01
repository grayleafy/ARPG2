using FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimatorPause : MotionState
{
    Entity entity;
    float preAnimatorSpeed;

    public override void EnterState(StateMachineBase stateMachine)
    {
        base.EnterState(stateMachine);
        entity = GetEntity(stateMachine);

        preAnimatorSpeed = entity.AnimatorComponent.speed / entity.TimeScale;
        entity.AnimatorComponent.speed = 0;
    }


    public override void ExitState(StateMachineBase stateMachine)
    {
        base.ExitState(stateMachine);
        entity.AnimatorComponent.speed = preAnimatorSpeed * entity.TimeScale;
    }
}
