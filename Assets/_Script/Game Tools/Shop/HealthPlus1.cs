using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class HealthPlus1 : Goods
    {
        public override bool IsUnlocked { get; set; } = true;
        [SerializeField] PlayerProperty playerProperty;

        protected override void Awake()
        {
            base.Awake();
        }

        public override void LoadData()
        {
            if (GameSaver.Instance.Exists(nameof(HealthPlus1) + "_IsUnlocked"))
            {
                IsUnlocked = GameSaver.Instance.Load<bool>(nameof(HealthPlus1) + "_IsUnlocked");
                CheckBugCount();
            }
            if (GameSaver.Instance.Exists(nameof(HealthPlus1) + "_CurrentLevel"))
            {
                CurrentLevel = GameSaver.Instance.Load<int>(nameof(HealthPlus1) + "_CurrentLevel");
            }
            base.LoadData();
        }

        private void Start()
        {

            GameSaver.Instance.SaveDataEventCall(() =>
            {
                BayatGames.SaveGameFree.SaveGame.Save<bool>(nameof(HealthPlus1) + "_IsUnlocked", IsUnlocked);
                BayatGames.SaveGameFree.SaveGame.Save<int>(nameof(HealthPlus1) + "_CurrentLevel", CurrentLevel);
            });
        }

        protected override void GoodsFunction()
        {
            playerProperty.MaxHealthPlus1();
        }
    }
}
