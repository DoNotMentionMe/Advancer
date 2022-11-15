using System;
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Adv
{
    public class InterfaceUI : MonoBehaviour
    {
        [SerializeField] VoidEventChannel LevelStart;
        [SerializeField] VoidEventChannel LevelEnd;
        [SerializeField] VoidEventChannel EarlyOutLevel;
        [SerializeField] VoidEventChannel LevelClosing;
        [SerializeField] VoidEventChannel ClearingUIClose;
        [SerializeField] FloatEventChannel healtChange;
        [SerializeField] FloatEventChannel ComboChange;
        [SerializeField] FloatEventChannel MoneyChange;
        [SerializeField] Text HealthShow;
        [SerializeField] Text ComboShow;
        [SerializeField] Text MoneyShow;
        [SerializeField] GameObject 胜利界面;
        [SerializeField] GameObject 失败界面;
        [SerializeField] GameObject 教程完成界面;
        [SerializeField] GameObject 生存结束界面;
        [SerializeField] GameObject CurrentLiveTimeShow;
        [SerializeField] PlayerInput input;
        [SerializeField] LanguageEventChannel languageChange;
        [SerializeField] PlayerProperty playerProperty;
        [SerializeField] ComboShow ComboShow_Anim;

        private const string healthShowFont = "Health: ";
        private const string healthShowFont_Chinese = "生命值: ";
        private const string ComboShowFont = "Combo ";
        private const string MoneyShowFont = "Money:";
        private const string MoneyShowFont_Chinese = "货币: ";

        private float currentCombo = 0;

        private void Awake()
        {
            LevelStart.AddListener(() =>
            {
                ComboShow.enabled = true;
                CurrentLiveTimeShow.SetActive(true);
            });

            LevelEnd.AddListener(() =>
            {
                ComboShow.enabled = false;
            });

            EarlyOutLevel.AddListener(() =>
            {
                if (BaseLevelModule.LastLevelKey.Equals(nameof(LevelInfinite)))
                {
                    CurrentLiveTimeShow.SetActive(false);
                }
                currentCombo = 0;
            });

            LevelClosing.AddListener(() =>
            {
                if (BaseLevelModule.IsVictory)
                {
                    if (BaseLevelModule.CurrentRunningLevelKey.Equals(nameof(Level0)))
                        教程完成界面.SetActive(true);
                    else
                        胜利界面.SetActive(true);
                }
                else if (BaseLevelModule.CurrentRunningLevelKey.Equals(nameof(LevelInfinite)))
                {
                    生存结束界面.SetActive(true);
                    CurrentLiveTimeShow.SetActive(false);
                }
                else
                    失败界面.SetActive(true);
            });
            ClearingUIClose.AddListener(() =>
            {
                currentCombo = 0;
            });

            ComboChange.AddListener((Combo) =>
            {
                ComboShow.text = string.Concat(ComboShowFont, Combo);
                if (Combo > currentCombo)//连击数增加了
                {
                    //播放增加动画
                    ComboShow_Anim.PlayComboAnim();
                }
                else if (Combo < currentCombo)
                {
                    ComboShow_Anim.ImmediatelySetDown();
                }
                currentCombo = Combo;
            });

            MoneyChange.AddListener((Money) =>
            {
                if (ChineseEnglishShift.language == Language.English)
                    MoneyShow.text = string.Concat(MoneyShowFont, Money);
                else if (ChineseEnglishShift.language == Language.Chinese)
                    MoneyShow.text = string.Concat(MoneyShowFont_Chinese, Money);
            });
            MoneyChange.Broadcast(PlayerAsset.Money);

            languageChange.AddListener((languageC) =>
            {
                if (languageC == Language.English)
                    MoneyShow.text = string.Concat(MoneyShowFont, PlayerAsset.Money);
                else if (languageC == Language.Chinese)
                    MoneyShow.text = string.Concat(MoneyShowFont_Chinese, PlayerAsset.Money);

                if (languageC == Language.English)
                    HealthShow.text = healthShowFont + playerProperty.MaxHealth;
                else if (languageC == Language.Chinese)
                    HealthShow.text = healthShowFont_Chinese + playerProperty.MaxHealth;
            });
        }

        private void OnEnable()
        {
            healtChange.AddListener(ShowHealth);
            input.EnableUIInput();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnDisable()
        {
            healtChange.RemoveListenner(ShowHealth);
        }

        private void ShowHealth(float health)
        {
            if (ChineseEnglishShift.language == Language.English)
                HealthShow.text = healthShowFont + health;
            else if (ChineseEnglishShift.language == Language.Chinese)
                HealthShow.text = healthShowFont_Chinese + health;
        }
    }
}