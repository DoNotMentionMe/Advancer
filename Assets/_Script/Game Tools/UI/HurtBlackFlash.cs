using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Adv
{
    public class HurtBlackFlash : MonoBehaviour
    {
        [SerializeField] Image blackPanel;
        [SerializeField] float PanelInitalA = 20;
        [SerializeField] float FlashStartTime;
        [SerializeField] float FlashEndTime;

        private Coroutine BlackFlashCoroutine;

        public void StartBlackFlash()
        {
            if (BlackFlashCoroutine != null)
            {
                StopCoroutine(BlackFlashCoroutine);
                BlackFlashCoroutine = null;
            }
            BlackFlashCoroutine = StartCoroutine(nameof(BlackFlash));
        }

        IEnumerator BlackFlash()
        {
            var color = blackPanel.color;
            color.a = PanelInitalA;
            blackPanel.color = color;
            float t = 0f;
            while (t < 1f)
            {
                t += Time.unscaledDeltaTime / FlashStartTime;
                color.a = Mathf.Lerp(0f, PanelInitalA, t);
                blackPanel.color = color;
                yield return null;
            }
            t = 0f;
            while (t < 1f)
            {
                t += Time.unscaledDeltaTime / FlashEndTime;
                color.a = Mathf.Lerp(PanelInitalA, 0f, t);
                blackPanel.color = color;
                yield return null;
            }
        }
    }
}
