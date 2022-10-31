using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class Goods_Shield : Goods
    {
        public override bool IsUnlocked { get; set; } = true;
        [SerializeField] PlayerProperty playerProperty;
        [SerializeField] Goods_ShieldLevelUp shieldLevelUp;

        protected override void GoodsFunction()
        {
            playerProperty.CanGetShield = true;
        }

        public override bool CheckBugCount()
        {
            if (CanDestroy && CurrentLevel >= DestroyCount)
            {
                BugButton.enabled = false;
                gameObject.SetActive(false);
                shieldLevelUp.IsUnlocked = true;
                return true;
            }
            return false;
        }
    }
}
