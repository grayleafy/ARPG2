using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SkillSystem
{
    /// <summary>
    /// 是否使用根运动
    /// </summary>
    [Serializable]
    public class SetApplyRootMotion : DurationEffect
    {
        public bool applyRootMotionPosition = true;
        public bool applyRootMotionRotation = true;

        public override void EffectStart(SkillCaster skillCaster, Skill skill)
        {
            base.EffectStart(skillCaster, skill);
            if (skillCaster.entity.AnimatorComponent != null)
            {
                skillCaster.entity.applyRootMotionPosition = applyRootMotionPosition;
                skillCaster.entity.applyRootMotionRotation = applyRootMotionRotation;

            }
            EndCast();
        }
    }
}

