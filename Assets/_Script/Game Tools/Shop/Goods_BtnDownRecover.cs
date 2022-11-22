using System.Collections;
using System.Collections.Generic;
using BayatGames.SaveGameFree;
using UnityEngine;

namespace Adv
{
    public class Goods_BtnDownRecover : Goods
    {
        public override bool IsUnlocked { get; set; } = false;

        [SerializeField] Shield shield;

        public override void LoadData()
        {
            if (GameSaver.Instance.Exists(nameof(Goods_BtnDownRecover) + "_IsUnlocked"))
            {
                IsUnlocked = GameSaver.Instance.Load<bool>(nameof(Goods_BtnDownRecover) + "_IsUnlocked");
                CheckBugCount();
            }
            if (GameSaver.Instance.Exists(nameof(Goods_BtnDownRecover) + "_CurrentLevel"))
                CurrentLevel = GameSaver.Instance.Load<int>(nameof(Goods_BtnDownRecover) + "_CurrentLevel");

            base.LoadData();
        }

        private void Start()
        {
            GameSaver.Instance.SaveDataEventCall(() =>
            {
                SaveGame.Save<bool>(nameof(Goods_BtnDownRecover) + "_IsUnlocked", IsUnlocked);
                SaveGame.Save<int>(nameof(Goods_BtnDownRecover) + "_CurrentLevel", CurrentLevel);
            });
        }

        protected override void GoodsFunction()
        {
            shield.canRecover = true;
            SaveGame.Save<bool>("canRecover", shield.canRecover);
        }

        public override bool CheckBugCount()
        {
            if (CanDestroy && CurrentLevel >= DestroyCount)
            {
                BugButton.enabled = false;
                gameObject.SetActive(false);
                return true;
            }
            return false;
        }
    }
}
