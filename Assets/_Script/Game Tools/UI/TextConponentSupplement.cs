using System.Data;
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Adv
{
    /// <summary>
    /// 1、识别当前选择的语言，修改文本信息
    /// 2、当调用LanguageChange.Brocast()时候将改变文本(设置界面中调用)
    /// </summary>
    public class TextConponentSupplement : MonoBehaviour
    {
        [SerializeField] bool ChangeWithControllerInput;
        [SerializeField, TextArea(3, 8)] string ChineseText;
        [SerializeField, TextArea(3, 8)] string ChineseText_Controller;
        [SerializeField, TextArea(3, 8)] string EnglishText;
        [SerializeField, TextArea(3, 8)] string EnglishText_Controller;

        private LanguageEventChannel LanguageChange;
        private Text mText;
        private bool IsController;

        private void Update()
        {
            if (!ChangeWithControllerInput) return;
            string[] names = Input.GetJoystickNames();
            bool IsEnterController = false;
            for (var i = 0; i < names.Length; i++)
            {
                if (names[i].StartsWith("Controller"))
                {
                    if (!IsController)//插入手柄事件
                    {
                        IsController = true;
                        SetTextWihtController();
                    }
                    IsEnterController = true;
                }
                if (i == 2 && IsController && !IsEnterController)//拔出手柄事件
                {
                    IsController = false;
                    SetTextWihtController();
                }
            }
            // if (!IsController && names.Length > 0)
            // {
            //     SetTextWihtController();
            //     IsController = true;
            // }
            // else if (IsController && names.Length == 0)
            // {
            //     SetTextWihtController();
            //     IsController = false;
            // }
        }

        private void SetTextWihtController()
        {
            if (!ChangeWithControllerInput) return;
            if (IsController)
            {
                //切换文本
                if (ChineseEnglishShift.language == Language.Chinese)
                {
                    mText.text = ChineseText_Controller;
                }
                else if (ChineseEnglishShift.language == Language.English)
                {
                    mText.text = EnglishText_Controller;
                }
            }
            else
            {
                if (ChineseEnglishShift.language == Language.Chinese)
                {
                    mText.text = ChineseText;
                }
                else if (ChineseEnglishShift.language == Language.English)
                {
                    mText.text = EnglishText;
                }
            }
        }

        private void Awake()
        {
            mText = GetComponent<Text>();
            LanguageChange = Resources.Load<LanguageEventChannel>("EventChannels/LanguageEventChannel_LanguageChange");

            string[] names = Input.GetJoystickNames();
            for (var i = 0; i < names.Length; i++)
            {
                if (names[i].StartsWith("Controller"))
                {
                    IsController = true;
                }
            }

            if (ChineseEnglishShift.language == Language.Chinese)
            {
                mText.text = ChineseText;
            }
            else if (ChineseEnglishShift.language == Language.English)
            {
                mText.text = EnglishText;
            }
            SetTextWihtController();

            LanguageChange.AddListener((language) =>
            {
                if (language == Language.Chinese)
                {
                    mText.text = ChineseText;
                    SetTextWihtController();
                }
                else if (language == Language.English)
                {
                    mText.text = EnglishText;
                    SetTextWihtController();
                }
            });
        }
    }
}
