using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class People : Creature , IHittable
{
    [Header("受击相关")]
    public float hitTime = 0.8f;
    public HitTargetType HitTargetType => HitTargetType.Organi;


    public override void EntityStart()
    {
        base.EntityStart();
        AnimatorComponent.SetFloat("HumanScale", AnimatorComponent.humanScale);
        AnimatorComponent.SetFloat("HumanScaleInverse", 1.0f / AnimatorComponent.humanScale);
    }


    public virtual void OnHit(HitInfo hitInfo)
    {
        //受击动画
        AnimatorComponent.SetTrigger("Hit");
        //冲击力
        AddForceByTargetVelocity(hitInfo.hitImpact);
        //输入禁用
        InputRecorderComponent.AddDisableRequest();
        RuntimeTimer.GetInstance().WaitForInvoke(hitTime, () => InputRecorderComponent.RemoveDisableRequest());
        //伤害计算
        CharacterStats.TakeDamage(hitInfo);
    }
}
