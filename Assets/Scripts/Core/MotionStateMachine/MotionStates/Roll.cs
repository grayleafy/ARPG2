using FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MotionFSM
{
    [Serializable]
    public class Roll : MotionState
    {
        Entity entity;
        Animator animator;


        public string rollTriggerName = "Roll";
        public float rollTime = 0.583f;
        public AnimationCurve speedCurve;
        public float velocitySmooth = 12.0f;
        public float rotateSmooth = 8.0f;

        //保存开始的方向
        GroundHitInfo HitInfo
        {
            get
            {
                return ((entity as Creature).motionStateMachine as LandCreatureMotionStateMachine).hitInfo;
            }
        }
        Vector3 rollDirection;
        Quaternion rollRotation;
        float leftTime;




        public override void EnterState(StateMachineBase stateMachine)
        {
            base.EnterState(stateMachine);
            entity = GetEntity(stateMachine);
            animator = entity.AnimatorComponent;

            animator?.SetTrigger(rollTriggerName);
            animator?.SetBool("OnGround", true);

            //记录翻滚方向
            var inputRecorder = (entity as IInput).GetInputRecorder();
            if (inputRecorder.move == Vector2.zero)
            {
                rollDirection = entity.transform.forward;
                rollRotation = entity.transform.rotation;
            }
            else
            {
                rollDirection = inputRecorder.TransformMoveDirection(inputRecorder.move, Camera.main, out rollRotation);
            }

            //打断技能
            var skillCaster = entity.GetComponent<SkillCaster>();
            if (skillCaster != null)
            {
                skillCaster.InterruptAllSkills();
            }

            leftTime = rollTime;
        }

        public override void Update(StateMachineBase stateMachine, float dt)
        {
            base.Update(stateMachine, dt);

            leftTime -= dt;
            if (leftTime < 0)
            {
                EndState(stateMachine);
            }
        }

        public override void FixUpdate(StateMachineBase stateMachine, float dt)
        {
            base.FixUpdate(stateMachine, dt);

            //速度和旋转
            entity.AddTorqueSmoothlyByTargetRotation(rollRotation, rotateSmooth);
            float rollSpeed = speedCurve.Evaluate((rollTime - leftTime) / rollTime);
            entity.AddForceSmooothlyByTargetVelocity(rollDirection * rollSpeed, velocitySmooth);

            FixedStickGround(dt);
        }


        /// <summary>
        /// 贴地
        /// </summary>
        /// <param name="dt"></param>
        void FixedStickGround(float dt)
        {
            if (HitInfo.isHitGround)
            {
                float gh = HitInfo.ground.point.y;
                float h;
                if (gh <= entity.RigidbodyComponent.position.y)
                {
                    h = Mathf.Lerp(entity.RigidbodyComponent.position.y, gh, 0.5f * entity.TimeScale);
                }
                else
                {
                    h = Mathf.Lerp(entity.RigidbodyComponent.position.y, gh, 0.5f * entity.TimeScale);
                }
                float targetVelocity = (h - entity.RigidbodyComponent.position.y) / Time.fixedDeltaTime;
                float deltaVelocity = targetVelocity - entity.RigidbodyComponent.velocity.y;


                entity.AddForce(new Vector3(0, deltaVelocity / entity.TimeScale, 0), ForceMode.VelocityChange);
                //entity.AddForceByTaregtPosition(postion);
            }
        }
    }
}

