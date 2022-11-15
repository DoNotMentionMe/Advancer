using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Adv
{
    public class ComboShow : MonoBehaviour
    {
        [SerializeField] Transform mTransform;
        [SerializeField] Text showText;
        [SerializeField] Vector3 MaxLocalScale;
        [SerializeField] float animLength;
        [SerializeField] float StartColorA;
        [SerializeField] float MaxColorA;
        [SerializeField] FloatEventChannel ComboChange;
        [SerializeField] float SetDownValue;
        [SerializeField] float SetDownWaitTime;

        private Coroutine ComboAnimCoroutine;
        private Coroutine SetDownCoroutine;
        private WaitForSeconds waitForSetDownWaitTime;

        private void Awake()
        {
            waitForSetDownWaitTime = new WaitForSeconds(SetDownWaitTime);
            ComboChange.AddListener((combo) =>
            {
                if (SetDownCoroutine != null)
                    StopCoroutine(SetDownCoroutine);
                SetDownCoroutine = StartCoroutine(nameof(SetDown));
            });
        }

        public void ImmediatelySetDown()
        {
            var color = showText.color;
            color.a = SetDownValue;
            showText.color = color;
        }

        public void PlayComboAnim()
        {
            if (ComboAnimCoroutine != null)
                StopCoroutine(ComboAnimCoroutine);
            ComboAnimCoroutine = StartCoroutine(nameof(ComboAnim));
        }

        IEnumerator ComboAnim()
        {
            mTransform.localScale = MaxLocalScale;
            var color = showText.color;
            color.a = MaxColorA;
            showText.color = color;
            float t = 0f;
            while (t < 1)
            {
                mTransform.localScale = Vector3.Lerp(MaxLocalScale, Vector3.one, t);
                color.a = Mathf.Lerp(MaxColorA, StartColorA, t);
                showText.color = color;
                t += Time.deltaTime / animLength;
                yield return null;
            }
            mTransform.localScale = Vector3.one;
            ComboAnimCoroutine = null;
        }

        IEnumerator SetDown()
        {
            yield return waitForSetDownWaitTime;
            var color = showText.color;
            color.a = SetDownValue;
            showText.color = color;
            SetDownCoroutine = null;
        }
    }
}
