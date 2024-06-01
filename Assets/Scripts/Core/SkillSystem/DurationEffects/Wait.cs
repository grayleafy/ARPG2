using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SkillSystem
{
    /// <summary>
    /// 等待一段时间
    /// </summary>
    [Serializable]
    public class Wait : DurationEffect
    {
        public float waitTime = 0;
        private float leftTime = 0;

        public override void EffectStart(SkillCaster skillCaster, Skill skill)
        {
            base.EffectStart(skillCaster, skill);
            leftTime = waitTime;
        }

        public override void EffectUpdate(SkillCaster skillCaster, Skill skill, float dt)
        {
            base.EffectUpdate(skillCaster, skill, dt);
            leftTime -= dt;
            if (leftTime < 0)
            {
                EndCast();
            }
        }
    }
}

