using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class Level2 : BaseLevelModule
    {
        public override string Key => nameof(Level2);

        [SerializeField] GameObject BOSS01;
        [SerializeField] GameObject 通关界面;
        private bool FirstLevel2Success = true;

        protected override void ReleaseEnemyEvent()
        {
            InstantiateEnemy(BOSS01);
        }

        protected override void RunAfterEnemysDied()
        {
            if (FirstLevel2Success)
            {
                FirstLevel2Success = false;
                通关界面.SetActive(true);
            }
        }
    }
}