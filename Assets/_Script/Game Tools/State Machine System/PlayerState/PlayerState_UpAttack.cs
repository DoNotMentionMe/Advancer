using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerState_UpAttack : PlayerState
    {
        private bool ICache = false;
        private int StartCombo = 0;
        private float AttackStartTime;//攻击前摇
        private float EffectiveAttackTime;
        private float AttackEndTime;//攻击后摇
        private float AttackStateStartTime;
        private float AttackStateDurationTime => Time.time - AttackStateStartTime;
        private AttackStates attackState;
        private CacheType cacheType;

        public enum AttackStates
        { Start, Ing, End, Not }

        public enum CacheType
        { Left, Up, Right }

        public override void Enter()
        {
            base.Enter();

            playerController.UpAttack.Broadcast();

            ICache = false;
            StartCombo = PlayerProperty.Combo;

            AttackStartTime = playerController.AttackStartTime;
            EffectiveAttackTime = playerController.EffectiveAttackTime;
            AttackEndTime = playerController.AttackEndTime;

            if (attackState == AttackStates.Not)
            {
                AttackStateStartTime = Time.time;
                anim.Play(UpAttackStart);
                attackState = AttackStates.Start;
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            //指令缓存 只在关闭AttackCanBreak情况有有效
            if (playerController.DoubleAttack && StateDuration < playerController.DoubleAttackEffectiveTime)
            {
                if (playerInput.Right)
                {
                    anim.Play(Idle);
                    attackState = AttackStates.Not;
                    playerController.AttackEnd(2);
                    FSM.SwitchState(typeof(PlayerState_RightUpAttack));
                }
            }
            else if (!playerController.DoubleAttack)
            {
                if (playerInput.Left && attackState == AttackStates.End)
                {
                    ICache = true;
                    playerInput.Left = false;
                    cacheType = CacheType.Left;
                }
                else if (playerInput.Right && attackState == AttackStates.End)
                {
                    ICache = true;
                    playerInput.Right = false;
                    cacheType = CacheType.Right;
                }
            }
            if (playerInput.Up && attackState == AttackStates.End)
            {
                ICache = true;
                playerInput.Up = false;
                cacheType = CacheType.Up;
            }

            //状态管理
            if (attackState == AttackStates.Start && AttackStateDurationTime >= AttackStartTime)
            {
                anim.Play(UpAttack);
                attackState = AttackStates.Ing;
                playerController.AttackStart(2);//右
                AttackStateStartTime = Time.time;//重置时间
            }
            else if (attackState == AttackStates.Ing && AttackStateDurationTime >= EffectiveAttackTime)
            {
                anim.Play(UpAttackEnd);
                attackState = AttackStates.End;
                playerController.AttackEnd(2);
                AttackStateStartTime = Time.time;//重置时间
            }
            else if (attackState == AttackStates.End && AttackStateDurationTime >= AttackEndTime)
            {
                anim.Play(Idle);
                attackState = AttackStates.Not;

                //没有命中
                if (StartCombo == PlayerProperty.Combo)
                {
                    PlayerProperty.NotEmptyAttackCurrentLevel = false;
                    PlayerProperty.ResetCombo();
                }

                if (ICache)
                {
                    ICache = false;
                    if (cacheType == CacheType.Left)
                        FSM.SwitchState(typeof(PlayerState_LeftAttack));
                    else if (cacheType == CacheType.Up)
                        FSM.SwitchState(typeof(PlayerState_UpAttack));
                    else if (cacheType == CacheType.Right)
                        FSM.SwitchState(typeof(PlayerState_RightAttack));
                }
                else
                    FSM.SwitchState(typeof(PlayerState_Idle));
            }

            //能力-打断攻击
            if (!playerController.AttackCanBreak) return;
            if (playerInput.Left)
            {
                attackState = AttackStates.Not;
                playerInput.Left = false;
                FSM.SwitchState(typeof(PlayerState_LeftAttack));
            }
            else if (playerInput.Up)
            {
                attackState = AttackStates.Not;
                playerInput.Up = false;
                FSM.SwitchState(typeof(PlayerState_UpAttack));
            }
            else if (playerInput.Right)
            {
                attackState = AttackStates.Not;
                playerInput.Right = false;
                FSM.SwitchState(typeof(PlayerState_RightAttack));
            }
        }
    }
}