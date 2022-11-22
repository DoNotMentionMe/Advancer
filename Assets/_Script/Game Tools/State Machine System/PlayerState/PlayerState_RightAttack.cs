using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerState_RightAttack : PlayerState
    {
        private bool ICache = false;
        private int StartCombo = 0;
        private float AttackStartTime;//攻击前摇
        private float EffectiveAttackTime;
        private float AttackEndTime;//攻击后摇
        private float AttackStateStartTime;
        private float AttackStateDurationTime => Time.time - AttackStateStartTime;
        private AttackStates attackState = AttackStates.Not;
        private CacheType cacheType;

        public enum AttackStates
        { Start, Ing, End, Not }

        public enum CacheType
        { Left, Up, Right }

        public override void Enter()
        {
            base.Enter();

            playerController.RightAttack.Broadcast();

            ICache = false;
            StartCombo = PlayerProperty.Combo;

            //改变方向
            playerController.ChangeScale(1);//右

            AttackStartTime = playerController.AttackStartTime;
            EffectiveAttackTime = playerController.EffectiveAttackTime;
            AttackEndTime = playerController.AttackEndTime;

            if (attackState == AttackStates.Not)
            {
                AttackStateStartTime = Time.time;
                anim.Play(RightAttackStart);
                attackState = AttackStates.Start;
                playerController.dust.StartDustRight();
                playerController.LeftAttackObj.StopAttackPauseCoroutine();
                Time.timeScale = 1;
                playerController.AttackEnd(3);
                playerController.AttackEnd(2);
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            //指令缓存 只在关闭AttackCanBreak情况有有效
            if (playerController.DoubleAttack && StateDuration < playerController.DoubleAttackEffectiveTime)
            {
                if (playerInput.Up)
                {
                    anim.Play(Idle);
                    attackState = AttackStates.Not;
                    playerController.AttackEnd(3);
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
                else if (playerInput.Up && attackState == AttackStates.End)
                {
                    ICache = true;
                    playerInput.Up = false;
                    cacheType = CacheType.Up;
                }
            }
            if (playerInput.Right && attackState == AttackStates.End)
            {
                ICache = true;
                playerInput.Right = false;
                cacheType = CacheType.Right;
            }

            //状态管理
            if (attackState == AttackStates.Start && AttackStateDurationTime >= AttackStartTime)
            {
                anim.Play(RightAttack);
                attackState = AttackStates.Ing;
                playerController.AttackStart(3);//右
                AttackStateStartTime = Time.time;//重置时间
            }
            else if (attackState == AttackStates.Ing && AttackStateDurationTime >= EffectiveAttackTime)
            {
                anim.Play(RightAttackEnd);
                attackState = AttackStates.End;
                playerController.AttackEnd(3);
                AttackStateStartTime = Time.time;//重置时间
                playerController.RightAttackObj.StopAttackPauseCoroutine();
                Time.timeScale = 1;
            }
            else if (attackState == AttackStates.End && AttackStateDurationTime >= AttackEndTime)
            {
                anim.Play(Idle);
                attackState = AttackStates.Not;

                if (StartCombo == PlayerProperty.Combo && StartCombo != 0)
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