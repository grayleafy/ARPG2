using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SkillSystem
{
    /// <summary>
    /// 普通的常用技能打断需要执行的效果
    /// </summary>
    [Serializable]
    public class CommonInterrupt : DurationEffect
    {
        public bool applyRootMotionPosition = false;
        public bool applyRootMotionRotation = false;
        public bool setAnimatorTrigger = true;
        public string TriggerName = "Interrupt";

        [Header("接管根运动")]
        public bool takeOverRootMotion = false;

        public override void EffectStart(SkillCaster skillCaster, Skill skill)
        {
            base.EffectStart(skillCaster, skill);
            skillCaster.entity.applyRootMotionPosition = applyRootMotionPosition;
            skillCaster.entity.applyRootMotionRotation = applyRootMotionRotation;


            if (setAnimatorTrigger && skillCaster.entity.AnimatorComponent != null)
            {
                skillCaster.entity.AnimatorComponent.SetTrigger(TriggerName);
            }

            skillCaster.entity.isTakenOver = takeOverRootMotion;

            EndCast();
        }
    }
}

