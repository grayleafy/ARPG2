using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SkillSystem
{
    [Serializable]
    public class AutoLock : DurationEffect
    {
        [Tooltip("索敌的搜索半径")]
        public float searchRadius = 5f;
        [Tooltip("保持的目标距离")]
        public float targetDistance = 1;
        [Tooltip("位移速度")]
        public float positionSmooth = 4;
        [Tooltip("旋转速度")]
        public float rotateSmooth = 8;
        [Tooltip("保持时间")]
        public float duration = 0.2f;

        private float leftTime;
        private Collider[] collidersCache = new Collider[10];
        private Transform lockTarget = null;



        public override void EffectStart(SkillCaster skillCaster, Skill skill)
        {
            base.EffectStart(skillCaster, skill);
            //剩余时间
            leftTime = duration;
            int cnt = Physics.OverlapSphereNonAlloc(skillCaster.gameObject.transform.position, searchRadius, collidersCache);
            //筛选出有entity的并且阵营敌对的
            List<Entity> enemies = FilterEnemy(skillCaster, collidersCache, cnt);
            //寻找角度最接近的
            float tempDot = -2;
            if (enemies.Count > 0)
            {
                foreach (var enemy in enemies)
                {
                    float dot = Vector3.Dot(skillCaster.transform.forward, enemy.transform.position - skillCaster.transform.position);
                    if (dot >= tempDot)
                    {
                        tempDot = dot;
                        lockTarget = enemy.transform;
                    }
                }
            }
            else
            {
                lockTarget = null;
            }

            //接管根运动
            skillCaster.entity.isTakenOver = true;
        }

        public override void EffectEnd(SkillCaster skillCaster, Skill skill)
        {
            base.EffectEnd(skillCaster, skill);

            skillCaster.entity.isTakenOver = false;
        }

        public override void EffectFixedUpdate(SkillCaster skillCaster, Skill skill, float dt)
        {
            base.EffectFixedUpdate(skillCaster, skill, dt);

            //接管根运动
            if (skillCaster.entity.applyRootMotionPosition)
            {
                //skillCaster.entity.AddForceSmooothlyByTargetVelocity(Vector3.zero);
                skillCaster.entity.RigidbodyComponent.MovePosition(skillCaster.entity.RigidbodyComponent.position + skillCaster.entity.rootMotionFixedDeltaPosition);
            }
            if (skillCaster.entity.applyRootMotionRotation)
            {
                //skillCaster.entity.AddTorqueByTargetAngularVelocity(Vector3.Lerp(skillCaster.entity.RigidbodyComponent.angularVelocity, Vector3.zero, 0.2f));
                skillCaster.entity.RigidbodyComponent.MoveRotation(skillCaster.entity.rootMotionFixedDeltaRotation * skillCaster.entity.RigidbodyComponent.rotation);
            }
            skillCaster.entity.rootMotionFixedDeltaPosition = Vector3.zero;
            skillCaster.entity.rootMotionFixedDeltaRotation = Quaternion.identity;

            if (lockTarget != null)
            {
                //保持距离
                Vector3 currentDirection = lockTarget.position - skillCaster.transform.position;
                Vector3 targetPosition = lockTarget.position - currentDirection.normalized * targetDistance;
                targetPosition = Vector3.Lerp(skillCaster.transform.position, targetPosition, positionSmooth * dt);
                skillCaster.entity.AddForceByTaregtPosition(targetPosition);
                //朝向
                Quaternion q = Quaternion.LookRotation(lockTarget.position - skillCaster.transform.position, Vector3.up);
                skillCaster.entity.AddTorqueSmoothlyByTargetRotation(q, rotateSmooth);
            }
        }

        public override void EffectUpdate(SkillCaster skillCaster, Skill skill, float dt)
        {
            base.EffectUpdate(skillCaster, skill, dt);
            leftTime -= dt;
            if (leftTime <= 0)
            {
                EndCast();
            }
        }

        //筛选出敌人
        private List<Entity> FilterEnemy(SkillCaster skillCaster, Collider[] colliders, int cnt)
        {
            List<Entity> enemys = new List<Entity>();
            for (int i = 0; i < cnt; i++)
            {
                Entity entity = HitInfo.GetEntityByCollider(colliders[i]);
                if (entity != null && TeamMgr.GetInstance().IsEnemy(entity.teamType, skillCaster.entity.teamType))
                {
                    enemys.Add(entity);
                }
            }
            return enemys;
        }
    }
}

