using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Adv
{
    public class FailUILevelUpTipsText : MonoBehaviour
    {
        [SerializeField] Text tips;

        [SerializeField, TextArea(3, 5)] string tips1;
        [SerializeField, TextArea(3, 5)] string tips1_English;
        [SerializeField, TextArea(3, 5)] string tips2;
        [SerializeField, TextArea(3, 5)] string tips2_English;
        [SerializeField, TextArea(3, 5)] string tips3;
        [SerializeField, TextArea(3, 5)] string tips3_English;
        [SerializeField, TextArea(3, 5)] string tips4;
        [SerializeField, TextArea(3, 5)] string tips4_English;
        [SerializeField, TextArea(3, 5)] string tips5;
        [SerializeField, TextArea(3, 5)] string tips5_English;

        private static int currentIndex = 1;

        private void OnEnable()
        {
            if (ChineseEnglishShift.language == Language.Chinese)
            {
                switch (currentIndex)
                {
                    case 1:
                        tips.text = tips1;
                        currentIndex++;
                        break;
                    case 2:
                        tips.text = tips2;
                        currentIndex++;
                        break;
                    case 3:
                        tips.text = tips3;
                        currentIndex++;
                        break;
                    case 4:
                        tips.text = tips4;
                        currentIndex++;
                        break;
                    case 5:
                        tips.text = tips5;
                        currentIndex = 1;
                        break;
                    default:
                        break;
                }
            }
            else if (ChineseEnglishShift.language == Language.English)
            {
                switch (currentIndex)
                {
                    case 1:
                        tips.text = tips1_English;
                        currentIndex++;
                        break;
                    case 2:
                        tips.text = tips2_English;
                        currentIndex++;
                        break;
                    case 3:
                        tips.text = tips3_English;
                        currentIndex++;
                        break;
                    case 4:
                        tips.text = tips4_English;
                        currentIndex++;
                        break;
                    case 5:
                        tips.text = tips5_English;
                        currentIndex = 1;
                        break;
                    default:
                        break;
                }
            }
        }

    }
}
