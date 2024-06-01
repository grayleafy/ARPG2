
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class SectorExcutor : HitExcutor
{
    public float radius = 1;
    public float force = 3;
    [Range(-180f, 180f)]
    public float startAngle = 90;
    [Range(-360, 360), Tooltip("扇形范围，逆时针为正方向")]
    public float rangeAngle = -180;
    [Tooltip("单次攻击持续时间")]
    public float duration = 0.1f;
    [Range(0.1f, 180), Tooltip("射线检测间隔角度")]
    public float intervalAngle = 3;



    private RaycastHit[] raycastHits = new RaycastHit[5];
    float deltaAngle;
    int castTimes;
    int currentCastTime = 0;
    float currentDuration = 0;

    public override void OnHitStart()
    {
        base.OnHitStart();
        deltaAngle = rangeAngle > 0 ? intervalAngle : -intervalAngle;
        castTimes = (int)(Mathf.Abs(rangeAngle) / intervalAngle);
        currentCastTime = 0;
        currentDuration = 0;
    }

    public override void OnHitFixedUpdate(float dt)
    {
        base.OnHitFixedUpdate(dt);
        //计算时间和次数
        currentDuration += dt;
        int nextCastTime = (int)((float)castTimes * currentDuration / duration);
        nextCastTime = Mathf.Min(nextCastTime, castTimes);

        //多次射线检测
        Vector3 origin = transfrom.position + offsetPosition;
        for (int i = currentCastTime; i < nextCastTime; i++)
        {
            float a = startAngle + i * deltaAngle;
            Vector3 rayDirection = GetRayDirection(a);
            //todo:  Physics.BoxCastNonAlloc
            int count = Physics.RaycastNonAlloc(origin, rayDirection, raycastHits, radius);
            for (int j = 0; j < count; j++)
            {
                HitInfo hitInfo = new HitInfo(hitConfig, ref raycastHits[j]);
                hitInfo.hitImpact = rayDirection * force;
                Hit(hitInfo);
            }
        }
        currentCastTime = nextCastTime;

        //判断结束
        if (currentDuration > duration)
        {
            FinishSelf();
            return;
        }
    }

#if UNITY_EDITOR
    public override void OnSceneViewStart(SceneView sceneView)
    {
        base.OnSceneViewStart(sceneView);
        deltaAngle = rangeAngle > 0 ? intervalAngle : -intervalAngle;
        castTimes = (int)(Mathf.Abs(rangeAngle) / intervalAngle);
        currentCastTime = 0;
        currentDuration = 0;
    }

    public override void OnSceneViewUpdate(SceneView sceneView)
    {
        if (transfrom != null)
        {
            //计算时间和次数
            currentDuration += (float)SceneViewTimer.GetInstance().EditorDeltaTime;
            int nextCastTime = (int)((float)castTimes * currentDuration / duration);
            nextCastTime = Mathf.Min(nextCastTime, castTimes);

            //多次射线检测
            Vector3 origin = transfrom.position + offsetPosition;
            for (int i = currentCastTime; i < nextCastTime; i++)
            {
                float a = startAngle + i * deltaAngle;
                Vector3 rayDirection = GetRayDirection(a);
                Debug.DrawLine(origin, origin + rayDirection * radius, Color.red, 0.1f);
            }
            currentCastTime = nextCastTime;

            //判断结束
            if (currentDuration > duration)
            {
                FinishSelf();
                return;
            }
        }
    }

#endif

    //射线方向
    Vector3 GetRayDirection(float angle)
    {
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
        return transfrom.TransformDirection(offsetRotation * rotation * Vector3.forward);
    }



    //public List<HitInfo> HitDetect(HitConfig hitConfig)
    //{
    //    List<HitInfo> hitResults = new List<HitInfo>();
    //    HashSet<IHit> exists = new HashSet<IHit>();
    //    Vector3 origin = transfrom.position + offsetPosition;
    //    float a = startAngle;
    //    float da = rangeAngle > 0 ? intervalAngle : -intervalAngle;
    //    int t = (int)(rangeAngle / intervalAngle);
    //    for (int i = 0; i < t; i++)
    //    {
    //        Vector3 rayDirection = GetRayDirection(a);
    //        int count = Physics.RaycastNonAlloc(origin, rayDirection, raycastHits, radius);
    //        for (int j = 0; j < count; j++)
    //        {
    //            HitInfo hitInfo = GetHitInfoByCollider(hitConfig, ref raycastHits[j]);
    //            hitInfo.hitForce = rayDirection * force;
    //            if (hitInfo.target != null && exists.Contains(hitInfo.target) == false)
    //            {
    //                exists.Add(hitInfo.target);
    //                hitResults.Add(hitInfo);
    //            }
    //        }
    //        a += da;
    //    }
    //    return hitResults;
    //}
}
