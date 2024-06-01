using FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SkillSystem
{
    [Serializable]
    public class CheckBlackboard : SkillCondition
    {
        [SerializeReference, SubclassSelector]
        public InspectorBlackboardValue checkValue;
        public override bool CheckCondition(Skill skill, SkillCaster caster)
        {
            var t = checkValue.GetValue() .Equals( skill.blackboard.GetValue<object>(checkValue.name));
            return t;
        }
    }
}

