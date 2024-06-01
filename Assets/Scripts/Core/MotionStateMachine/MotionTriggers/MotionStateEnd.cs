using FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MotionFSM
{
    public class MotionStateEnd : MotionTrigger
    {
        public MotionState state;

        public override bool CheckCondition(StateMachineBase stateMachine)
        {
            return state.isEnd;
        }

        public override bool FixCheckCondition(StateMachineBase stateMachine)
        {
            return state.isEnd;
        }

    }
}

