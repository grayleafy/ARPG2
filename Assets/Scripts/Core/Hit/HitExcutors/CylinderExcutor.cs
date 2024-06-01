using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class CylinderExcutor : HitExcutor
{
    public float impactForce = 3f;
    public float radius = 1f;
    public float length = 3f;

    private RaycastHit[] raycastHits = new RaycastHit[10];

    public override void OnHitFixedUpdate(float dt)
    {
        base.OnHitFixedUpdate(dt);

        Vector3 dir = offsetRotation * transfrom.forward;
        Vector3 origin = transfrom.position + offsetPosition;
        int cnt = Physics.SphereCastNonAlloc(origin, radius, dir, raycastHits, length);
        for (int i = 0; i < cnt; i++)
        {
            HitInfo hitInfo = new HitInfo(hitConfig, ref raycastHits[i]);
            hitInfo.hitPosition = origin + dir.normalized * raycastHits[i].distance;
            hitInfo.hitImpact = dir.normalized * impactForce;
            Hit(hitInfo);
        }

        FinishSelf();
    }


#if UNITY_EDITOR
    public override void OnSceneViewUpdate(SceneView sceneView)
    {
        for (float a = 0; a < 360; a += 5)
        {
            float x = MathF.Cos(a / 180 * Mathf.PI) * radius;
            float y = MathF.Sin(a / 180 * Mathf.PI) * radius;
            Vector3 origin = transfrom.TransformPoint(offsetRotation * new Vector3(x, y, 0)) + offsetPosition;
            Vector3 dir = offsetRotation * transfrom.forward;
            Vector3 end = origin + dir.normalized * length;
            Debug.DrawLine(origin, end);
        }
        FinishSelf();
    }
#endif
}
