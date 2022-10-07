using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Adv
{
    public class InterfaceUI : MonoBehaviour
    {
        [SerializeField] VoidEventChannel Level1Achieve;
        [SerializeField] VoidEventChannel Fail;
        [SerializeField] VoidEventChannel Level1ButtonClick;
        [SerializeField] VoidEventChannel Level2ButtonClick;
        [SerializeField] FloatEventChannel healtChange;
        [SerializeField] Text HealthShow;
        [SerializeField] GameObject Level1;
        [SerializeField] GameObject Level2;

        private string healthShowFont = "Health:";
        private bool level1IsAchieve = false;

        private void OnEnable()
        {
            healtChange.AddListener(ShowHealth);
            Level1ButtonClick.AddListener(SetFalseAllButton);
            Level2ButtonClick.AddListener(SetFalseAllButton);
            Fail.AddListener(SetTrueAllButton);
            Level1Achieve.AddListener(() =>
            {
                level1IsAchieve = true;
                SetTrueAllButton();
            });
            if (!level1IsAchieve)
            {
                Level2.SetActive(false);
            }
        }

        private void OnDisable()
        {
            Level1ButtonClick.RemoveListenner(SetFalseAllButton);
            Level2ButtonClick.RemoveListenner(SetFalseAllButton);
            healtChange.RemoveListenner(ShowHealth);
        }

        private void ShowHealth(float health)
        {
            HealthShow.text = healthShowFont + health;
        }

        private void SetFalseAllButton()
        {
            Level1.SetActive(false);
            Level2.SetActive(false);
        }

        private void SetTrueAllButton()
        {
            Level1.SetActive(true);
            if (level1IsAchieve)
                Level2.SetActive(true);
        }
    }
}