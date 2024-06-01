using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffController : MonoBehaviour
{
    public Entity entity;
    [SerializeReference, SubclassSelector]
    public List<Buff> buffs = new();



    private void Reset()
    {
        entity = GetComponent<Entity>();
    }

    private void OnEnable()
    {
        foreach (Buff buff in buffs)
        {
            buff.OnEnable(this);
        }
    }

    private void OnDisable()
    {
        foreach (Buff buff in buffs)
        {
            buff.OnDisable(this);
        }
    }

    private void Update()
    {
        var temp = buffs.ToArray();
        foreach (Buff buff in temp)
        {
            buff.OnUpdate(this, Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        var temp = buffs.ToArray();
        foreach (Buff buff in temp)
        {
            buff.OnFixedUpdate(this, Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// 挂载buff的拷贝
    /// </summary>
    /// <param name="buff"></param>
    public void AddBuff(Buff buff, Entity source)
    {
        //深拷贝
        buff = DeepCloner.GetInstance().DeepCopy(buff);
        buff.source = source;

        //如果存在同名buff则叠加或刷新
        var exsitBuff = FindBuff(buff.name);
        if (exsitBuff != null)
        {
            exsitBuff.OnStack(this, buff);
        }
        //否则添加buff
        else
        {
            buffs.Add(buff);
            buff.OnEnable(this);
        }

    }
    /// <summary>
    /// 移出buff
    /// </summary>
    /// <param name="buff"></param>
    public void RemoveBuff(Buff buff)
    {
        buffs.Remove(buff);
        buff.OnDisable(this);
    }

    private Buff FindBuff(string name)
    {
        foreach (Buff buff in buffs)
        {
            if (buff.name == name)
            {
                return buff;
            }
        }
        return null;
    }


    #region 粒子特效相关
    private Dictionary<string, GameObject> particles = new();
    private Dictionary<string, int> particleCounts = new();
    /// <summary>
    /// 在entity物体下添加buff粒子效果
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="name"></param>
    public void AddParticle(string abName, string name)
    {
        string fullName = abName + "." + name;
        if (particleCounts.ContainsKey(fullName) == false)
        {
            particleCounts[fullName] = 0;
        }
        particleCounts[fullName]++;
        if (particleCounts[fullName] == 1)
        {
            PoolMgr.GetInstance().GetObj(abName, name, (go) =>
            {
                if (particles.ContainsKey(fullName) == true)
                {
                    RemoveParticle(abName, name);
                }
                particles[fullName] = go;
                go.transform.SetParent(gameObject.transform, false);
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;
            });
        }
    }

    /// <summary>
    /// 在entity物体下移除buff粒子效果
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="name"></param>
    public void RemoveParticle(string abName, string name)
    {
        string fullName = abName + "." + name;
        if (particleCounts.ContainsKey(fullName) == false)
        {
            particleCounts[fullName] = 0;
        }
        particleCounts[fullName]--;
        if (particleCounts[fullName] == 0)
        {
            PoolMgr.GetInstance().PushObj(abName, name, particles[fullName]);

            particles.Remove(fullName);
        }
    }

    #endregion
}
