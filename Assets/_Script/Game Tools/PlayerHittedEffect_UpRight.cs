using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerHittedEffect_UpRight : MonoBehaviour
    {
        [SerializeField] PlayerHittedEventChannel playerhitted;
        private ParticleSystem mParSys;
        private Transform mTransform;

        void Awake()
        {
            mParSys = GetComponent<ParticleSystem>();
            mTransform = transform;
            playerhitted.AddListener((playerhitted, contactPoint) =>
            {
                if (playerhitted == PlayerHitted.Hitted_Right && Mathf.Abs(Mathf.Abs(contactPoint.y) - 1.67f) <= 0.2f)
                    Effect_UpRight(contactPoint);
            });
        }

        private void Effect_UpRight(Vector2 CollPos)
        {
            mTransform.position = CollPos;
            mParSys.Play();
        }
    }
}
