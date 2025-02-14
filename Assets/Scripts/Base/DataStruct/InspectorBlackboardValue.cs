﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FSM
{
    [Serializable]
    public abstract class InspectorBlackboardValue
    {
        public string name;
        public abstract object GetValue();
    }


    [Serializable]
    public class InspectorBlackboardValueEntity : InspectorBlackboardValue
    {
        public Entity value;
        public override object GetValue()
        {
            return value;
        }
    }

    [Serializable]
    public class InspectorBlackboardValueGameObject : InspectorBlackboardValue
    {
        public GameObject value;
        public override object GetValue()
        {
            return value;
        }
    }

    [Serializable]
    public class InspectorBlackboardValueTransform : InspectorBlackboardValue
    {
        public Transform value;
        public override object GetValue()
        {
            return value;
        }
    }

    [Serializable]
    public class InspectorBlackboardValueInt : InspectorBlackboardValue
    {
        public int value;
        public override object GetValue()
        {
            return value;
        }
    }

    [Serializable]
    public class InspectorBlackboardValueFloat : InspectorBlackboardValue
    {
        public float value;
        public override object GetValue()
        {
            return value;
        }
    }

    [Serializable]
    public class InspectorBlackboardValueString : InspectorBlackboardValue
    {
        public string value;
        public override object GetValue()
        {
            return value;
        }
    }

    [Serializable]
    public class InspectorBlackboardValueAnimationCurve : InspectorBlackboardValue
    {
        public AnimationCurve value;
        public override object GetValue()
        {
            return value;
        }
    }

    [Serializable]
    public class InspectorBlackboardValueMotionState : InspectorBlackboardValue
    {
        [SerializeReference, SubclassSelector]
        public MotionState value;
        public override object GetValue()
        {
            return value;
        }
    }
}

