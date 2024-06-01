using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillSystem;
[System.Serializable]
public class Skill
{
    [Header("技能描述")]
    public string skillName;
    public string skillDescription;
    public List<string> skillTags = new();

    [Header("主动释放")]
    public List<InputCommand> castCommands = new();
    //触发的按键
    [HideInInspector]
    public InputCommand triggerCommand;
    [SerializeReference, SubclassSelector]
    public List<SkillCondition> castConditions = new();
    [SerializeReference, SubclassSelector]
    public List<DurationEffect> castEffects = new();

    [Header("被打断后的状态复原")]
    [SerializeReference, SubclassSelector]
    public List<DurationEffect> interruptEffects = new();

    [Header("被动效果")]
    [SerializeReference, SubclassSelector]
    public List<ContinuousEffect> passiveEffects = new();



    [Header("变量黑板")]
    public InspectorBlackboard blackboard;



    #region 私有状态变量 
    private bool isCasting = false;
    private int currentCastingEffectIndex = -1;
    private Dictionary<DurationEffect, DurationEffect> nextInterruptEffectDic = new();
    private List<DurationEffect> runningInterruptEffects = new();
    #endregion


    public void SkillEnable(SkillCaster skillCaster)
    {
        foreach (var effects in passiveEffects)
        {
            effects.EffectStart(skillCaster, this);
        }

        //构建打断效果字典
        for (int i = 0; i < interruptEffects.Count; i++)
        {
            DurationEffect next = i + 1 < interruptEffects.Count ? interruptEffects[i + 1] : null;
            nextInterruptEffectDic[interruptEffects[i]] = next;
        }
    }

    public void SkillDisable(SkillCaster skillCaster)
    {
        foreach (var effects in passiveEffects)
        {
            effects.EffectEnd(skillCaster, this);
        }
        //打断
        Interrupt(skillCaster);
    }

    public void SkillUpdate(SkillCaster skillCaster, float dt)
    {
        foreach (var effects in passiveEffects)
        {
            effects.EffectUpdate(skillCaster, this, dt);
        }
        //主动效果更新
        if (isCasting && currentCastingEffectIndex < castEffects.Count)
        {
            castEffects[currentCastingEffectIndex].EffectUpdate(skillCaster, this, dt);
            while (castEffects[currentCastingEffectIndex].IsCasting() == false)
            {
                castEffects[currentCastingEffectIndex].EffectEnd(skillCaster, this);
                currentCastingEffectIndex++;
                if (currentCastingEffectIndex < castEffects.Count)
                {
                    castEffects[currentCastingEffectIndex].EffectStart(skillCaster, this);
                }
                else
                {
                    currentCastingEffectIndex = -1;
                    isCasting = false;
                    break;
                }
            }
        }
        //其它效果
        UpdateRunningEffect(runningInterruptEffects, skillCaster, dt);
    }

    public void SkillFixedUpdate(SkillCaster skillCaster, float dt)
    {
        foreach (var effects in passiveEffects)
        {
            effects.EffectFixedUpdate(skillCaster, this, dt);
        }
        //主动效果更新
        if (isCasting && currentCastingEffectIndex < castEffects.Count)
        {
            castEffects[currentCastingEffectIndex].EffectFixedUpdate(skillCaster, this, dt);
            while (castEffects[currentCastingEffectIndex].IsCasting() == false)
            {
                castEffects[currentCastingEffectIndex].EffectEnd(skillCaster, this);
                currentCastingEffectIndex++;
                if (currentCastingEffectIndex < castEffects.Count)
                {
                    castEffects[currentCastingEffectIndex].EffectStart(skillCaster, this);
                }
                else
                {
                    currentCastingEffectIndex = -1;
                    isCasting = false;
                    break;
                }
            }
        }
        //其它效果
        FixedUpdateRunningEffect(runningInterruptEffects, skillCaster, dt);
    }

    public void Cast(SkillCaster skillCaster)
    {
        isCasting = true;
        currentCastingEffectIndex = 0;
        if (currentCastingEffectIndex < castEffects.Count)
        {
            castEffects[currentCastingEffectIndex].EffectStart(skillCaster, this);
        }
        else
        {
            isCasting = false;
            currentCastingEffectIndex = -1;
        }
    }

    /// <summary>
    /// 打断该技能
    /// </summary>
    /// <param name="skillCaster"></param>
    public void Interrupt(SkillCaster skillCaster)
    {
        //Debug.Log("打断技能");
        //中断当前效果
        if (isCasting && currentCastingEffectIndex < castEffects.Count)
        {
            castEffects[currentCastingEffectIndex].EffectEnd(skillCaster, this);
        }

        isCasting = false;
        currentCastingEffectIndex = -1;

        //打断效果
        if (interruptEffects.Count > 0)
        {
            AddRunningEffect(runningInterruptEffects, interruptEffects[0], skillCaster);
        }
    }

    #region 效果运行列表的更新相关
    void AddRunningEffect(List<DurationEffect> runningList, DurationEffect durationEffect, SkillCaster skillCaster)
    {
        runningList.Add(durationEffect);
        durationEffect.EffectStart(skillCaster, this);
    }

    void RemoveRunningEffect(List<DurationEffect> runningList, DurationEffect durationEffect, SkillCaster skillCaster)
    {
        runningList.Remove(durationEffect);
        durationEffect.EffectEnd(skillCaster, this);
        if (nextInterruptEffectDic.ContainsKey(durationEffect) && nextInterruptEffectDic[durationEffect] != null)
        {
            AddRunningEffect(runningList, nextInterruptEffectDic[durationEffect], skillCaster);
        }
    }

    void UpdateRunningEffect(List<DurationEffect> runningList, SkillCaster skillCaster, float dt)
    {
        List<DurationEffect> endList = new();
        foreach (var effect in runningList)
        {
            effect.EffectUpdate(skillCaster, this, dt);
            if (effect.IsCasting() == false)
            {
                endList.Add(effect);
            }
        }
        foreach (var effect in endList)
        {
            RemoveRunningEffect(runningList, effect, skillCaster);
        }
    }
    void FixedUpdateRunningEffect(List<DurationEffect> runningList, SkillCaster skillCaster, float dt)
    {
        List<DurationEffect> endList = new();
        foreach (var effect in runningList)
        {
            effect.EffectFixedUpdate(skillCaster, this, dt);
            if (effect.IsCasting() == false)
            {
                endList.Add(effect);
            }
        }
        foreach (var effect in endList)
        {
            RemoveRunningEffect(runningList, effect, skillCaster);
        }
    }
    #endregion
    public bool IsCasting()
    {
        return isCasting;
    }

    public bool CanCast(SkillCaster skillCaster)
    {
        for (int i = 0; i < castConditions.Count; i++)
        {
            if (castConditions[i].CheckCondition(this, skillCaster) == false)
            {
                return false;
            }
        }
        return true;
    }
}
