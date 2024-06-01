using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SkillSystem
{
    [Serializable]
    public class CheckBlackboardString : SkillCondition
    {
        public string valueName;
        public string value;
        public override bool CheckCondition(Skill skill, SkillCaster caster)
        {
            return skill.blackboard.GetValue<string>(valueName) == value;
        }
    }
}

