using FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MotionFSM
{
    /// <summary>
    /// 地面状态,只能地面生物使用
    /// </summary>
    [Serializable]
    public class OnGround : MotionState
    {
        [Header("移动速度由charactorStats控制，此处只是显示")]
        public float speed = 0;
        public float velocitySmooth = 8.0f;
        public float rotateSmooth = 8.0f;
        Entity entity;
        InputRecorder inputRecorder;
        GroundHitInfo HitInfo
        {
            get
            {
                return ((entity as Creature).motionStateMachine as LandCreatureMotionStateMachine).hitInfo;
            }
        }


        public override void EnterState(FSM.StateMachineBase stateMachineBase)
        {
            base.EnterState(stateMachineBase);
            entity = GetEntity(stateMachineBase);
            entity.AnimatorComponent.SetBool("OnGround", true);
            inputRecorder = (entity as Creature).InputRecorderComponent;

            //从角色属性中获取移动速度
            speed = (entity as IStats).CharacterStats.MoveSpeed;
            (entity as IStats).CharacterStats.updateEvent.AddListener(UpdateSpeed);
        }

        public override void ExitState(StateMachineBase stateMachineBase)
        {
            base.ExitState(stateMachineBase);
            GetEntity(stateMachineBase).AnimatorComponent.SetBool("OnGround", false);
            (entity as IStats).CharacterStats.updateEvent.RemoveListener(UpdateSpeed);
        }

        public override void FixUpdate(StateMachineBase stateMachineBase, float dt)
        {
            base.FixUpdate(stateMachineBase, dt);

            
            if (entity.AnimatorComponent != null && entity.AnimatorComponent.GetBool("FootFree") == false)
            {

            }
            else
            {
                MoveByInput();
            }

            FixedStickGround(dt);
        }

        public override void Update(StateMachineBase stateMachine, float dt)
        {
            base.Update(stateMachine, dt);

            //设置时间尺度处理前的速度
            Vector3 v = entity.RigidbodyComponent.velocity;
            Vector3 fwd = entity.transform.forward;
            Vector3 right = Vector3.Cross(Vector3.up, fwd);
            float vertical = Vector3.Dot(v, fwd) / entity.TimeScale;
            float horizontal = Vector3.Dot(v, right) / entity.TimeScale;
            entity.AnimatorComponent.SetFloat("Horizontal", horizontal, 0.2f, dt);
            entity.AnimatorComponent.SetFloat("Vertical", vertical, 0.2f, dt);

            //StickGround(dt);
        }

        void UpdateSpeed(CharacterStats stats)
        {
            speed = stats.MoveSpeed;
        }

        void MoveByInput()
        {
            Vector3 v = speed * inputRecorder.TransformMoveDirection(inputRecorder.move, Camera.main, out Quaternion rotate);
            if (entity.applyRootMotionRotation == false)
                entity.AddTorqueSmoothlyByTargetRotation(rotate, v == Vector3.zero ? 0 : rotateSmooth);
            //Debug.Log("onground");
            if (entity.applyRootMotionPosition == false)
                entity.AddForceSmooothlyByTargetVelocity(v, velocitySmooth);
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

