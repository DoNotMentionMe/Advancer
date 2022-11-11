using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Adv
{
    public class CanPlaySFX : Conditional
    {
        public override TaskStatus OnUpdate()
        {
            if (AudioManager.Instance.canSFX)
                return TaskStatus.Success;
            else
                return TaskStatus.Failure;
        }
    }
}
