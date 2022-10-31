using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class ParticleEffectController : PersistentSingleton<ParticleEffectController>
    {
        [SerializeField] ParticleSystem HitEffect;
        private Transform HitEffectTransform;

        protected override void Awake()
        {
            base.Awake();
            HitEffectTransform = HitEffect.transform;
        }

        public void PlayHitEffect(Vector2 PlayPosition)
        {
            HitEffectTransform.position = PlayPosition;
            HitEffect.Play();
        }
    }
}
