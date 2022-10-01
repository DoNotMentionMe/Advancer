using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerState : IState
    {
        protected float StateDuration => Time.time - stateStartTime;

        protected PlayerController playerController;
        protected PlayerInput playerInput;
        protected PlayerFSM FSM;

        protected float stateFixedFrameCount = 0;

        private float stateStartTime;

        public virtual void Initialize(
                                PlayerController playerController,
                                PlayerInput playerInput,
                                PlayerFSM FSM)
        {
            this.playerController = playerController;
            this.playerInput = playerInput;
            this.FSM = FSM;
        }

        public virtual void Enter()
        {
            stateStartTime = Time.time;
            stateFixedFrameCount = 0;
        }

        public virtual void Exit()
        {
        }

        public virtual void LogicUpdate()
        {
        }

        public virtual void PhysicUpdate()
        {
            stateFixedFrameCount += 1;//1秒50帧，一帧0.02秒
        }

        private void OnDestroy()
        {
            FSM = null;
        }
    }
}