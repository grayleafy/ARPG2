using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    /// <summary>
    /// 从对象池加载一个物体
    /// </summary>
    [Serializable]
    public class SpawnPoolGameObject : DurationEffect
    {
        public string abName;
        public string prefabName;

        public Transform parent;
        public Vector3 localPosition;
        public Quaternion localRotation;
        public Vector3 localScale = new Vector3(1, 1, 1);

        public override void EffectStart(SkillCaster skillCaster, Skill skill)
        {
            base.EffectStart(skillCaster, skill);
            //var go = ABMgr.GetInstance().LoadRes<GameObject>(abName, prefabName);
            //go = GameObject.Instantiate(go);
            //go.transform.SetParent(parent);
            //go.transform.localPosition = localPosition;
            //go.transform.localRotation = localRotation;
            //go.transform.localScale = localScale;

            PoolMgr.GetInstance().GetObj(abName, prefabName, (obj) =>
            {
                obj.transform.SetParent(parent);
                obj.transform.localPosition = localPosition;
                obj.transform.localRotation = localRotation;
                obj.transform.localScale = localScale;
            });
            EndCast();
        }
    }
}

