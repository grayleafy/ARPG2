using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    /// <summary>
    /// 复合节点基类
    /// </summary>
    [Serializable]
    public abstract class CompositeEffectBase : DurationEffect
    {
        List<DurationEffect> runningEffects = new();

        public override void EffectStart(SkillCaster skillCaster, Skill skill)
        {
            base.EffectStart(skillCaster, skill);
            runningEffects.Clear();
        }

        public override void EffectEnd(SkillCaster skillCaster, Skill skill)
        {
            base.EffectEnd(skillCaster, skill);
            for (int i = 0; i < runningEffects.Count; i++)
            {
                runningEffects[i].EffectEnd(skillCaster, skill);
            }
            runningEffects.Clear();
        }

        public override void EffectUpdate(SkillCaster skillCaster, Skill skill, float dt)
        {
            base.EffectUpdate(skillCaster, skill, dt);
            for (int i = 0; i < runningEffects.Count; i++)
            {
                runningEffects[i].EffectUpdate(skillCaster, skill, dt);
                if (runningEffects[i].IsCasting() == false)
                {
                    runningEffects[i].EffectEnd(skillCaster, skill);
                    runningEffects.RemoveAt(i);
                    i--;
                }
            }
        }

        public override void EffectFixedUpdate(SkillCaster skillCaster, Skill skill, float dt)
        {
            base.EffectFixedUpdate(skillCaster, skill, dt);
            for (int i = 0; i < runningEffects.Count; i++)
            {
                runningEffects[i].EffectFixedUpdate(skillCaster, skill, dt);
                if (runningEffects[i].IsCasting() == false)
                {
                    runningEffects[i].EffectEnd(skillCaster, skill);
                    runningEffects.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// 开始释放一个子效果,开始、更新和结束由Composite节点接管，不用再处理该子效果
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="skillCaster"></param>
        /// <param name="skill"></param>
        protected void CastChildEffect(DurationEffect effect, SkillCaster skillCaster, Skill skill)
        {
            effect.EffectStart(skillCaster, skill);
            runningEffects.Add(effect);
        }

        /// <summary>
        /// 执行队列中是否有子效果还在执行
        /// </summary>
        /// <returns></returns>
        protected bool IsChildEffectCasting()
        {
            for (int i = 0; i < runningEffects.Count; i++)
            {
                if (runningEffects[i].IsCasting()) return true;
            }
            return false;
        }
    }
}

