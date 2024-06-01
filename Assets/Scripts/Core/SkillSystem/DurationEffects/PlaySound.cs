using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SkillSystem
{
    [Serializable]
    public class PlaySound : DurationEffect
    {
        public AudioClip clip;
        public GameObject parent;

        public override void EffectStart(SkillCaster skillCaster, Skill skill)
        {
            base.EffectStart(skillCaster, skill);
            AudioMgr.GetInstance().PlaySound(clip, parent, false);
            EndCast();
        }
    }
}

