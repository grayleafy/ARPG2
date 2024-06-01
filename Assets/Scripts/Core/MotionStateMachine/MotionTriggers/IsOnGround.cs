using FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MotionFSM
{
    public class IsOnGround : MotionTrigger
    {
        public GroundHitInfo groundHitInfo;
        public override bool CheckCondition(StateMachineBase stateMachine)
        {
            return false;
        }

        public override bool FixCheckCondition(StateMachineBase stateMachine)
        {
            return groundHitInfo.isHitGround;
        }
    }
}

