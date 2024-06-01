using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class SkillEffect
{
    public string description;
    public virtual void EffectStart(SkillCaster skillCaster, Skill skill)
    {
        if (description != "" && description != null)
        {
            //Debug.Log("开始执行:" + description);
        }
    }

    /// <summary>
    /// 被打断时不一定执行该函数
    /// </summary>
    /// <param name="skillCaster"></param>
    /// <param name="skill"></param>
    public virtual void EffectEnd(SkillCaster skillCaster, Skill skill)
    {

    }

    public virtual void EffectUpdate(SkillCaster skillCaster, Skill skill, float dt)
    {

    }

    public virtual void EffectFixedUpdate(SkillCaster skillCaster, Skill skill, float dt)
    {

    }
}
