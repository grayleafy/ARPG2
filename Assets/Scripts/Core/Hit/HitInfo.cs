using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HitInfo
{
    public IHittable target;
    public GameObject hitGameObject;
    public Vector3 hitPosition;
    public Vector3 hitNormal;
    public Vector3 hitImpact;
    public HitConfig hitConfig;

    public HitInfo() { }
    public HitInfo(HitConfig hitConfig, ref RaycastHit raycastHit)
    {
        this.hitConfig = hitConfig;
        hitGameObject = raycastHit.collider.gameObject;
        this.hitPosition = raycastHit.point;
        this.hitNormal = raycastHit.normal;
        this.target = GetEntityByCollider(raycastHit.collider) as IHittable;
    }

    /// <summary>
    /// 计算基础来源伤害值
    /// </summary>
    /// <returns></returns>
    public float GetBasicSourceDamageValue()
    {
        if (hitConfig.damage == null)   return 0f;
        CharacterStats characterStats = hitConfig.hitSource.GetComponent<CharacterStats>();
        return hitConfig.damage.GetBasicDamage(characterStats);
    }



    /// <summary>
    /// 根据碰撞体设置一个HitInfo,不会返回null但是target有可能为null
    /// </summary>
    /// <param name="hitConfig"></param>
    /// <param name="raycastHit"></param>
    /// <returns></returns>
    protected HitInfo GetHitInfoByCollider(HitConfig hitConfig, ref RaycastHit raycastHit)
    {
        HitInfo hitInfo = new HitInfo();
        hitInfo.hitConfig = hitConfig;
        hitInfo.hitPosition = raycastHit.point;
        hitInfo.hitNormal = raycastHit.normal;
        hitInfo.target = GetEntityByCollider(raycastHit.collider) as IHittable;
        return hitInfo;
    }

    /// <summary>
    /// 根据碰撞体，递归向父亲获取第一个entity
    /// </summary>
    /// <param name="colider"></param>
    /// <returns></returns>
    public static Entity GetEntityByCollider(Collider colider)
    {
        Transform t = colider.transform;
        while (t != null)
        {
            Entity entity = t.GetComponent<Entity>();
            if (entity != null)
            {
                return entity;
            }
            t = t.parent;
        }
        return null;
    }
}
