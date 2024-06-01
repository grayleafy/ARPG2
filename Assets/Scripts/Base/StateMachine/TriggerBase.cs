using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    [Serializable]
    public abstract class TriggerBase
    {
        /// <summary>
        /// 每一帧Update中检测
        /// </summary>
        /// <param name="stateMachine"></param>
        /// <returns></returns>
        public abstract bool CheckCondition(StateMachineBase stateMachine);
        /// <summary>
        /// FixUpdate中检测
        /// </summary>
        /// <param name="stateMachine"></param>
        /// <returns></returns>
        public abstract bool FixCheckCondition(StateMachineBase stateMachine);
    }
}

