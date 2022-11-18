using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class ParticleEffect_Dust : MonoBehaviour
    {
        [SerializeField] Vector2 rightStartPos;
        [SerializeField] Vector2 leftStartPos;
        [SerializeField] Vector2 EndPos;
        [SerializeField] float Distance;
        private ParticleSystem mPartSys;
        private Transform mTransform;
        private float moveSpeed;
        private Coroutine DustRightCor;
        private Coroutine DustLeftCor;

        private void Awake()
        {
            mPartSys = GetComponent<ParticleSystem>();
            mTransform = transform;
            moveSpeed = Distance / mPartSys.main.duration;
        }

        public void StartDustRight()
        {
            if (DustRightCor != null)
            {
                StopCoroutine(DustRightCor);
                DustRightCor = null;
            }
            DustRightCor = StartCoroutine(nameof(DustRight));
        }

        public void StartDustLeft()
        {
            if (DustLeftCor != null)
            {
                StopCoroutine(DustLeftCor);
                DustRightCor = null;
            }
            DustLeftCor = StartCoroutine(nameof(DustLeft));
        }

        IEnumerator DustRight()
        {
            mPartSys.Play();
            mTransform.position = rightStartPos;
            SetScale(1);
            while (mTransform.position.x < 0)
            {
                mTransform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
                yield return null;
            }
            mTransform.position = EndPos;
            DustRightCor = null;
        }

        IEnumerator DustLeft()
        {
            mPartSys.Play();
            mTransform.position = leftStartPos;
            SetScale(-1);
            while (mTransform.position.x > 0)
            {
                mTransform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
                yield return null;
            }
            mTransform.position = EndPos;
            DustLeftCor = null;
        }

        private void SetScale(float x)
        {
            var localscale = mTransform.localScale;
            localscale.x = x;
            mTransform.localScale = localscale;
        }
    }
}
