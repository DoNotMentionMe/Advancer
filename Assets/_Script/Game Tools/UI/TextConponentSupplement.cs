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
        private VoidEventChannel IsControllerChange;
        private Text mText;

        private void Awake()
        {
            mText = GetComponent<Text>();
            LanguageChange = Resources.Load<LanguageEventChannel>("EventChannels/LanguageEventChannel_LanguageChange");
            IsControllerChange = Resources.Load<VoidEventChannel>("EventChannels/VoidEventChannel_IsControllerChange");



            // string[] names = Input.GetJoystickNames();
            // for (var i = 0; i < names.Length; i++)
            // {
            //     if (names[i].StartsWith("Controller"))
            //     {
            //         IsController = true;
            //     }
            // }

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

            IsControllerChange.AddListener(() =>
            {
                SetTextWihtController();
            });
        }

        private void SetTextWihtController()
        {
            if (!ChangeWithControllerInput) return;
            if (InterfaceUI.IsController)
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
    }
}
