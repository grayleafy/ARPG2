using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DrawRequestValue))]
public class RequestValueDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);

        RequestValue requestValue = property.managedReferenceValue as RequestValue;
        Type type = requestValue.GetType();
        Type genericType = type.GetGenericArguments()[0];
        
        //EditorGUILayout.ObjectField(10, )
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label);
    }
}
