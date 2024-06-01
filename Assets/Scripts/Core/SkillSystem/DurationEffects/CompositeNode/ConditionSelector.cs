using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SkillSystem
{



    /// <summary>
    /// 根据条件顺序选择其中一个节点执行
    /// </summary>
    [Serializable]
    public class ConditionSelector : CompositeEffectBase
    {
        [Serializable]
        public class ConditionEffect
        {
            public string description;
            [SerializeReference, SubclassSelector]
            public SkillCondition condition;
            [SerializeReference, SubclassSelector]
            public DurationEffect effect;
        }


        public List<ConditionEffect> conditionEffects = new();
        private DurationEffect selectEffect;

        public override void EffectStart(SkillCaster skillCaster, Skill skill)
        {
            base.EffectStart(skillCaster, skill);
            selectEffect = null;
            for (int i = 0; i < conditionEffects.Count; i++)
            {
                if (conditionEffects[i].condition.CheckCondition(skill, skillCaster))
                {
                    selectEffect = conditionEffects[i].effect;
                    break;
                }
            }
            if (selectEffect == null)
            {
                EndCast();
            }
            else
            {
                CastChildEffect(selectEffect, skillCaster, skill);
            }
        }

        public override void EffectUpdate(SkillCaster skillCaster, Skill skill, float dt)
        {
            base.EffectUpdate(skillCaster, skill, dt);
            if (IsChildEffectCasting() == false)
            {
                EndCast();
            }
        }

        public override void EffectFixedUpdate(SkillCaster skillCaster, Skill skill, float dt)
        {
            base.EffectFixedUpdate(skillCaster, skill, dt);
            if (IsChildEffectCasting() == false)
            {
                EndCast();
            }
        }
    }
}

