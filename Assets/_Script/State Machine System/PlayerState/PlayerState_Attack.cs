using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerState_Attack : PlayerState
    {

        public override void Enter()
        {
            base.Enter();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!playerInput.Attack)
            {
                FSM.SwitchState(typeof(PlayerState_Idle));
            }
        }
    }
}