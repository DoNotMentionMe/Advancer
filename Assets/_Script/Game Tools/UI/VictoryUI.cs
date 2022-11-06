using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Adv
{
    public class VictoryUI : MonoBehaviour
    {
        [SerializeField] FloatEventChannel MoneyChange;
        [SerializeField] Text MoneyGetShow;

        [SerializeField] float Level0Money;
        [SerializeField] float Level1EasyMoney;
        [SerializeField] float Level1Money;
        [SerializeField] float Level1ProMoney;
        [SerializeField] float Level2EasyMoney;
        [SerializeField] float Level2Money;
        [SerializeField] float Level2ProMoney;
        [SerializeField] float Level3EasyMoney;
        [SerializeField] float Level3Money;
        [SerializeField] float Level3ProMoney;
        [SerializeField] float LevelKIA1Money;
        [SerializeField] float Level4EasyMoney;
        [SerializeField] float Level4Money;
        [SerializeField] float Level4ProMoney;
        private Dictionary<string, float> MoneyWithLevelKey = new Dictionary<string, float>();//Key,Money
        [Space]
        [SerializeField] float Level0ExtraMoney;
        [SerializeField] float Level1EasyExtraMoney;
        [SerializeField] float Level1ExtraMoney;
        [SerializeField] float Level1ProExtraMoney;
        [SerializeField] float Level2EasyExtraMoney;
        [SerializeField] float Level2ExtraMoney;
        [SerializeField] float Level2ProExtraMoney;
        [SerializeField] float Level3EasyExtraMoney;
        [SerializeField] float Level3ExtraMoney;
        [SerializeField] float Level3ProExtraMoney;
        [SerializeField] float LevelKIA1ExtraMoney;
        [SerializeField] float Level4EasyExtraMoney;
        [SerializeField] float Level4ExtraMoney;
        [SerializeField] float Level4ProExtraMoney;
        private Dictionary<string, float> ExtraMoneyWithLevelKey = new Dictionary<string, float>();//Key,Money

        private void Awake()
        {
            MoneyWithLevelKey.Add(nameof(Level1Easy), Level1EasyMoney);
            MoneyWithLevelKey.Add(nameof(Level2Easy), Level2EasyMoney);
            MoneyWithLevelKey.Add(nameof(Level3Easy), Level3EasyMoney);
            MoneyWithLevelKey.Add(nameof(Level4Easy), Level4EasyMoney);
            MoneyWithLevelKey.Add(nameof(Level0), Level0Money);
            MoneyWithLevelKey.Add(nameof(Level1), Level1Money);
            MoneyWithLevelKey.Add(nameof(Level2), Level2Money);
            MoneyWithLevelKey.Add(nameof(Level3), Level3Money);
            MoneyWithLevelKey.Add(nameof(Level4), Level4Money);
            MoneyWithLevelKey.Add(nameof(Level1Pro), Level1ProMoney);
            MoneyWithLevelKey.Add(nameof(Level2Pro), Level2ProMoney);
            MoneyWithLevelKey.Add(nameof(Level3Pro), Level3ProMoney);
            MoneyWithLevelKey.Add(nameof(Level4Pro), Level4ProMoney);
            MoneyWithLevelKey.Add(nameof(LevelKIA1), LevelKIA1Money);

            ExtraMoneyWithLevelKey.Add(nameof(Level1Easy), Level1EasyExtraMoney);
            ExtraMoneyWithLevelKey.Add(nameof(Level2Easy), Level2EasyExtraMoney);
            ExtraMoneyWithLevelKey.Add(nameof(Level3Easy), Level3EasyExtraMoney);
            ExtraMoneyWithLevelKey.Add(nameof(Level4Easy), Level4EasyExtraMoney);
            ExtraMoneyWithLevelKey.Add(nameof(Level0), Level0ExtraMoney);
            ExtraMoneyWithLevelKey.Add(nameof(Level1), Level1ExtraMoney);
            ExtraMoneyWithLevelKey.Add(nameof(Level2), Level2ExtraMoney);
            ExtraMoneyWithLevelKey.Add(nameof(Level3), Level3ExtraMoney);
            ExtraMoneyWithLevelKey.Add(nameof(Level4), Level4ExtraMoney);
            ExtraMoneyWithLevelKey.Add(nameof(Level1Pro), Level1ProExtraMoney);
            ExtraMoneyWithLevelKey.Add(nameof(Level2Pro), Level2ProExtraMoney);
            ExtraMoneyWithLevelKey.Add(nameof(Level3Pro), Level3ProExtraMoney);
            ExtraMoneyWithLevelKey.Add(nameof(Level4Pro), Level4ProExtraMoney);
            ExtraMoneyWithLevelKey.Add(nameof(LevelKIA1), LevelKIA1ExtraMoney);
        }

        private void OnEnable()
        {
            if (BaseLevelModule.LastLevelKey == BaseLevelModule.EndKey)
            {
                MoneyChange.Broadcast(PlayerAsset.Money);
            }
            else
            {
                float plusMoney = 0;
                var lastLevelKey = BaseLevelModule.LastLevelKey;
                plusMoney += MoneyWithLevelKey[lastLevelKey];
                MoneyGetShow.text = "";
                MoneyGetShow.text += "通关奖励: " + MoneyWithLevelKey[lastLevelKey];
                if (PlayerProperty.NotEmptyAttackCurrentLevel)
                {
                    plusMoney += ExtraMoneyWithLevelKey[lastLevelKey];
                    MoneyGetShow.text += "\n全连奖励: " + ExtraMoneyWithLevelKey[lastLevelKey];
                }
                if (PlayerProperty.NotHurtCurrentLevel)
                {
                    plusMoney += ExtraMoneyWithLevelKey[lastLevelKey];
                    MoneyGetShow.text += "\n无伤奖励: " + ExtraMoneyWithLevelKey[lastLevelKey];
                }

                PlayerAsset.Money += plusMoney;
                MoneyChange.Broadcast(PlayerAsset.Money);
            }
        }
    }
}
