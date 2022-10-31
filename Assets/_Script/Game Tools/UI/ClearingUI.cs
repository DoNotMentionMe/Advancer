using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Adv
{
    public class ClearingUI : MonoBehaviour
    {
        [SerializeField] VoidEventChannel LevelStart;
        [SerializeField] VoidEventChannel ClearingUIClose;
        [SerializeField] FloatEventChannel MoneyChange;
        [SerializeField] Button CloseButton;

        private Button LastSelected;

        private void Awake()
        {
            LevelStart.AddListener(() =>
            {
                LastSelected = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            });

            CloseButton.onClick.AddListener(() =>
            {
                ClearingUIClose.Broadcast();
                LastSelected.Select();
                gameObject.SetActive(false);
            });

            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            CloseButton.Select();
            CloseButton.OnSelect(null);
        }
    }
}