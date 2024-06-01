using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    /// <summary>
    /// 状态结束时触发，需要设置状态
    /// </summary>
    public class IsStateEnd : TriggerBase
    {
        public StateBase state;



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

