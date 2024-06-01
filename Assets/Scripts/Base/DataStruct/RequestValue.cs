using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestValue
{

}


/// <summary>
/// 一种特殊的变量，可以由多个外部使用者调用，返回对应优先级的最终叠加结果
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public class RequestValue<T> : RequestValue
{
    private List<T> requests = new();
    [SerializeField]
    private T currentResult;
    private Func<T, T, T> prirotyFunc;   //优先级判断的方法

    /// <summary>
    /// 传入叠加优先级的方法以及默认值
    /// </summary>
    /// <param name="prirotyFunc"></param>
    /// <param name="defaultValue"></param>
    public RequestValue(Func<T, T, T> prirotyFunc, T defaultValue)
    {
        this.prirotyFunc = prirotyFunc;
        AddRequest(defaultValue);
    }

    public void AddRequest(T value)
    {
        requests.Add(value);
        UpdateCurrentResult();
    }

    public void RemoveRequest(T value)
    {
        requests.Remove(value);
        UpdateCurrentResult();
    }

    public T GetResult()
    {
        return currentResult;
    }

    private void UpdateCurrentResult()
    {
        T result = requests[0];
        for (int i = 1; i < requests.Count; i++)
        {
            result = prirotyFunc(result, requests[i]);
        }
        currentResult = result;
    }
}
