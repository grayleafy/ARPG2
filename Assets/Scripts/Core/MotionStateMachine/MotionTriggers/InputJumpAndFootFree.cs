using FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MotionFSM
{
    public class InputJumpAndFootFree : MotionTrigger
    {
        public override bool CheckCondition(StateMachineBase stateMachine)
        {
            IInput iInput = stateMachine.GetBlackboard<Entity>("entity") as IInput;
            if (iInput != null)
            {
                return iInput.GetInputRecorder().jump && stateMachine.GetBlackboard<Entity>("entity").AnimatorComponent.GetBool("FootFree");
            }
            return false;
        }

        public override bool FixCheckCondition(StateMachineBase stateMachine)
        {
            return false;
        }
    }
}

