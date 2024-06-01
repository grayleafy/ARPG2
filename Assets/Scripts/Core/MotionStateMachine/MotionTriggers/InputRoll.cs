using FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MotionFSM
{
    [Serializable]
    public class InputRoll : MotionTrigger
    {
        public override bool CheckCondition(StateMachineBase stateMachine)
        {
            return (stateMachine.GetBlackboard<Entity>("entity") as IInput).GetInputRecorder().roll;
        }

        public override bool FixCheckCondition(StateMachineBase stateMachine)
        {
            return (stateMachine.GetBlackboard<Entity>("entity") as IInput).GetInputRecorder().roll;
        }
    }
}

