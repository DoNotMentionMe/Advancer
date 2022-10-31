using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class Goods_ShieldLevelUp : Goods
    {
        public override bool IsUnlocked { get; set; } = false;

        [SerializeField] PlayerProperty playerProperty;

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            GameSaver.Instance.SaveDataEventCall(() =>
            {
                BayatGames.SaveGameFree.SaveGame.Save<bool>(nameof(Goods_ShieldLevelUp) + "_IsUnlocked", IsUnlocked);
                BayatGames.SaveGameFree.SaveGame.Save<int>(nameof(Goods_ShieldLevelUp) + "_CurrentLevel", CurrentLevel);
            });
        }

        protected override void GoodsFunction()
        {
            playerProperty.CountComboToGetShield = 10;
            playerProperty.CountComboToGetShield -= CurrentLevel;
        }
    }
}
