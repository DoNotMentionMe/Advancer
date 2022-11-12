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
                if (ChineseEnglishShift.language == Language.Chinese)
                    MoneyGetShow.text = "奖励";
                else if (ChineseEnglishShift.language == Language.English)
                    MoneyGetShow.text = "Bonus";

                if (ChineseEnglishShift.language == Language.Chinese)
                    MoneyGetShow.text += "\n通关:" + MoneyWithLevelKey[lastLevelKey];
                else if (ChineseEnglishShift.language == Language.English)
                    MoneyGetShow.text += "\nClear:" + MoneyWithLevelKey[lastLevelKey];

                if (PlayerProperty.NotEmptyAttackCurrentLevel)//全连
                {
                    plusMoney += ExtraMoneyWithLevelKey[lastLevelKey];
                    if (ChineseEnglishShift.language == Language.Chinese)
                        MoneyGetShow.text += "\n全连:" + ExtraMoneyWithLevelKey[lastLevelKey];
                    else if (ChineseEnglishShift.language == Language.English)
                        MoneyGetShow.text += "\nAll Combo:" + ExtraMoneyWithLevelKey[lastLevelKey];
                    NotEmptyAttackCheck(lastLevelKey);
                }

                if (PlayerProperty.NotHurtCurrentLevel)//无伤
                {
                    plusMoney += ExtraMoneyWithLevelKey[lastLevelKey];
                    if (ChineseEnglishShift.language == Language.Chinese)
                        MoneyGetShow.text += "\n无伤:" + ExtraMoneyWithLevelKey[lastLevelKey];
                    else if (ChineseEnglishShift.language == Language.English)
                        MoneyGetShow.text += "\nNo Damage:" + ExtraMoneyWithLevelKey[lastLevelKey];
                    NotDamageCheck(lastLevelKey);
                }

                PlayerAsset.Money += plusMoney;
                MoneyChange.Broadcast(PlayerAsset.Money);
            }
        }

        private void NotEmptyAttackCheck(string lastLevelKey)
        {
            if (lastLevelKey == nameof(Level1Pro))
            {
                SteamAchievement.Instance.Reach_Achievement(AchievementType.Level1Hard_AllCombo);
                SteamAchievement.Instance.achievementList[7].UnlockAchievementIcon();
            }
            else if (lastLevelKey == nameof(Level2Pro))
            {
                SteamAchievement.Instance.Reach_Achievement(AchievementType.Level2Hard_AllCombo);
                SteamAchievement.Instance.achievementList[8].UnlockAchievementIcon();
            }
            else if (lastLevelKey == nameof(Level3Pro))
            {
                SteamAchievement.Instance.Reach_Achievement(AchievementType.Level3Hard_AllCombo);
                SteamAchievement.Instance.achievementList[9].UnlockAchievementIcon();
            }
            else if (lastLevelKey == nameof(Level4Pro))
            {
                SteamAchievement.Instance.Reach_Achievement(AchievementType.Level4Hard_AllCombo);
                SteamAchievement.Instance.achievementList[10].UnlockAchievementIcon();
            }
        }


        private void NotDamageCheck(string lastLevelKey)
        {
            if (lastLevelKey == nameof(Level1Pro))
            {
                SteamAchievement.Instance.Reach_Achievement(AchievementType.Level1Hard_NotDamage);
                SteamAchievement.Instance.achievementList[11].UnlockAchievementIcon();
            }
            else if (lastLevelKey == nameof(Level2Pro))
            {
                SteamAchievement.Instance.Reach_Achievement(AchievementType.Level2Hard_NotDamage);
                SteamAchievement.Instance.achievementList[12].UnlockAchievementIcon();
            }
            else if (lastLevelKey == nameof(Level3Pro))
            {
                SteamAchievement.Instance.Reach_Achievement(AchievementType.Level3Hard_NotDamage);
                SteamAchievement.Instance.achievementList[13].UnlockAchievementIcon();
            }
            else if (lastLevelKey == nameof(Level4Pro))
            {
                SteamAchievement.Instance.Reach_Achievement(AchievementType.Level4Hard_NotDamage);
                SteamAchievement.Instance.achievementList[14].UnlockAchievementIcon();
            }
        }
    }
}
