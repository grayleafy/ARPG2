using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SkillSystem
{
    [Serializable]
    public class CheckInputCommand : SkillCondition
    {
        public InputCommand inputCommand;
        public override bool CheckCondition(Skill skill, SkillCaster caster)
        {
            return skill.triggerCommand.IsMatch(inputCommand);
        }
    }
}

