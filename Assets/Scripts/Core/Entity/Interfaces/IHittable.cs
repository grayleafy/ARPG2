using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 可被击中
/// </summary>
public interface IHittable
{
    /// <summary>
    /// 作为受击者的材质类型
    /// </summary>
    HitTargetType HitTargetType { get; }
    void OnHit(HitInfo hitInfo);
}
