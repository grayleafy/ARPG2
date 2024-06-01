using FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SkillSystem
{
    [Serializable]
    public class SetBlackBoard : DurationEffect
    {
        [SerializeReference, SubclassSelector]
        InspectorBlackboardValue value;

        public override void EffectStart(SkillCaster skillCaster, Skill skill)
        {
            base.EffectStart(skillCaster, skill);
            skill.blackboard.SetValue(value.name, value.GetValue());
            EndCast();
        }
    }
}

