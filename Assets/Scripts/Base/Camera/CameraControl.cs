using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class CameraControl
{
    public virtual void Start()
    {

    }

    /// <summary>
    /// 当控制权切换到自己时
    /// </summary>
    /// <param name="currentCamera"></param>
    public virtual void OnSwitch(Camera currentCamera)
    {

    }

    //todo: dt需要吗，镜头应该减速吗？
    public virtual void Update()
    {

    }


}
