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
        [SerializeField] VoidEventChannel LevelClosing;
        [SerializeField] FloatEventChannel healtChange;
        [SerializeField] FloatEventChannel ComboChange;
        [SerializeField] Text HealthShow;
        [SerializeField] Text ComboShow;
        [SerializeField] GameObject 胜利界面;
        [SerializeField] GameObject 失败界面;
        [SerializeField] GameObject 教程完成界面;

        private const string healthShowFont = "Health:";
        private const string ComboShowFont = "Combo ";

        private void Awake()
        {
            LevelStart.AddListener(() =>
            {
                ComboShow.enabled = true;
            });

            LevelEnd.AddListener(() =>
            {
                ComboShow.enabled = false;
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
                else
                    失败界面.SetActive(true);
            });

            ComboChange.AddListener((Combo) =>
            {
                ComboShow.text = string.Concat(ComboShowFont, Combo);
            });
        }

        private void OnEnable()
        {
            healtChange.AddListener(ShowHealth);
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