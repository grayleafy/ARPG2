using FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 共享变量黑板
/// </summary>
[Serializable]
public class InspectorBlackboard
{
    private Dictionary<string, object> dic = new();    //共享黑板

    //[Header("共享变量黑板")]
    [SerializeReference, SubclassSelector]
    [Header("使用下列变量预先初始化黑板")]
    private List<InspectorBlackboardValue> initValues = new();
    private bool isInitialized = false;

    /// <summary>
    /// 设置一个黑板变量
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public void SetValue<T>(string name, T value)
    {
        if (isInitialized == false) Init();
        if (dic.ContainsKey(name))
        {
            dic[name] = value;
        }
        else
        {
            dic.Add(name, value);
        }
    }

    /// <summary>
    /// 获取一个黑板变量，如果是值类型并且不存在则返回默认值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public T GetValue<T>(string name)
    {
        if (isInitialized == false) Init();
        if (dic.ContainsKey(name))
        {
            return (T)dic[name];
        }
        else
        {
            return default(T);
        }
    }

    /// <summary>
    /// 可以调用该函数在检视器中预先设置初始需要的共享变量
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    public void AddInitNeedValue<T>(string name)
    {
        string className = typeof(InspectorBlackboardValue).FullName + typeof(T).Name;
        try
        {
            Type type = Type.GetType(className);
            InspectorBlackboardValue backgroundValue = Activator.CreateInstance(type) as InspectorBlackboardValue;
            backgroundValue.name = name;
            initValues.Add(backgroundValue);
        }
        catch (Exception e)
        {
            Debug.LogError("添加黑板变量[" + name + "]失败，请先实现BlackboardValue对应类型的派生, " + e.Message);
        }
    }

    private void Init()
    {
        isInitialized = true;
        foreach (var value in initValues)
        {
            dic.Add(value.name, value.GetValue());
        }
    }
}
