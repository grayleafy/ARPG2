using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCaster : MonoBehaviour
{
    [SerializeField]
    List<Skill> skills = new();
    public Skill currentCastingSkill = null;

    public Entity entity;
    public InputRecorder inputRecorder;
    private void Reset()
    {
        entity = GetComponent<Entity>();
        inputRecorder = GetComponent<InputRecorder>();
    }

    private void OnEnable()
    {
        for (int i = 0; i < skills.Count; i++)
        {
            skills[i].SkillEnable(this);
        }
    }
    private void OnDisable()
    {
        for (int i = 0; i < skills.Count; i++)
        {
            skills[i].SkillDisable(this);
        }
    }


    private void Update()
    {
        for (int i = 0; i < skills.Count; i++)
        {
            skills[i].SkillUpdate(this, entity.TimeScale * Time.deltaTime);
        }

        if (currentCastingSkill != null && currentCastingSkill.IsCasting() == false) { currentCastingSkill = null; }
        TryCastNextSkill();
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < skills.Count; i++)
        {
            skills[i].SkillFixedUpdate(this, entity.TimeScale * Time.fixedDeltaTime);
        }
    }

    void TryCastNextSkill()
    {
        if (currentCastingSkill == null)
        {
            InputCommand command = inputRecorder?.DequeueInputCommand();
            if (command != null)
            {
                //匹配满足释放条件的技能
                for (int i = 0; i < skills.Count; i++)
                {
                    bool isCommandMatched = false;
                    for (int j = 0; j < skills[i].castCommands.Count; j++)
                    {
                        if (command.IsMatch(skills[i].castCommands[j]))
                        {
                            isCommandMatched = true;
                            break;
                        }
                    }
                    if (isCommandMatched && skills[i].CanCast(this))
                    {
                        CastSkill(skills[i], command);
                        //Debug.Log("释放技能");
                        break;
                    }
                }

            }
        }
    }

    public void CastSkill(Skill skill, InputCommand inputCommand)
    {
        InterruptAllSkills();
        currentCastingSkill = skill;
        skill.triggerCommand = inputCommand;
        skill.Cast(this);
    }

    /// <summary>
    /// 打断所有正在释放的技能
    /// </summary>
    public void InterruptAllSkills()
    {
        if (currentCastingSkill != null)
        {
            currentCastingSkill.Interrupt(this);
            currentCastingSkill = null;
        }
    }
}
