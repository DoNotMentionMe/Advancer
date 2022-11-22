using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Adv
{
    public class AchiveveShow : MonoBehaviour
    {
        [SerializeField] Text 描述;
        [SerializeField] Image Icon;
        [SerializeField] float showTime = 2f;

        public void StartShowAchieve(string comment, Sprite icon)
        {
            gameObject.SetActive(true);
            StartCoroutine(nameof(ShowAchieve));
            描述.text = comment;
            Icon.sprite = icon;
        }

        IEnumerator ShowAchieve()
        {
            yield return waitForShowTime;
            gameObject.SetActive(false);
        }

        private WaitForSeconds waitForShowTime;

        private void Awake()
        {
            waitForShowTime = new WaitForSeconds(showTime);
        }
    }
}
