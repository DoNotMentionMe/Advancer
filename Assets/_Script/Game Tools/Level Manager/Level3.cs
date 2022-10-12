using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class Level3 : BaseLevelModule
    {
        public override string Key => nameof(Level3);

        protected override void ReleaseEnemyEvent()
        {
            throw new System.NotImplementedException();
        }

        protected override void RunAfterEnemysDied()
        {
            throw new System.NotImplementedException();
        }
    }
}