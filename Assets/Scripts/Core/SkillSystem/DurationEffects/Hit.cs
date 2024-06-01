using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SkillSystem
{
    [Serializable]
    public class Hit : DurationEffect
    {
        
        [SerializeReference, SubclassSelector]
        [DrawHitDetectRange]
        public HitExcutor hitExcutor;
        public override void EffectStart(SkillCaster skillCaster, Skill skill)
        {
            base.EffectStart(skillCaster, skill);
            hitExcutor.OnHitStart();
        }

        public override void EffectEnd(SkillCaster skillCaster, Skill skill)
        {
            base.EffectEnd(skillCaster, skill);
            hitExcutor.OnHitEnd();
        }

        public override void EffectUpdate(SkillCaster skillCaster, Skill skill, float dt)
        {
            base.EffectUpdate(skillCaster, skill, dt);
            hitExcutor.OnHitUpdate(dt);
            if (hitExcutor.IsHiting == false)
            {
                EndCast();
            }
        }

        public override void EffectFixedUpdate(SkillCaster skillCaster, Skill skill, float dt)
        {
            base.EffectFixedUpdate(skillCaster, skill, dt);
            hitExcutor.OnHitFixedUpdate(dt);
            if (hitExcutor.IsHiting == false)
            {
                EndCast();
            }
        }
    }
}

