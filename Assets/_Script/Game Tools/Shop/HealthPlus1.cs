using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class HealthPlus1 : Goods
    {
        public override bool IsUnlocked { get; set; } = true;
        [SerializeField] PlayerProperty playerProperty;

        protected override void GoodsFunction()
        {
            playerProperty.MaxHealthPlus1();
        }
    }
}
