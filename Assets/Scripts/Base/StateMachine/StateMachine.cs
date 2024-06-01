using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{





    [Serializable]
    public abstract class StateMachineBase
    {
        [Header("共享变量黑板")]
        [SerializeField]
        private InspectorBlackboard blackboard = new();    //共享黑板



        public StateMachineBase()
        {
            SetBlackboardInitNeedValueList();//在检视器中显示需要的黑板变量
        }

        /// <summary>
        /// 设置一个黑板变量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetBlackboard<T>(string name, T value)
        {
            blackboard.SetValue<T>(name, value);
        }

        /// <summary>
        /// 获取一个黑板变量，如果是值类型并且不存在则返回默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T GetBlackboard<T>(string name)
        {
            return blackboard.GetValue<T>(name);
        }


        /// <summary>
        /// 派生类设置需要的黑板变量列表，以便在检视器中设置,在里面调用AddNeedBackgroundValue
        /// </summary>
        public virtual void SetBlackboardInitNeedValueList()
        {

        }

        /// <summary>
        /// 添加需要的黑板变量，在SetBackgroundValueNeedList中调用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        public void AddInitNeedValue<T>(string name)
        {
            blackboard.AddInitNeedValue<T>(name);
        }


    }

    [Serializable]
    public abstract class StateMachine<TState, TTrigger> : StateMachineBase where TState : StateBase where TTrigger : TriggerBase
    {

        Dictionary<string, TState> states = new();
        Dictionary<TState, Dictionary<TTrigger, TState>> triggersDic = new();
        [Header("当前状态")]
        [SerializeReference, SubclassSelector]
        public TState currentState;

        /// <summary>
        /// 状态机启动
        /// </summary>
        public void Start()
        {
            Build();
        }

        /// <summary>
        /// 建立状态机并且进入默认状态
        /// </summary>
        /// <exception cref="Exception"></exception>
        protected virtual void Build()
        {
            AddAllStates();
            AddAllTriggers();
            string defaultName = GetDefaultStateName();
            if (states.ContainsKey(defaultName) == false)
            {
                throw new Exception("默认状态不存在于状态字典中，请先设置正确的状态集合");
            }
            currentState = states[GetDefaultStateName()];
            currentState.EnterState(this);
        }


        public TState GetState(string name)
        {
            if (states.ContainsKey(name) == false)
            {
                return null;
            }
            return states[name];
        }

        public void AddState(string stateName, TState state)
        {
            states.Add(stateName, state);
            triggersDic.Add(state, new Dictionary<TTrigger, TState>());
        }

        public void AddTrigger(string sourceStateName, TTrigger trigger, string targetStateName)
        {
            if (states.ContainsKey(sourceStateName) == false || states.ContainsKey(targetStateName) == false)
            {
                throw new System.Exception("添加触发器失败，不含有对应状态");
            }
            triggersDic[states[sourceStateName]].Add(trigger, states[targetStateName]);
        }
        /// <summary>
        /// 调用AddState设置状态
        /// </summary>
        protected abstract void AddAllStates();
        /// <summary>
        /// 调用AddTrigger设置触发器
        /// </summary>
        protected abstract void AddAllTriggers();
        /// <summary>
        /// 设置默认状态名
        /// </summary>
        /// <returns></returns>
        protected abstract string GetDefaultStateName();

        /// <summary>
        /// 状态机更新
        /// </summary>
        /// <param name="dt"></param>
        public virtual void Update(float dt)
        {
            currentState.Update(this, dt);
            var nextState = CheckTriggers(currentState);
            if (nextState != null)
            {
                TransitionToState(nextState);
            }
        }

        /// <summary>
        /// 状态机固定频率更新
        /// </summary>
        /// <param name="dt"></param>
        public virtual void FixUpdate(float dt)
        {
            currentState.FixUpdate(this, dt);
            var nextState = FixCheckTriggers(currentState);
            if (nextState != null)
            {
                TransitionToState(nextState);
            }
        }

        /// <summary>
        /// 转换状态
        /// </summary>
        /// <param name="nextState"></param>
        public void TransitionToState(TState nextState)
        {
            currentState.ExitState(this);
            currentState = nextState;
            currentState.EnterState(this);
        }

        //检测状态的所有触发器是否有满足的
        TState CheckTriggers(TState state)
        {
            var triggers = triggersDic[state];
            foreach (var trigger in triggers.Keys)
            {
                if (trigger.CheckCondition(this) == true)
                {
                    return triggers[trigger];
                }
            }
            return null;
        }

        //检测状态的所有触发器是否有满足的
        TState FixCheckTriggers(TState state)
        {
            var triggers = triggersDic[state];
            foreach (var trigger in triggers.Keys)
            {
                if (trigger.FixCheckCondition(this) == true)
                {
                    return triggers[trigger];
                }
            }
            return null;
        }
    }
}


