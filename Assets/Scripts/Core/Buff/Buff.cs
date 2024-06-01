using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[Serializable]
public abstract class Buff
{
    [HideInInspector]
    [Tooltip("buff来源")]
    public Entity source;

    public string name;
    public string description;
    public List<Tag> tags = new();
    [Tooltip("叠加层数")]
    public int stackCount = 1;
    public float duration = float.PositiveInfinity;
    [Header("特效预制件名称")]
    public string particlePrefabAbName = null;
    public string particlePrefabName = null;

    /// <summary>
    /// 相同名字buff叠加时的处理
    /// </summary>
    /// <param name="otherBuff"></param>
    public virtual void OnStack(BuffController buffController, Buff otherBuff)
    {

    }

    /// <summary>
    /// buff激活
    /// </summary>
    /// <param name="buffController"></param>
    public virtual void OnEnable(BuffController buffController)
    {
        if (particlePrefabAbName != null && particlePrefabName != null && particlePrefabAbName != "" && particlePrefabName != "")
        {
            buffController.AddParticle(particlePrefabAbName, particlePrefabName);
        }
    }
    /// <summary>
    /// buff结束
    /// </summary>
    /// <param name="buffController"></param>
    public virtual void OnDisable(BuffController buffController)
    {
        if (particlePrefabAbName != null && particlePrefabName != null && particlePrefabAbName != "" && particlePrefabName != "")
        {
            buffController.RemoveParticle(particlePrefabAbName, particlePrefabName);
        }
    }
    /// <summary>
    /// buff更新
    /// </summary>
    /// <param name="buffController"></param>
    public virtual void OnUpdate(BuffController buffController, float dt)
    {
        //持续时间到则移除自己
        if (duration != float.PositiveInfinity)
        {
            duration -= dt;
            if (duration <= 0)
            {
                buffController.RemoveBuff(this);
            }
        }
    }
    /// <summary>
    /// 物理相关更新
    /// </summary>
    /// <param name="buffController"></param>
    /// <param name="dt"></param>
    public virtual void OnFixedUpdate(BuffController buffController, float dt)
    {

    }

}
