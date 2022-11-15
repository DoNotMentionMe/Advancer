using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Adv
{
    public class HealthShow : MonoBehaviour
    {
        [SerializeField] Transform mTransform;
        [SerializeField] Text HealthText;
        [SerializeField] Color InitalColor;
        [SerializeField] Color ReduceHealthColor;
        [SerializeField] Vector3 ReduceHealthScale;
        [SerializeField] float animLength;

        private Coroutine ReduceHealthAnim;

        private void Awake()
        {
            HealthText.color = InitalColor;
        }

        public void PlayReduceHealthAnim()
        {
            if (ReduceHealthAnim != null)
                StopCoroutine(ReduceHealthAnim);
            ReduceHealthAnim = StartCoroutine(nameof(ReduceHealth));
        }

        IEnumerator ReduceHealth()
        {
            HealthText.color = ReduceHealthColor;
            mTransform.localScale = ReduceHealthScale;

            float t = 0f;
            while (t < 1)
            {
                mTransform.localScale = Vector3.Lerp(ReduceHealthScale, Vector3.one, t);
                t += Time.deltaTime / animLength;
                yield return null;
            }


            HealthText.color = InitalColor;
            mTransform.localScale = Vector3.one;

            ReduceHealthAnim = null;
        }
    }
}
