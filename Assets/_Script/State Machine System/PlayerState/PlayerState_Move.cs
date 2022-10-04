using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerState_Move : PlayerState
    {
        public override void Enter()
        {
            base.Enter();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            playerController.MoveX(playerInput.axesX, 1);

            if (playerInput.axesX == 0)
            {
                FSM.SwitchState(typeof(PlayerState_Idle));
            }
            else if (playerInput.Attack)
            {
                FSM.SwitchState(typeof(PlayerState_RightAttack));
            }
        }
    }
}