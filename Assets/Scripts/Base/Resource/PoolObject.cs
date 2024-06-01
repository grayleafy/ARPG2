using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对象池物体组件，挂载了它才能被PoolMgr.GetObj(GameObject prefab)创建
/// </summary>
public class PoolObject : MonoBehaviour
{
    /// <summary>
    /// 由PoolMgr创建时设置，作为自己销毁时的全名
    /// </summary>
    [SerializeField]
    private string fullName;
    public string FullName => fullName;
    

    public void SetFullName(string fullName)
    {
        this.fullName = fullName;
    }

    

    /// <summary>
    /// 将自己放入池中
    /// </summary>
    public void PushSelfInPool()
    {
        if (fullName == null)
        {
            throw new System.Exception("应该使用PoolMgr实例化该预制体");
        }
        PoolMgr.GetInstance().PushObj(gameObject);
    }
}
