using FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SkillSystem
{
    /// <summary>
    /// 技能未释放状态持续一段时间后设置黑板变量
    /// </summary>
    [Serializable]
    public class SetBlackboardWhenNotCasting : ContinuousEffect
    {
        public float afterTime;
        [SerializeReference, SubclassSelector]
        public InspectorBlackboardValue blackboardValue;

        float durationTime = 0;

        public override void EffectUpdate(SkillCaster skillCaster, Skill skill, float dt)
        {
            base.EffectUpdate(skillCaster, skill, dt);
            if (skill.IsCasting() == false)
            {
                durationTime += dt;
                if (durationTime > afterTime)
                {
                    skill.blackboard.SetValue(blackboardValue.name, blackboardValue.GetValue());
                }
            }
            else
            {
                durationTime = 0;
            }
        }
    }
}

