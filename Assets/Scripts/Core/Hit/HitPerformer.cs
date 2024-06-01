using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[Serializable]
public class HitParticle
{
    public GameObject particlePrefab;
    public enum ParentType
    {
        World,
        HitGameObject,
        SourceEntity
    }
    [Tooltip("粒子的空间坐标系的父亲")]
    public ParentType parentType;
    public enum OrientationType
    {
        HitNormal,
        HitImpact
    }
    public OrientationType orientationType;
    public Vector3 positionOffset;
    public Quaternion rotationOffset;

    /// <summary>
    /// 实例化粒子效果
    /// </summary>
    /// <param name="hitInfo"></param>
    public void InstantiateParticle(HitInfo hitInfo)
    {
        PoolMgr.GetInstance().GetObj(particlePrefab, (go) =>
        {
            //父空间
            if (parentType == ParentType.World)
            {

            }
            else if (parentType == ParentType.HitGameObject)
            {
                go.transform.SetParent(hitInfo.hitGameObject.transform);
            }
            else if (parentType == ParentType.SourceEntity)
            {
                go.transform.SetParent(hitInfo.hitConfig.hitSource.transform);
            }

            //位置和朝向
            go.transform.position = hitInfo.hitPosition + positionOffset;
            if (orientationType == OrientationType.HitNormal)
            {
                go.transform.rotation = Quaternion.LookRotation(hitInfo.hitNormal, Vector3.up);
            }
            else if (orientationType != OrientationType.HitImpact)
            {
                go.transform.rotation = Quaternion.LookRotation(hitInfo.hitImpact, Vector3.up);
            }
            go.transform.rotation = rotationOffset * go.transform.rotation;
        });
    }
}


/// <summary>
/// 根据攻击和受击双方的类型，决定受击表现
/// </summary>
[Serializable]
public class HitPerformer
{
    public AudioClip hitSound;
    public List<HitParticle> hitParticles = new();
    

    /// <summary>
    /// 播放对应的表现
    /// </summary>
    /// <param name="hitInfo"></param>
    public void Play(HitInfo hitInfo)
    {
        //播放音效
        AudioMgr.GetInstance().PlaySound(hitSound, hitInfo.hitGameObject, false);
        //创建粒子效果
        for (int i = 0; i < hitParticles.Count; i++)
        {
            hitParticles[i].InstantiateParticle(hitInfo);
        }
        
    }
}
