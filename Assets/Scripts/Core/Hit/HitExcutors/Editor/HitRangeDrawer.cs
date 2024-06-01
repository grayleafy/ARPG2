using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DrawHitDetectRange))]
public class HitRangeDrawer : PropertyDrawer
{
    public string controlName = "攻击范围绘制";
    HitExcutor hitExcutor;
    SerializedProperty property;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        this.property = property;
        Rect rect = position;
        rect.yMin = rect.yMin + 20;
        rect.yMax = rect.yMin + 20;
        bool b1 = GUI.Button(rect, "显示范围");
        rect.yMin = rect.yMax;
        rect.yMax = rect.yMin + 20;
        bool b2 = GUI.Button(rect, "隐藏范围");
        rect.yMin = rect.yMax;
        EditorGUI.PropertyField(rect, property, label, true);
        




        hitExcutor = property.managedReferenceValue as HitExcutor;
        if (b1)
        {
            //GUI.SetNextControlName(controlName);
            hitExcutor.OnSceneViewStart(SceneView.currentDrawingSceneView);
            SceneView.duringSceneGui += DrawRange;
            KeepSceneActive(60);
            property.isExpanded = false;
            property.isExpanded = true;
            SceneView.RepaintAll();
        }
        if (b2)
        {
            //GUI.SetNextControlName(controlName);
            property.isExpanded = false;
            property.isExpanded = true;
            SceneView.duringSceneGui -= DrawRange;
            SceneView.RepaintAll();
        }


    }


    void DrawRange(SceneView sceneView)
    {
        hitExcutor.OnSceneViewUpdate(sceneView);
        //GUI.SetNextControlName(controlName);
        try
        {
            if (property.isExpanded == false || hitExcutor.IsHiting == false)
            {
                SceneView.duringSceneGui -= DrawRange;
            }
        }
        catch (Exception)
        {
            SceneView.duringSceneGui -= DrawRange;
        }
    }

    double leftTime;
    /// <summary>
    /// 保存场景视图活跃一段时间
    /// </summary>
    /// <param name="duration"></param>
    void KeepSceneActive(float duration)
    {
        leftTime = duration;
        EditorApplication.update += KeepSceneActiveReally;
    }
    void KeepSceneActiveReally()
    {
        SceneView.RepaintAll();
        leftTime -= EditorTimer.GetInstance().EditorDeltaTime;
        if (leftTime <= 0)
        {
            EditorApplication.update -= KeepSceneActiveReally;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true) + 60;
    }
}
