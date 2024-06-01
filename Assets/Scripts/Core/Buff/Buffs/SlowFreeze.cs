using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BuffSystem
{
    /// <summary>
    /// 减速，达到一定层数后冻结并移除自己
    /// </summary>
    [Serializable]
    public class SlowFreeze : Buff
    {
        [Tooltip("减速百分比")]
        [Range(0f, 1f)]
        public float slowPercentage = 0.1f;

        public SlowFreeze()
        {
            name = "渐冻";
            particlePrefabAbName = "particle";
            particlePrefabName = "减速";
            tags.Add(Tag.MoveSpeedSlow);
        }

        public override void OnStack(BuffController buffController, Buff otherBuff)
        {
            base.OnStack(buffController, otherBuff);
            duration = MathF.Max(duration, otherBuff.duration);
            stackCount++;
            stackCount = Mathf.Min(3, stackCount);

            //减速叠层
            (buffController.entity as IStats).CharacterStats.ModifyAttribute(CharacterStats.ModifiableAttributeType.MoveSpeed, false, -slowPercentage, false);
            slowPercentage += (otherBuff as SlowFreeze).slowPercentage;
            (buffController.entity as IStats).CharacterStats.ModifyAttribute(CharacterStats.ModifiableAttributeType.MoveSpeed, false, -slowPercentage, true);

            //冰冻
            if (stackCount == 3)
            {
                buffController.AddBuff(new Freeze()
                {
                    duration = 3f
                }, otherBuff.source);
                duration = 0;
            }
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

