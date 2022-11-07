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
        [SerializeField] VoidEventChannel SaveDataEvent;
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
                if (LastSelected.enabled)
                    LastSelected.Select();
                else
                    LevelManager.Instance.SelectButtonWithKey(nameof(Level1Easy));
                GameSaver.Instance.SaveAllData();//最后保存数据
                gameObject.SetActive(false);
            });

            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            CloseButton.Select();
            CloseButton.OnSelect(null);
        }

        private void OnDisable()
        {

        }
    }
}