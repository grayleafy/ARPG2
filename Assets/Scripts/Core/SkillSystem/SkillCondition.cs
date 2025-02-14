﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class SkillCondition
{
    public abstract bool CheckCondition(Skill skill, SkillCaster caster);
}
