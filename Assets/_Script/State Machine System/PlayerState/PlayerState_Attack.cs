using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerState_Attack : PlayerState
    {

        private int currentAxesX;
        private int currentAxesY;

        public override void Enter()
        {
            base.Enter();

            currentAxesX = playerInput.axesX;

            if (currentAxesX != 0)
            {
                playerController.AttackStart(playerInput.axesX);
            }
            else
            {
                playerController.AttackStart((int)playerController.LocalScaleX);
            }
        }

        public override void PhysicUpdate()
        {
            base.PhysicUpdate();

            //playerController.MoveX((int)playerController.LocalScaleX, 0.3f);

            if (stateFixedFrameCount >= 10)
            {
                playerController.AttackEnd();
                FSM.SwitchState(typeof(PlayerState_Idle));
            }
        }
    }
}