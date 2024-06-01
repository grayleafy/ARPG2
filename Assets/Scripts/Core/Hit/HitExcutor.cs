using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public abstract class HitExcutor
{
    [Header("攻击基础配置")]
    public HitConfig hitConfig;
    [Header("攻击检测范围相关")]
    public Transform transfrom;
    public Vector3 offsetPosition = Vector3.zero;
    public Quaternion offsetRotation = Quaternion.identity;

    //保存临时变量,避免一个物体被多次击中
    private bool isHiting = false;
    private HashSet<IHittable> alreadyHits = new();

    public virtual void OnHitStart()
    {
        alreadyHits.Clear();
        isHiting = true;
    }

    public virtual void OnHitUpdate(float dt)
    {

    }

    public virtual void OnHitFixedUpdate(float dt)
    {

    }

    public virtual void OnHitEnd()
    {
        isHiting = false;
    }

    /// <summary>
    /// 击中物体时调用，执行攻击逻辑
    /// </summary>
    /// <param name="hitInfo"></param>
    /// <exception cref="NotImplementedException"></exception>
    protected void Hit(HitInfo hitInfo)
    {
        if (hitInfo.target == null) return;
        //过滤自己
        if (hitInfo.target == hitInfo.hitConfig.hitSource as IHittable) return;
        //避免多次击中同一个对象
        if (IsAlreadyHit(hitInfo.target)) return;
        alreadyHits.Add(hitInfo.target);

        //击中的表现，包括粒子，音效，震屏
        HitPerformer hitPerformer = HitMgr.GetInstance().GetHitPerformer(hitConfig.hitSourceType, hitInfo.target.HitTargetType);
        hitPerformer.Play(hitInfo);

        //击中后施加buff
        BuffController buffController = (hitInfo.target as IBuff)?.GetBuffController();
        if (buffController != null)
        {
            foreach (var buff in hitInfo.hitConfig.inflictedBuffs)
            {
                buffController.AddBuff(buff, hitInfo.hitConfig.hitSource);
            }
        }

        //镜头抖动
        CameraMgr.GetInstance().AddShakeRequest(hitInfo.hitConfig.shakeRequest);

        //协程顿帧,顿帧结束后调用受击者的受击事件
        if (hitConfig.FreezeFrameDuration > 0)
        {
            (hitInfo.target as Entity).AddTimeScaleRequest(0.01f);
            hitInfo.hitConfig.hitSource.AddTimeScaleRequest(0.01f);
            //一段时间后结束顿帧，并且执行受击效果
            RuntimeTimer.GetInstance().WaitForInvoke(hitConfig.FreezeFrameDuration, () =>
            {
                (hitInfo.target as Entity).RemoveTimeScaleRequest(0.01f);
                hitInfo.hitConfig.hitSource.RemoveTimeScaleRequest(0.01f);

                hitInfo.target.OnHit(hitInfo);
            });
        }
        else
        {
            hitInfo.target.OnHit(hitInfo);
        }
    }

    /// <summary>
    /// 是否在本次攻击中已经击中过
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private bool IsAlreadyHit(IHittable target)
    {
        if (alreadyHits.Contains(target)) return true;
        return false;
    }

    public bool IsHiting => isHiting;
    /// <summary>
    /// 自己调用，结束攻击或者绘制
    /// </summary>
    protected void FinishSelf()
    {
        isHiting = false;
    }



#if UNITY_EDITOR

    /// <summary>
    /// 在场景视图上显示范围，函数内使用debug.drawline绘制,结束后使用finishSelf表示一次绘制完成
    /// </summary>
    public abstract void OnSceneViewUpdate(SceneView sceneView);


    /// <summary>
    /// 在场景视图上显示范围，初始化
    /// </summary>
    /// <param name="sceneView"></param>
    public virtual void OnSceneViewStart(SceneView sceneView)
    {
        isHiting = true;
    }

#endif



}
