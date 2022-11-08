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

        private const string healthShowFont = "Health: ";
        private const string ComboShowFont = "Combo ";
        private const string MoneyShowFont = "Money:";

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

            ComboChange.AddListener((Combo) =>
            {
                ComboShow.text = string.Concat(ComboShowFont, Combo);
            });

            MoneyChange.AddListener((Money) =>
            {
                MoneyShow.text = string.Concat(MoneyShowFont, Money);
            });
            MoneyChange.Broadcast(PlayerAsset.Money);
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
            HealthShow.text = healthShowFont + health;
        }
    }
}