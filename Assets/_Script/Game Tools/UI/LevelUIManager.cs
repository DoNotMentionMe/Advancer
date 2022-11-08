using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Adv
{
    public class LevelUIManager : MonoBehaviour
    {
        [SerializeField] Image Backgroun_Font;
        [SerializeField] Text BtnTips;
        [SerializeField] Button LevelSelectLabelButton;
        [SerializeField] GameObject Level0;
        [SerializeField] GameObject Level1Easy;
        [SerializeField] LabelOptionsUI LevelLabel;
        [SerializeField] PlayerInput input;
        [SerializeField] BoolEventChannel CloseAllLabelOption;
        [SerializeField] VoidEventChannel SaveLastBtnBeforeCloseAllUI;

        private Button Level0Btn;
        private Button Level1EasyBtn;
        private Button LastSelectBtn;


        private void Awake()
        {
            LevelSelectLabelButton.onClick.AddListener(() =>
            {
                Backgroun_Font.enabled = LevelLabel.IsOpen;
                BtnTips.enabled = LevelLabel.IsOpen;

                if (LevelLabel.IsOpen)
                {
                    if (LastSelectBtn != null)
                    {
                        LastSelectBtn.Select();
                    }
                    else if (Level0.activeSelf)
                    {
                        if (Level0Btn == null)
                            Level0Btn = Level0.GetComponent<Button>();
                        Level0Btn.Select();
                    }
                    else if (Level1Easy.activeSelf)
                    {
                        if (Level1EasyBtn == null)
                            Level1EasyBtn = Level1Easy.GetComponent<Button>();
                        Level1EasyBtn.Select();
                    }
                }
                else
                {
                    LastSelectBtn = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
                }
            });

            SaveLastBtnBeforeCloseAllUI.AddListener(() =>
            {
                if (LevelLabel.enabled && LevelLabel.IsOpen)
                    LastSelectBtn = EventSystem.current.currentSelectedGameObject?.GetComponent<Button>();
            });

            CloseAllLabelOption.AddListener((IsOpen) =>
            {
                Backgroun_Font.enabled = IsOpen;
                BtnTips.enabled = IsOpen;
            });
        }

        private void Start()
        {
            input.onBattle += OpenLevelUI;
        }

        private void OpenLevelUI()
        {
            LevelSelectLabelButton.onClick.Invoke();
        }

    }
}
