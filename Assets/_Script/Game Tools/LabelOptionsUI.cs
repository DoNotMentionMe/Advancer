using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Adv
{
    public class LabelOptionsUI : MonoBehaviour
    {
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
                ButtonsInLable.Add(LabelCanvas.transform.GetChild(i).GetComponent<Button>());
            }

            labelButton.onClick.AddListener(() =>
            {
                LabelCanvas.enabled = !LabelCanvas.enabled;
                for (var i = 0; i < ButtonsInLable.Count; i++)
                {
                    ButtonsInLable[i].enabled = !ButtonsInLable[i].enabled;
                }
                if (FirstSelectedWhenOpenLable.enabled)
                    FirstSelectedWhenOpenLable.Select();
            });
        }
    }
}