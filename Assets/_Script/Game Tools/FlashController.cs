using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class FlashController : MonoBehaviour
    {
        [SerializeField] Material flashMaterial;

        private Coroutine mFlashEffectCorotine;

        /// <summary>
        /// 闪烁后执行Action
        /// </summary>
        /// <param name="renderer">闪烁对象的Renderer组件</param>
        /// <param name="OriginMaterial">初始材质</param>
        /// <param name="flashCount">闪烁次数</param>
        /// <param name="flashInterval">闪烁间隔</param>
        /// <param name="lastAction">闪烁结束后执行</param>
        public void FlashEffectAndAction(Renderer renderer, Material OriginMaterial, int flashCount, float flashInterval, Action lastAction)
        {
            WaitForSeconds waitForFlashInterval = new WaitForSeconds(flashInterval);

            StartCoroutine(FlashEffect(renderer, OriginMaterial, flashCount, waitForFlashInterval, lastAction));
        }

        /// <summary>
        /// 闪烁
        /// </summary>
        /// <param name="renderer">闪烁对象的Renderer组件</param>
        /// /// <param name="OriginMaterial">初始材质</param>
        /// <param name="flashCount">闪烁次数</param>
        /// <param name="flashInterval">闪烁间隔</param>
        public void flashEffect(Renderer renderer, Material OriginMaterial, int flashCount, float flashInterval)
        {
            if (mFlashEffectCorotine != null)
            {
                StopCoroutine(mFlashEffectCorotine);
                renderer.material = OriginMaterial;
            }

            WaitForSeconds waitForFlashInterval = new WaitForSeconds(flashInterval);


            mFlashEffectCorotine = StartCoroutine(FlashEffect(renderer, OriginMaterial, flashCount, waitForFlashInterval));
        }

        private IEnumerator FlashEffect(Renderer renderer, Material OriginMaterial, int flashCount, WaitForSeconds waitForFlashInterval)
        {
            for (int i = 0; i < flashCount; i++)
            {
                if (renderer != null)
                    renderer.material = flashMaterial;

                yield return waitForFlashInterval;

                if (renderer != null)
                    renderer.material = OriginMaterial;

                yield return waitForFlashInterval;
            }
            if (renderer != null)
                renderer.material = OriginMaterial;

            mFlashEffectCorotine = null;
        }

        private IEnumerator FlashEffect(Renderer renderer, Material OriginMaterial, int flashCount, WaitForSeconds waitForFlashInterval, Action lastAction)
        {
            yield return StartCoroutine(FlashEffect(renderer, OriginMaterial, flashCount, waitForFlashInterval));

            lastAction?.Invoke();
        }
    }
}
