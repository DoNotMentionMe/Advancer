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

        private string healthShowFont = "Health:";

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