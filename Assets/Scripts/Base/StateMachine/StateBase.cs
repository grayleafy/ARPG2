using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    [Serializable]
    public class StateBase
    {
        [HideInInspector]
        public bool isEnd = false;

        /// <summary>
        /// 主动结束当前状态
        /// </summary>
        /// <param name="stateMachine"></param>
        public void EndState(StateMachineBase stateMachine)
        {
            isEnd = true;
        }

        public virtual void EnterState(StateMachineBase stateMachine)
        {
            isEnd = false;
        }

        public virtual void ExitState(StateMachineBase stateMachine)
        {
            isEnd = true;
        }

        public virtual void Update(StateMachineBase stateMachine, float dt)
        {
            if (isEnd) { return; }
        }

        public virtual void FixUpdate(StateMachineBase stateMachine, float dt)
        {
            if (isEnd) { return; }
        }
    }
}

