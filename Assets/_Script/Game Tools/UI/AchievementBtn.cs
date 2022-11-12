using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Adv
{
    public class AchievementBtn : MonoBehaviour,
                                IPointerEnterHandler,
                                IPointerExitHandler,
                                ISelectHandler,
                                IDeselectHandler
    {
        [SerializeField] Text Comment;
        [SerializeField, TextArea(3, 8)] string commentChinese;
        [SerializeField, TextArea(3, 8)] string commentEnglish;

        public void OnDeselect(BaseEventData eventData)
        {
            //Comment.text = "";
        }

        public void OnSelect(BaseEventData eventData)
        {
            Comment.text = ChineseEnglishShift.language == Language.Chinese ? commentChinese : commentEnglish;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //Comment.text = "";
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Comment.text = ChineseEnglishShift.language == Language.Chinese ? commentChinese : commentEnglish;
        }

        // private void StartIntervalExit()
        // {
        //     if (IntervalExitCoroutine != null)
        //         StopCoroutine(IntervalExitCoroutine);
        //     IntervalExitCoroutine = StartCoroutine(nameof(IntervalExit));
        // }

        // private void StopIntervalExit()
        // {
        //     if (IntervalExitCoroutine != null)
        //         StopCoroutine(IntervalExitCoroutine);
        // }

        // private WaitForSeconds waitForInterval;
        // private static Coroutine IntervalExitCoroutine;

        // private void Awake()
        // {
        //     waitForInterval = new WaitForSeconds(0.5f);
        // }

        // IEnumerator IntervalExit()
        // {
        //     yield return waitForInterval;
        //     Comment.text = "";
        //     IntervalExitCoroutine = null;
        // }
    }
}
