using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SkillSystem
{
    /// <summary>
    /// 设置动画器的trigger
    /// </summary>
    [Serializable]
    public class SetAnimatorTrigger : DurationEffect
    {
        public string triggerName;
        public override void EffectStart(SkillCaster skillCaster, Skill skill)
        {
            base.EffectStart(skillCaster, skill);
            skillCaster.entity.AnimatorComponent?.SetTrigger(triggerName);
            EndCast();
        }
    }
}

