using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Adv
{
    public class CurrentLiveTimeShow : MonoBehaviour
    {
        [HideInInspector] public int liveTime;

        private Text text;
        private float LevelStartTime;

        private void Awake()
        {
            text = GetComponent<Text>();
        }

        private void OnEnable()
        {
            if (BaseLevelModule.CurrentRunningLevelKey != nameof(LevelInfinite))
            {
                gameObject.SetActive(false);
                return;
            }
            liveTime = 0;
            LevelStartTime = Time.time;
            //开始计算协程
            StartCoroutine(nameof(CountLiveTime));
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        IEnumerator CountLiveTime()
        {
            while (true)
            {
                yield return null;
                liveTime = (int)(Time.time - LevelStartTime);
                text.text = liveTime.ToString();
            }
        }
    }
}
