using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 可以回收入对象池的实体，调用PushSelfInPool（）
/// </summary>
public class PoolEntity : Entity
{
    public string abName;
    public string prefabName;

    /// <summary>
    /// 将自己放入对象池
    /// </summary>
    public void PushSelfInPool()
    {
        PoolMgr.GetInstance().PushObj(abName, prefabName, this.gameObject);
    }
}
