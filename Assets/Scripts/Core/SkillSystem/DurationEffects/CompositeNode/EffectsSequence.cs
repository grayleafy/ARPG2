using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SkillSystem
{
    /// <summary>
    /// 顺序执行所有节点
    /// </summary>
    [Serializable]
    public class EffectsSequence : CompositeEffectBase
    {
        [SerializeReference, SubclassSelector]
        public List<DurationEffect> effects = new();
        int effectIndex = 0;

        public override void EffectStart(SkillCaster skillCaster, Skill skill)
        {
            base.EffectStart(skillCaster, skill);
            effectIndex = 0;
            if (effectIndex < effects.Count)
            {
                CastChildEffect(effects[effectIndex], skillCaster, skill);
            }
        }

        public override void EffectEnd(SkillCaster skillCaster, Skill skill)
        {
            base.EffectEnd(skillCaster, skill);
            effectIndex = 0;
        }

        public override void EffectUpdate(SkillCaster skillCaster, Skill skill, float dt)
        {
            base.EffectUpdate(skillCaster, skill, dt);
            if (IsChildEffectCasting() == false)
            {
                effectIndex++;
                if (effectIndex < effects.Count)
                {
                    CastChildEffect(effects[effectIndex], skillCaster, skill);
                }
                else
                {
                    EndCast();
                }
            }
        }

        public override void EffectFixedUpdate(SkillCaster skillCaster, Skill skill, float dt)
        {
            base.EffectFixedUpdate(skillCaster, skill, dt);
            if (IsChildEffectCasting() == false)
            {
                effectIndex++;
                if (effectIndex < effects.Count)
                {
                    CastChildEffect(effects[effectIndex], skillCaster, skill);
                }
                else
                {
                    EndCast();
                }
            }
        }
    }
}

