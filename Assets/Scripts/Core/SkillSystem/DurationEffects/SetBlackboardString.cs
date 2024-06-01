using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SkillSystem
{
    [Serializable]
    public class SetBlackboardString : DurationEffect
    {
        public string valueName;
        public string value;

        public override void EffectStart(SkillCaster skillCaster, Skill skill)
        {
            base.EffectStart(skillCaster, skill);
            skill.blackboard.SetValue<string>(valueName, value);
            EndCast();
        }
    }
}

