using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Adv
{
    public class LabelOptionsUI : MonoBehaviour
    {
        public bool IsOpen = false;
        [SerializeField] BoolEventChannel CloseAllLabelOption;
        [SerializeField] Button labelButton;
        [SerializeField] Button FirstSelectedWhenOpenLable;
        [SerializeField] Canvas LabelCanvas;
        [Header("===下面列表不用设置, 显示只用于Debug===")]
        [SerializeField] List<Button> ButtonsInLable = new List<Button>();

        private void Awake()
        {
            var labelButtonCount = LabelCanvas.transform.childCount;
            for (var i = 0; i < labelButtonCount; i++)
            {
                if (LabelCanvas.transform.GetChild(i).TryGetComponent<Button>(out Button btn))
                {
                    //关卡
                    ButtonsInLable.Add(btn);
                }
                else
                {
                    //商品条目，第三个子对象是按键
                    ButtonsInLable.Add(LabelCanvas.transform.GetChild(i).GetChild(2).GetComponent<Button>());
                }
            }
            CloseAllLabelOption.AddListener((Switch) =>
            {
                PageSwitch(Switch);
            });

            labelButton.onClick.AddListener(() =>
            {
                var isOpen = IsOpen;//记录当前选项是否开启
                CloseAllLabelOption.Broadcast(false);
                PageSwitch(!isOpen);
                IsOpen = !isOpen;
            });

        }

        private void PageSwitch(bool Switch)
        {
            IsOpen = Switch;
            LabelCanvas.enabled = Switch;
            for (var i = 0; i < ButtonsInLable.Count; i++)
            {
                ButtonsInLable[i].enabled = Switch;
            }
            if (FirstSelectedWhenOpenLable.enabled)
                FirstSelectedWhenOpenLable.Select();

        }
    }
}