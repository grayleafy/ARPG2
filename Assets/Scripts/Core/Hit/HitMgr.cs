using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class HitMgr : SingletonMono<HitMgr>
{
    [Serializable]
    public class HitPair
    {
        [SerializeField]
        public HitSourceType hitSourceType;
        [SerializeField]
        public HitTargetType hitTargetType;

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj is HitPair == false) { return false; }
            return hitSourceType == (obj as HitPair).hitSourceType && hitTargetType == (obj as HitPair).hitTargetType;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hitSourceType.GetHashCode();
                hash = hash * 23 + hitTargetType.GetHashCode();
                return hash;
            }
        }
    }

    [SerializeField]
    SerializableDictionary<HitPair, HitPerformer> hitPerformerDic = new();

    /// <summary>
    /// 根据攻击和受击材质，获取对应的表现
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="targetType"></param>
    /// <returns></returns>
    public HitPerformer GetHitPerformer(HitSourceType sourceType, HitTargetType targetType)
    {
        HitPair hitPair = new HitPair()
        {
            hitSourceType = sourceType,
            hitTargetType = targetType
        };
        if (hitPerformerDic.ContainsKey(hitPair))
        {
            return hitPerformerDic[hitPair];
        }
        return null;
    }
}
