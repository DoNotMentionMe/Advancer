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

        public override void LoadData()
        {
            if (GameSaver.Instance.Exists(nameof(Goods_AttackCanBreak) + "_IsUnlocked"))
            {
                IsUnlocked = GameSaver.Instance.Load<bool>(nameof(Goods_AttackCanBreak) + "_IsUnlocked");
                CheckBugCount();
            }
            if (GameSaver.Instance.Exists(nameof(Goods_AttackCanBreak) + "_CurrentLevel"))
                CurrentLevel = GameSaver.Instance.Load<int>(nameof(Goods_AttackCanBreak) + "_CurrentLevel");

            base.LoadData();
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
            //playerController.SetAttackStartTime();
        }
    }
}
