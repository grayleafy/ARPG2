using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SkillSystem
{
    /// <summary>
    /// 释放在地面上，根据状态机当前状态判断
    /// </summary>
    [Serializable]
    public class IsOnGround : SkillCondition
    {
        public bool isOnground = true;
        public override bool CheckCondition(Skill skill, SkillCaster caster)
        {
            var entity = caster.entity;
            return ((entity as Creature).motionStateMachine.currentState is MotionFSM.OnGround) == isOnground;
        }
    }
}

