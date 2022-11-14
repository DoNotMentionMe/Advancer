using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Adv
{
    public class BugGoodsBtn : MonoBehaviour, ISelectHandler
    {
        [SerializeField] Text CommnetText;
        [SerializeField, TextArea(3, 9)] string CommentChinese;
        [SerializeField, TextArea(3, 9)] string CommentEnglish;
        public void OnSelect(BaseEventData eventData)
        {
            if (ChineseEnglishShift.language == Language.Chinese)
                CommnetText.text = CommentChinese;
            else if (ChineseEnglishShift.language == Language.English)
                CommnetText.text = CommentEnglish;

        }
    }
}
