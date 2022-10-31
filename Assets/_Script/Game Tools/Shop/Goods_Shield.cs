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

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            GameSaver.Instance.SaveDataEventCall(() =>
            {
                BayatGames.SaveGameFree.SaveGame.Save<bool>(nameof(Goods_Shield) + "_IsUnlocked", IsUnlocked);
                BayatGames.SaveGameFree.SaveGame.Save<int>(nameof(Goods_Shield) + "_CurrentLevel", CurrentLevel);
            });
        }

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
                GameSaver.Instance.SaveAllData();
                return true;
            }
            return false;
        }
    }
}
