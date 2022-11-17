using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Adv
{
    public class NormalSettingBtn : MonoBehaviour,
                                IPointerEnterHandler,
                                IPointerClickHandler
    {
        [SerializeField] AudioData EnterSound;
        [SerializeField] AudioData SubmitSound;

        public void OnPointerClick(PointerEventData eventData)
        {
            AudioManager.Instance.PlayRandomSFX(SubmitSound);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            AudioManager.Instance.PlayRandomSFX(EnterSound);
        }
    }
}
