using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerState : IState
    {
        protected float StateDuration => Time.time - stateStartTime;

        protected const string Idle = "Idle";
        protected const string RightAttackStart = "RightAttackStart";
        protected const string RightAttack = "RightAttack";
        protected const string RightAttackEnd = "RightAttackEnd";
        protected const string UpAttackStart = "UpAttackStart";
        protected const string UpAttack = "UpAttack";
        protected const string UpAttackEnd = "UpAttackEnd";

        protected PlayerController playerController;
        protected PlayerInput playerInput;
        protected PlayerFSM FSM;
        protected Animator anim;
        protected Animator anim2;
        protected GameObject anim2Obj;

        protected float stateFixedFrameCount = 0;

        protected float stateStartTime;

        public virtual void Initialize(
                                PlayerController playerController,
                                PlayerInput playerInput,
                                Animator anim,
                                Animator anim2,
                                GameObject anim2Obj,
                                PlayerFSM FSM)
        {
            this.playerController = playerController;
            this.playerInput = playerInput;
            this.anim = anim;
            this.anim2 = anim2;
            this.anim2Obj = anim2Obj;
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