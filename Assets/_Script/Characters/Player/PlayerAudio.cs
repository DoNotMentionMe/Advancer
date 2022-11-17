using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerAudio : MonoBehaviour
    {
        public AudioData AttackAudio;
        public AudioData HitAudio;
        public AudioData BeHittedAudio;

        [SerializeField] VoidEventChannel attackHit;

        private void Awake()
        {
            attackHit.AddListener(HitAudioPlay);
        }

        private void OnDestroy()
        {
            attackHit.RemoveListenner(HitAudioPlay);
        }

        private void HitAudioPlay()
        {
            PlayerProperty.AddCombo();
            AudioManager.Instance.PlayRandomSFX(HitAudio);
        }
    }
}