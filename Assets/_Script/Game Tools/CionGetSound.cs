using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class CionGetSound : MonoBehaviour
    {
        [SerializeField] AudioData CionGetSFX;
        [SerializeField] int PlayCount;
        [SerializeField] float playInterval;

        private WaitForSeconds waitForPlayInterval;

        private void Awake()
        {
            waitForPlayInterval = new WaitForSeconds(playInterval);
        }

        public void StartCionGetSound()
        {
            StartCoroutine(nameof(CionGet));
        }

        IEnumerator CionGet()
        {
            int i = 0;
            while (i < PlayCount - 1)
            {
                AudioManager.Instance.PlayRandomSFX(CionGetSFX);
                //AudioManager.Instance.PlaySFX(CionGetSFX);
                i++;
                yield return waitForPlayInterval;
            }
            AudioManager.Instance.PlaySFX(CionGetSFX);
        }
    }
}
