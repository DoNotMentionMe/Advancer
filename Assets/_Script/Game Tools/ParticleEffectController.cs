using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class ParticleEffectController : PersistentSingleton<ParticleEffectController>
    {
        [SerializeField] ParticleSystem HitEffect;
        [SerializeField] ParticleSystem BreakArrow_Horizontal;
        [SerializeField] ParticleSystem BreakArrow_Vertical;
        private Transform HitEffectTransform;
        private Transform BreakArrow_HorizontalTransform;
        private Transform BreakArrow_VerticalTransform;

        protected override void Awake()
        {
            base.Awake();
            HitEffectTransform = HitEffect.transform;
            BreakArrow_HorizontalTransform = BreakArrow_Horizontal.transform;
            BreakArrow_VerticalTransform = BreakArrow_Vertical.transform;

        }

        public void PlayHitEffect(Vector2 PlayPosition)
        {
            HitEffectTransform.position = PlayPosition;
            HitEffect.Play();
        }

        public void PlayBreakArrow_Horizontal(Vector2 PlayPosition, int ScaleX)
        {
            BreakArrow_HorizontalTransform.position = PlayPosition;
            var Scale = BreakArrow_HorizontalTransform.localScale;
            Scale.x = ScaleX;
            BreakArrow_HorizontalTransform.localScale = Scale;
            BreakArrow_Horizontal.Play();
        }

        public void PlayBreakArrow_Vertical(Vector2 PlayPosition, int ScaleX)
        {
            BreakArrow_VerticalTransform.position = PlayPosition;
            var Scale = BreakArrow_VerticalTransform.localScale;
            Scale.x = ScaleX;
            BreakArrow_VerticalTransform.localScale = Scale;
            BreakArrow_Vertical.Play();
        }
    }
}
