using FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MotionFSM
{
    [Serializable]
    public class Fall : MotionState
    {
        public float gravity = 20;
        public float maxFallVelocity = 18f;

        Entity entity;

        public override void EnterState(StateMachineBase stateMachine)
        {
            base.EnterState(stateMachine);
            entity = GetEntity(stateMachine);

            var animator = entity.AnimatorComponent;
            animator?.SetBool("OnGround", false);
        }



        public override void FixUpdate(StateMachineBase stateMachine, float dt)
        {
            base.FixUpdate(stateMachine, dt);
            if (entity.RigidbodyComponent.velocity.y > -maxFallVelocity)
            {
                entity.AddForce(new Vector3(0, -gravity, 0), ForceMode.Acceleration);
            }
        }
    }
}

