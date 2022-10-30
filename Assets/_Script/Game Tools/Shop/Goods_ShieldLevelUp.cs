using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class Goods_ShieldLevelUp : Goods
    {
        public override bool IsUnlocked { get; set; } = false;

        [SerializeField] PlayerProperty playerProperty;

        protected override void GoodsFunction()
        {
            playerProperty.CountComboToGetShield = 10;
            playerProperty.CountComboToGetShield -= CurrentLevel;
        }
    }
}
