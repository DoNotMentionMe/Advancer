using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class Goods_AttackCanBreak : Goods
    {
        public override bool IsUnlocked { get; set; } = false;

        [SerializeField] PlayerController playerController;

        protected override void GoodsFunction()
        {
            playerController.AttackCanBreak = true;
        }
    }
}
