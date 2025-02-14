﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;




/// <summary>
/// UI管理器
/// 1.管理所有显示的面板
/// 2.提供给外部 显示和隐藏等等接口
/// </summary>
public class UIMgr : SingletonBase<UIMgr>
{
    private Dictionary<string, BasePanel> uniquePanelDic = new Dictionary<string, BasePanel>();      //静态面板字典
    private Dictionary<UILayer, RectTransform> uiLayers = new();

    //记录我们UI的Canvas父对象 方便以后外部可能会使用它
    public RectTransform canvas;
    public string uiAbName = "ui";
    public string canvasName = "Canvas";
    public string eventSystemName = "EventSystem";

    public UIMgr()
    {
        if (canvas == null)
        {
            GameObject obj = GameObject.Find(canvasName);
            if (obj == null || obj.GetComponent<Canvas>() == null)
            {
                //创建Canvas 让其过场景的时候 不被移除
                obj = GameObject.Instantiate(ABMgr.GetInstance().LoadRes<GameObject>(uiAbName, canvasName));
                //obj.name = "Canvas";
            }
            canvas = obj.transform as RectTransform;
            GameObject.DontDestroyOnLoad(obj);

            //找到各层
            foreach (UILayer uiLayer in Enum.GetValues(typeof(UILayer)))
            {
                GameObject go = canvas.gameObject.FindInFirstChildren(uiLayer.ToString());
                if (go == null)
                {
                    go = new GameObject(uiLayer.ToString());
                }
                var rect = go.AddComponent<RectTransform>();
                rect.SetParent(canvas);
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.one;
                //rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width);
                //rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height);
                rect.offsetMax = Vector2.zero;
                rect.offsetMin = Vector2.zero;
                rect.localScale = Vector3.one;
                uiLayers[uiLayer] = rect;
            }


            //创建EventSystem 让其过场景的时候 不被移除
            obj = GameObject.Find(eventSystemName);
            if (obj == null || obj.GetComponent<EventSystem>() == null)
            {
                obj = GameObject.Instantiate(ABMgr.GetInstance().LoadRes<GameObject>(uiAbName, eventSystemName));
            }
            GameObject.DontDestroyOnLoad(obj);
        }
    }

    /// <summary>
    /// 通过层级枚举 得到对应层级的父对象
    /// </summary>
    /// <param name="layer"></param>
    /// <returns></returns>
    public RectTransform GetLayerFather(UILayer layer)
    {
        if (uiLayers.ContainsKey(layer))
        {
            return uiLayers[layer];
        }
        return null;
    }

    ///// <summary>
    ///// 切换面板显隐
    ///// </summary>
    ///// <typeparam name="T">面板脚本类型</typeparam>
    ///// <param name="panelName">面板名</param>
    ///// <param name="layer">显示在哪一层</param>
    ///// <param name="callBack">当面板预设体创建成功后 你想做的事</param>
    //public void SwitchPanel<T>(string panelName, UILayer layer = UILayer.Mid, UnityAction<T> callBack = null) where T : BasePanel
    //{
    //    if (GetPanel<T>(panelName) != null)
    //    {
    //        HidePanel<T>(panelName);
    //    }
    //    else
    //    {
    //        ShowPanel<T>(panelName, layer, callBack);
    //    }
    //}

    /// <summary>
    /// 显示静态单例面板
    /// </summary>
    /// <typeparam name="T">面板脚本类型</typeparam>
    /// <param name="panelName">面板名</param>
    /// <param name="layer">显示在哪一层</param>
    /// <param name="callBack">当面板预设体创建成功后 你想做的事</param>
    public void ShowPanel<T>(string panelName, UnityAction<T> callBack = null) where T : BasePanel
    {
        //如果是单例类并且已存在，要求单例面板和其它面板不能重名
        if (uniquePanelDic.ContainsKey(panelName))
        {
            uniquePanelDic[panelName].gameObject.SetActive(true);
            uniquePanelDic[panelName].OnShow();
            // 处理面板创建完成后的逻辑
            if (callBack != null)
                callBack(uniquePanelDic[panelName] as T);
            //避免面板重复加载 如果存在该面板 即直接显示 调用回调函数后  直接return 不再处理后面的异步加载逻辑
            return;
        }

        PoolMgr.GetInstance().GetObj(uiAbName, panelName, (obj) =>
        {
            //单例则判断是否有短时间内重复加载行为
            if (obj.GetComponent<T>().uniquePanel)
            {
                if (uniquePanelDic.ContainsKey(panelName))
                {
                    PoolMgr.GetInstance().PushObj(uiAbName, panelName, obj);
                    return;
                }
            }

            //把他作为 Canvas的子对象
            //并且 要设置它的相对位置
            //找到父对象 你到底显示在哪一层
            obj.name = panelName;
            //得到预设体身上的面板脚本
            T panel = obj.GetComponent<T>();
            Transform father = GetLayerFather(panel.uILayer);
            //设置父对象  设置相对位置和大小
            obj.transform.SetParent(father, false);

            //把面板存起来
            if (panel.uniquePanel)
            {
                uniquePanelDic.Add(panelName, panel);
            }

            panel.OnShow();

            // 处理面板创建完成后的逻辑
            if (callBack != null)
                callBack(panel);
        });
    }

    //    /// <summary>
    //    /// 打开动态面板
    //    /// </summary>
    //    /// <typeparam name="T">面板脚本类型</typeparam>
    //    /// <param name="panelName">面板名</param>
    //    /// <param name="layer">显示在哪一层</param>
    //    /// <param name="callBack">当面板预设体创建成功后 你想做的事</param>
    //    public void OpenDynamicPanel<T>(string panelName, UILayer layer = UILayer.Mid, UnityAction<T> callBack = null) where T : BasePanel
    //    {

    //        PoolMgr.GetInstance().GetObj("ui", panelName, (obj) =>
    //        {
    //            //把他作为 Canvas的子对象
    //            //并且 要设置它的相对位置
    //            //找到父对象 你到底显示在哪一层
    //            Transform father = bot;
    //            switch (layer)
    //            {
    //                case UILayer.Mid:
    //                    father = mid;
    //                    break;
    //                case UILayer.Top:
    //                    father = top;
    //                    break;
    //                case UILayer.System:
    //                    father = system;
    //                    break;
    //            }
    //            //设置父对象  设置相对位置和大小
    //            obj.transform.SetParent(father, false);

    //            //obj.transform.localPosition = Vector3.zero;
    //            //obj.transform.localScale = Vector3.one;

    //            //(obj.transform as RectTransform).offsetMax = Vector2.zero;
    //            //(obj.transform as RectTransform).offsetMin = Vector2.zero;

    //            //得到预设体身上的面板脚本
    //            T panel = obj.GetComponent<T>();
    //            // 处理面板创建完成后的逻辑
    //            if (callBack != null)
    //                callBack(panel);

    //            panel.OnShow();
    //        });
    //    }

    //    /// <summary>
    //    /// 关闭动态面板
    //    /// </summary>
    //    /// <param name="panelName"></param>
    //    public void CloseDynamicPanel<T>(T panel, string panelName) where T : BasePanel
    //    {
    //        PoolMgr.GetInstance().PushObj("ui", panelName, panel.gameObject);
    //    }

    /// <summary>
    /// 隐藏单例面板
    /// </summary>
    /// <param name="panelName"></param>
    public void HidePanel<T>(string panelName) where T : BasePanel
    {
        if (uniquePanelDic.ContainsKey(panelName))
        {
            uniquePanelDic[panelName].OnHide();
            uniquePanelDic.Remove(panelName);
            if (uniquePanelDic[panelName].destroyOnHide == false)
            {
                PoolMgr.GetInstance().PushObj(uiAbName, panelName, uniquePanelDic[panelName].gameObject);
            }
            else
            {
                GameObject.Destroy(uniquePanelDic[panelName].gameObject);
            }
        }
    }

    public void HidePanel<T>(T panel) where T : BasePanel
    {
        panel.OnHide();
        if (uniquePanelDic.ContainsKey(panel.gameObject.name))
        {
            uniquePanelDic.Remove(panel.gameObject.name);
        }
        if (panel.destroyOnHide == false)
        {
            PoolMgr.GetInstance()?.PushObj(uiAbName, panel.gameObject.name, panel.gameObject);
        }
        else
        {
            GameObject.Destroy(panel.gameObject);
        }

        
    }

    //    /// <summary>
    //    /// 得到某一个已经显示的面板 方便外部使用 
    //    /// </summary>
    //    public T GetPanel<T>(string name) where T : BasePanel
    //    {
    //        if (uniquePanelDic.ContainsKey(name))
    //            return uniquePanelDic[name] as T;
    //        return null;
    //    }

    //    /// <summary>
    //    /// 给控件添加自定义事件监听
    //    /// </summary>
    //    /// <param name="control">控件对象</param>
    //    /// <param name="type">事件类型</param>
    //    /// <param name="callBack">事件的响应函数</param>
    //    public static void AddCustomEventListener(UIBehaviour control, EventTriggerType type, UnityAction<BaseEventData> callBack)
    //    {
    //        EventTrigger trigger = control.GetComponent<EventTrigger>();
    //        if (trigger == null)
    //            trigger = control.gameObject.AddComponent<EventTrigger>();

    //        EventTrigger.Entry entry = new EventTrigger.Entry();
    //        entry.eventID = type;
    //        entry.callback.AddListener(callBack);

    //        trigger.triggers.Add(entry);
    //    }

}