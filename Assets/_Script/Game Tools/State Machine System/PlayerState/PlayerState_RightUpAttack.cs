using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerState_RightUpAttack : PlayerState
    {
        private int StartCombo = 0;
        private float AttackStartTime;//攻击前摇
        private float EffectiveAttackTime;
        private float AttackEndTime;//攻击后摇
        private float AttackStateStartTime;
        private float AttackStateDurationTime => Time.time - AttackStateStartTime;
        private AttackStates attackState;

        public enum AttackStates
        { Start, Ing, End, Not }

        public override void Enter()
        {
            base.Enter();

            StartCombo = PlayerProperty.Combo;

            //改变方向
            playerController.ChangeScale(1);//右

            AttackStartTime = playerController.AttackStartTime;
            EffectiveAttackTime = playerController.EffectiveAttackTime;
            AttackEndTime = playerController.AttackEndTime;

            if (attackState == AttackStates.Not)
            {
                AttackStateStartTime = Time.time;
                anim2Obj.SetActive(true);
                anim.Play(RightAttackStart);
                anim2.Play(UpAttackStart);
                attackState = AttackStates.Start;
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            //状态管理
            if (attackState == AttackStates.Start && AttackStateDurationTime >= AttackStartTime)
            {
                anim.Play(RightAttack);
                if (anim2Obj.activeSelf)
                    anim2.Play(UpAttack);
                attackState = AttackStates.Ing;
                playerController.AttackStart(3);//右
                playerController.AttackStart(2);
                AttackStateStartTime = Time.time;//重置时间
            }
            else if (attackState == AttackStates.Ing && AttackStateDurationTime >= EffectiveAttackTime)
            {
                anim.Play(RightAttackEnd);
                if (anim2Obj.activeSelf)
                    anim2.Play(UpAttackEnd);
                attackState = AttackStates.End;
                playerController.AttackEnd(3);
                playerController.AttackEnd(2);
                AttackStateStartTime = Time.time;//重置时间
            }
            else if (attackState == AttackStates.End && AttackStateDurationTime >= AttackEndTime)
            {
                anim.Play(Idle);
                if (anim2Obj.activeSelf)
                    anim2.Play(Idle);
                anim2Obj.SetActive(false);
                attackState = AttackStates.Not;

                if (StartCombo == PlayerProperty.Combo)
                    PlayerProperty.ResetCombo();

                // if (ICache)
                // {
                //     ICache = false;
                //     if (cacheType == CacheType.Left)
                //         FSM.SwitchState(typeof(PlayerState_LeftAttack));
                //     else if (cacheType == CacheType.Up)
                //         FSM.SwitchState(typeof(PlayerState_UpAttack));
                //     else if (cacheType == CacheType.Right)
                //         FSM.SwitchState(typeof(PlayerState_RightAttack));
                // }
                // else
                FSM.SwitchState(typeof(PlayerState_Idle));
            }
        }
    }
}