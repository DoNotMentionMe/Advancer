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
        [SerializeField, TextArea(3, 8)] string ChineseText;
        [SerializeField, TextArea(3, 8)] string EnglishText;

        private LanguageEventChannel LanguageChange;
        private Text mText;

        private void Awake()
        {
            mText = GetComponent<Text>();
            LanguageChange = Resources.Load<LanguageEventChannel>("EventChannels/LanguageEventChannel_LanguageChange");

            if (ChineseEnglishShift.language == Language.Chinese)
            {
                mText.text = ChineseText;
            }
            else if (ChineseEnglishShift.language == Language.English)
            {
                mText.text = EnglishText;
            }

            LanguageChange.AddListener((language) =>
            {
                if (language == Language.Chinese)
                {
                    mText.text = ChineseText;
                }
                else if (language == Language.English)
                {
                    mText.text = EnglishText;
                }
            });
        }
    }
}
