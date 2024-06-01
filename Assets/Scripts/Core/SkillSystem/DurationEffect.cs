using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SkillSystem
{
    [Serializable]
    public abstract class DurationEffect : SkillEffect
    {
        private bool isCasting = false;

        public override void EffectStart(SkillCaster skillCaster, Skill skill)
        {
            base.EffectStart(skillCaster, skill);
            isCasting = true;
        }

        public override void EffectEnd(SkillCaster skillCaster, Skill skill)
        {
            base.EffectEnd(skillCaster, skill);
            isCasting = false;
        }

        public override void EffectFixedUpdate(SkillCaster skillCaster, Skill skill, float dt)
        {
            base.EffectFixedUpdate(skillCaster, skill, dt);
            if (isCasting == false) { return; }
        }

        public override void EffectUpdate(SkillCaster skillCaster, Skill skill, float dt)
        {
            base.EffectUpdate(skillCaster, skill, dt);
            if (isCasting == false) { return; }
        }

        /// <summary>
        /// 自己调用，表示执行结束
        /// </summary>
        protected void EndCast()
        {
            isCasting = false;
        }

        public bool IsCasting()
        {
            return isCasting;
        }
    }
}

