using System;
using System.Collections;
using System.Collections.Generic;
using BayatGames.SaveGameFree;
using UnityEngine;

namespace Adv
{
    public class AudioManager : PersistentSingleton<AudioManager>
    {
        public bool canSFX = false;
        public bool canBGM = false;

        [SerializeField] AudioSource sFXPlayer;
        [SerializeField] AudioSource BGMPlayer;

        private Coroutine WaitSFXPlayEnd;
        private Coroutine SFXPlay;
        private WaitForSeconds waitForSFXPlayEnd;
        private WaitForSeconds waitForSFXplayEndButCanInterrupted;

        const float MIN_PITCH = 0.9f;
        const float MAX_PITCH = 1.1f;

        protected override void Awake()
        {
            base.Awake();
            SaveGame.SavePath = SaveGamePath.DataPath;
            if (SaveGame.Exists("canSFX"))
            {
                canSFX = SaveGame.Load<bool>("canSFX");
            }
            SFXSwitch(canSFX);
            if (SaveGame.Exists("canBGM"))
            {
                canBGM = SaveGame.Load<bool>("canBGM");
            }
            BGMSwitch(canBGM);



        }

        public void BGMSwitch(bool IsOpen)
        {
            BGMPlayer.enabled = IsOpen;
            if (IsOpen)
                BGMPlayer.Play();
        }

        public void SFXSwitch(bool IsOpen)
        {
            sFXPlayer.enabled = IsOpen;
        }


        public void PlaySFXAndDontRepeat(AudioData audioData)
        {
            if (WaitSFXPlayEnd != null) return;
            waitForSFXPlayEnd = new WaitForSeconds(audioData.audioClip.length);
            WaitSFXPlayEnd = StartCoroutine(WaitSFXPlayEndCorotine(() => PlaySFX(audioData)));
        }

        IEnumerator WaitSFXPlayEndCorotine(Action PlayerSFX)
        {
            PlayerSFX?.Invoke();
            yield return waitForSFXPlayEnd;
            waitForSFXPlayEnd = null;
            WaitSFXPlayEnd = null;
        }

        //Used for UI SFX
        public void PlaySFX(AudioData audioData)
        {
            if (!sFXPlayer.enabled) return;
            if (audioData.audioClip != null)
                sFXPlayer.pitch = 1;
            Play(audioData);
        }

        //Used for repeat_play SFX
        public void PlayRandomSFX(AudioData audioData)
        {
            if (!sFXPlayer.enabled) return;
            if (audioData.audioClip != null)
                sFXPlayer.pitch = UnityEngine.Random.Range(MIN_PITCH, MAX_PITCH);
            Play(audioData);
        }

        private void Play(AudioData audioData)
        {
            if (!sFXPlayer.enabled) return;
            //sFXPlayer.pitch = 1;
            if (audioData.audioClip != null)
                sFXPlayer.PlayOneShot(audioData.audioClip, audioData.volume);
        }

        public void PlayRandomSFX(AudioData[] audioDatas)
        {
            PlayRandomSFX(audioDatas[UnityEngine.Random.Range(0, audioDatas.Length)]);
        }



    }
}