using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 战斗击中信息
/// </summary>
[Serializable]
public class HitConfig
{
    public Entity hitSource;
    [SerializeReference, SubclassSelector]
    public Damage damage;
    public DamageType damageType;
    public HitSourceType hitSourceType;
    [Tooltip("顿帧持续时间")]
    public float FreezeFrameDuration = 0;
    [Tooltip("镜头抖动")]
    public ShakeRequest shakeRequest;

    [Header("施加buff")]
    [SerializeReference, SubclassSelector]
    public List<Buff> inflictedBuffs = new();
}
