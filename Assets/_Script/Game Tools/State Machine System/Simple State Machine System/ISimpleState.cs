using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public interface ISimpleState
    {
        void Enter();
        void Update();
        void FixedUpdate();
        void Exit();
    }

    public class CustomState : ISimpleState
    {
        #region 动作
        private Action mOnEnter;
        private Action mOnUpdate;
        private Action mOnFixedUpdate;
        private Action mOnExit;
        #endregion

        #region 注册动作
        public CustomState OnEnter(Action onEnter)
        {
            mOnEnter = onEnter;
            return this;
        }

        public CustomState OnUpdate(Action onUpdate)
        {
            mOnUpdate = onUpdate;
            return this;
        }

        public CustomState OnFixedUpdate(Action onFixedUpdate)
        {
            mOnFixedUpdate = onFixedUpdate;
            return this;
        }

        public CustomState OnExit(Action onExit)
        {
            mOnExit = onExit;
            return this;
        }
        #endregion

        #region 执行动作
        public void Enter()
        {
            mOnEnter?.Invoke();
        }

        public void Update()
        {
            mOnUpdate?.Invoke();
        }

        public void FixedUpdate()
        {
            mOnFixedUpdate?.Invoke();
        }

        public void Exit()
        {
            mOnExit?.Invoke();
        }
        #endregion
    }

    public class SimpleFSM<T>
    {
        public Dictionary<T, ISimpleState> mStates = new Dictionary<T, ISimpleState>();

        public CustomState State(T t)
        {
            if (mStates.ContainsKey(t))
            {
                return mStates[t] as CustomState;
            }

            var state = new CustomState();
            mStates.Add(t, state);
            return state;
        }

        private ISimpleState mCurrentState;
        private T mCurrentStateId;

        public ISimpleState CurrentState => mCurrentState;
        public T CurrentStateId => mCurrentStateId;

        private bool stateStarted = false;

        public void StartState(T t)
        {
            if (mStates.TryGetValue(t, out var state))
            {
                stateStarted = false;
                mCurrentStateId = t;
                mCurrentState = state;
                mCurrentState.Enter();
                stateStarted = true;
            }
        }

        public void ChangeState(T t)
        {
            if (mStates.TryGetValue(t, out var state))
            {
                if (mCurrentState != null)
                {
                    stateStarted = false;
                    mCurrentState.Exit();
                    mCurrentStateId = t;
                    mCurrentState = state;
                    mCurrentState.Enter();
                    stateStarted = true;
                }
            }
        }

        public void FixedUpdate()
        {
            if (stateStarted)
                mCurrentState?.FixedUpdate();
        }

        public void Update()
        {
            if (stateStarted)
                mCurrentState?.Update();
        }

        public void Clear()
        {
            mCurrentState = null;
            mCurrentStateId = default;
            mStates.Clear();
        }
    }

    // public class StateExample
    // {
    //     //Key
    //     public enum States
    //     {
    //         A,
    //         B,
    //         C
    //     }

    //     void Example()
    //     {
    //         var fsm = new SimpleFSM<States>();
    //         fsm.State(States.A)
    //             .OnEnter(() =>
    //             {

    //             })
    //             .OnUpdate(() =>
    //             {

    //             })
    //             .OnFixedUpdate(() =>
    //             {

    //             })
    //             .OnExit(() =>
    //             {

    //             });

    //         fsm.StartState(States.A);
    //     }
    // }
}