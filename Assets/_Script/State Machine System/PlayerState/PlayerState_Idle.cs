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
                playerController.AttackStart(1);
            else
                playerController.AttackEnd(1);

            if (playerInput.Up)
                playerController.AttackStart(2);
            else
                playerController.AttackEnd(2);

            if (playerInput.Right)
                playerController.AttackStart(3);
            else
                playerController.AttackEnd(3);


        }
    }
}