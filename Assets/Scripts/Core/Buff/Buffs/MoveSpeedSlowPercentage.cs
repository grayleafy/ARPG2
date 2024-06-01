using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BuffSystem
{
    /// <summary>
    /// 百分比移动速度减少
    /// </summary>
    [Serializable]
    public class MoveSpeedSlowPercentage : Buff
    {
        [Tooltip("减速百分比")]
        [Range(0f, 1f)]
        public float slowPercentage = 0.1f;

        public MoveSpeedSlowPercentage()
        {
            name = "减速";
            tags.Add(Tag.MoveSpeedSlow);
        }

        public override void OnStack(BuffController buffController, Buff otherBuff)
        {
            base.OnStack(buffController, otherBuff);
            duration = Mathf.Max(duration, otherBuff.duration);

        }

        public override void OnEnable(BuffController buffController)
        {
            base.OnEnable(buffController);
            (buffController.entity as IStats).CharacterStats.ModifyAttribute(CharacterStats.ModifiableAttributeType.MoveSpeed, false, -slowPercentage, true);
        }

        public override void OnDisable(BuffController buffController)
        {
            base.OnDisable(buffController);
            (buffController.entity as IStats).CharacterStats.ModifyAttribute(CharacterStats.ModifiableAttributeType.MoveSpeed, false, -slowPercentage, false);
        }
    }
}

