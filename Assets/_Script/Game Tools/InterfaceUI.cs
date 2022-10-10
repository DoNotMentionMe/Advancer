using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Adv
{
    public class InterfaceUI : MonoBehaviour
    {
        [SerializeField] FloatEventChannel healtChange;
        [SerializeField] Text HealthShow;
        [SerializeField] GameObject Level0;
        [SerializeField] GameObject Level1;
        [SerializeField] GameObject Level2;

        private string healthShowFont = "Health:";
        private bool level0IsAchieve = false;
        private bool level1IsAchieve = false;

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

        private void SetFalseAllButton()
        {
            Level0.SetActive(false);
            Level1.SetActive(false);
            Level2.SetActive(false);
        }

        private void SetTrueAllButton()
        {
            if (!level0IsAchieve) return;
            Level1.SetActive(true);
            if (!level1IsAchieve) return;
            Level2.SetActive(true);
        }
    }
}