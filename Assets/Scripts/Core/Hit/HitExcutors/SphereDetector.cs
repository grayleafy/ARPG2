using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[Serializable]
public class SphereDetector : HitExcutor
{
    public float radius = 1;
    public float impactForce = 3f;

    private Collider[] colliders = new Collider[10];

    public override void OnHitStart()
    {
        base.OnHitStart();

        Vector3 origin = transfrom.position + offsetPosition;
        int cnt = Physics.OverlapSphereNonAlloc(origin, radius, colliders);
        for (int i = 0; i < cnt; i++)
        {
            IHittable hitTarget = HitInfo.GetEntityByCollider(colliders[i]) as IHittable;
            if (hitTarget != null)
            {
                HitInfo hitInfo = new HitInfo();
                hitInfo.hitConfig = hitConfig;
                hitInfo.hitImpact = (colliders[i].gameObject.transform.position - hitConfig.hitSource.transform.position).normalized * impactForce;
                hitInfo.hitPosition = colliders[i].transform.position;
                hitInfo.hitNormal = -(colliders[i].gameObject.transform.position - hitConfig.hitSource.transform.position).normalized;
                hitInfo.hitGameObject = colliders[i].gameObject;
                hitInfo.target = hitTarget;

                Hit(hitInfo);
            }

        }

        FinishSelf();
    }


#if UNITY_EDITOR
    public override void OnSceneViewUpdate(SceneView sceneView)
    {
        Vector3 origin = transfrom.position + offsetPosition;

        for (float a = 0; a <= 360; a += 5)
        {
            for (float b = 0; b <= 360; b += 5)
            {
                Vector3 dir = Vector3.forward;
                dir = Quaternion.Euler(a, b, 0) * dir;
                dir *= radius;
                Vector3 end = transfrom.TransformPoint(offsetRotation * dir) + offsetPosition;
                Debug.DrawLine(origin, end);
            }
        }

        FinishSelf();
    }
#endif
}


