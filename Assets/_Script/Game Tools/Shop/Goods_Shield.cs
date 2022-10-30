using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class Goods_Shield : Goods
    {
        public override bool IsUnlocked { get; set; } = true;
        [SerializeField] PlayerProperty playerProperty;

        protected override void GoodsFunction()
        {
            playerProperty.CanGetShield = true;
        }
    }
}
