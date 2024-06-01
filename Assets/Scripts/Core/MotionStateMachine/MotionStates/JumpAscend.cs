using FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MotionFSM
{
    [Serializable]
    public class JumpAscend : MotionState
    {
        Entity entity;
        public float jumpHeight = 2.5f;
        public string clipName = "Jump";
        public float moveSpeed = 6.0f;
        public AnimationCurve jumpCurve;


        float clipLength = 0;
        float duration = 0;
        Vector3 moveVelocity;
        Quaternion rotation;

        public override void EnterState(StateMachineBase stateMachineBase)
        {
            base.EnterState(stateMachineBase);
            entity = GetEntity(stateMachineBase);
            if (entity.AnimatorComponent != null)
            {
                entity.AnimatorComponent.SetBool("Jump", true);
            }



            duration = 0;
            clipLength = entity.GetClipLength(clipName);

            ////水平速度限制
            //moveVelocity = entity.RigidbodyComponent.velocity;
            //moveVelocity.y = 0;
            //if (moveVelocity.magnitude > moveSpeed)
            //{
            //    moveVelocity = moveSpeed * moveVelocity.normalized;
            //}
            //entity.AddForceByTargetVelocity(moveVelocity);

            //水平速度设置,从输入中设置
            Vector2 inputDir = (entity as IInput).GetInputRecorder().move;
            moveVelocity = (entity as IInput).GetInputRecorder().TransformMoveDirection(inputDir, Camera.main, out rotation) * moveSpeed;
            entity.AddForceByTargetVelocity(moveVelocity);


            //角速度清零
            entity.RigidbodyComponent.angularVelocity = Vector3.zero;

            //打断技能
            var skillCaster = entity.GetComponent<SkillCaster>();
            if (skillCaster != null)
            {
                skillCaster.InterruptAllSkills();
            }
        }

        public override void ExitState(StateMachineBase stateMachineBase)
        {
            base.ExitState(stateMachineBase);
            if (entity.AnimatorComponent != null)
            {
                entity.AnimatorComponent.SetBool("Jump", false);
            }
        }

        public override void Update(StateMachineBase stateMachineBase, float dt)
        {
            base.Update(stateMachineBase, dt);


        }

        public override void FixUpdate(StateMachineBase stateMachineBase, float dt)
        {
            base.FixUpdate(stateMachineBase, dt);
            duration += dt;
            if (clipLength <= duration) EndState(stateMachineBase);


            //自己设曲线得
            float h1 = jumpCurve.Evaluate((duration - dt) / clipLength);
            float h2 = jumpCurve.Evaluate(duration / clipLength);
            float deltaHeight = (h2 - h1) * jumpHeight;

            //Vector3 upDeltaPosition = new Vector3(0, deltaHeight, 0);

            //fix中更新，并且应该addforce只施加向上力
            //entity.AddForceByTaregtPosition(entity.transform.position + upDeltaPosition + moveVelocity * dt);

            float v = deltaHeight / Time.fixedDeltaTime;
            float dv = v - entity.RigidbodyComponent.velocity.y;
            entity.AddForce(new Vector3(0, dv, 0), ForceMode.VelocityChange);

            //设置方向
            entity.AddTorqueSmoothlyByTargetRotation(rotation, 10);
        }
    }
}

