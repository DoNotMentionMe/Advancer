using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class BackgroundScroller : MonoBehaviour
    {
        [SerializeField] SpriteRenderer mSR;
        [SerializeField] float ScrollXOffsetEveryTime;
        [SerializeField] float ScrollYOffsetEveryTime;
        [SerializeField] float ScrollXTimeEveryTime;
        [SerializeField] VoidEventChannel RightAttack;
        [SerializeField] VoidEventChannel LeftAttack;
        [SerializeField] PlayerHittedEventChannel playerHitted;

        private Material material;
        private Coroutine ScrollXCoroutine;

        private void Awake()
        {
            material = mSR.material;

            playerHitted.AddListener((playerHitted, ContactPoint) =>
            {
                if (playerHitted == PlayerHitted.Hitted_Right)
                {
                    if (ScrollXCoroutine != null)
                        StopCoroutine(ScrollXCoroutine);
                    ScrollXCoroutine = StartCoroutine(ScrollX(-1));
                }
                else if (playerHitted == PlayerHitted.Hitted_Left)
                {
                    if (ScrollXCoroutine != null)
                        StopCoroutine(ScrollXCoroutine);
                    ScrollXCoroutine = StartCoroutine(ScrollX(1));
                }
            });

            RightAttack.AddListener(() =>
            {
                if (ScrollXCoroutine != null)
                    StopCoroutine(ScrollXCoroutine);
                ScrollXCoroutine = StartCoroutine(ScrollX(1));
            });

            LeftAttack.AddListener(() =>
            {
                if (ScrollXCoroutine != null)
                    StopCoroutine(ScrollXCoroutine);
                ScrollXCoroutine = StartCoroutine(ScrollX(-1));
            });
        }

        IEnumerator ScrollX(int direction)
        {
            float t = 0;
            while (t < ScrollXTimeEveryTime)
            {
                material.mainTextureOffset += direction * ScrollXOffsetEveryTime / ScrollXTimeEveryTime * Time.deltaTime * Vector2.right;
                t += Time.deltaTime;
                yield return null;
            }
            ScrollXCoroutine = null;
        }
    }
}
