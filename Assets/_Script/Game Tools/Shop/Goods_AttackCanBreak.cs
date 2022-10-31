using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class Goods_AttackCanBreak : Goods
    {
        public override bool IsUnlocked { get; set; } = false;

        [SerializeField] PlayerController playerController;
        [SerializeField] Level4 level4;

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            GameSaver.Instance.SaveDataEventCall(() =>
            {
                BayatGames.SaveGameFree.SaveGame.Save<bool>(nameof(Goods_AttackCanBreak) + "_IsUnlocked", IsUnlocked);
                BayatGames.SaveGameFree.SaveGame.Save<int>(nameof(Goods_AttackCanBreak) + "_CurrentLevel", CurrentLevel);
            });
            //读档
            // if (level4.IsPassed)
            // {
            //     IsUnlocked = true;
            //     shop.CheckAllLevelIsUnLocked();
            // }
        }

        protected override void GoodsFunction()
        {
            playerController.AttackCanBreak = true;
        }
    }
}
