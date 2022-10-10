using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerState_RightAttack : PlayerState
    {
        private float AttackStartTime;//攻击前摇
        private float EffectiveAttackTime;
        private float AttackEndTime;//攻击后摇
        private AttackStates attackState;

        public enum AttackStates
        {
            Start, Ing, End, Not
        }

        public override void Enter()
        {
            base.Enter();

            //改变方向
            playerController.ChangeScale(1);//右

            AttackStartTime = playerController.AttackStartTime;
            EffectiveAttackTime = playerController.EffectiveAttackTime;
            AttackEndTime = playerController.AttackEndTime;

            if (attackState == AttackStates.Not)
            {
                anim.Play(RightAttackStart);
                attackState = AttackStates.Start;
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (attackState == AttackStates.Start && StateDuration >= AttackStartTime)
            {
                anim.Play(RightAttack);
                attackState = AttackStates.Ing;
                playerController.AttackStart(3);//右
                stateStartTime = Time.time;//重置时间
            }
            else if (attackState == AttackStates.Ing && StateDuration >= EffectiveAttackTime)
            {
                anim.Play(RightAttackEnd);
                attackState = AttackStates.End;
                playerController.AttackEnd(3);
                stateStartTime = Time.time;//重置时间
            }
            else if (attackState == AttackStates.End && StateDuration >= AttackEndTime)
            {
                anim.Play(Idle);
                attackState = AttackStates.Not;
                FSM.SwitchState(typeof(PlayerState_Idle));
            }
        }
    }
}