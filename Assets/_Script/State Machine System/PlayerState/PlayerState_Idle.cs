using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerState_Idle : PlayerState
    {
        public override void Enter()
        {
            base.Enter();

            //暂时
            //playerController.Stop();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            // if (playerInput.axesX != 0)
            // {
            //     FSM.SwitchState(typeof(PlayerState_Move));
            // }
            // else 
            // if (playerInput.Attack)
            // {
            //     FSM.SwitchState(typeof(PlayerState_Attack));
            // }

            if (playerInput.Left)
            {
                FSM.SwitchState(typeof(PlayerState_LeftAttack));
                playerInput.Left = false;
            }

            if (playerInput.Up)
            {
                FSM.SwitchState(typeof(PlayerState_UpAttack));
                playerInput.Up = false;
            }

            if (playerInput.Right)
            {
                FSM.SwitchState(typeof(PlayerState_RightAttack));
                playerInput.Right = false;
            }


        }
    }
}