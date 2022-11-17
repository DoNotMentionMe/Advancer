using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Adv
{
    public class NormalGameplayBtn : MonoBehaviour,
                                    ISelectHandler,
                                    ISubmitHandler
    {
        [SerializeField] AudioData SelectSound;
        [SerializeField] AudioData SubmitSound;
        public void OnSelect(BaseEventData eventData)
        {
            AudioManager.Instance.PlayRandomSFX(SelectSound);
        }

        public void OnSubmit(BaseEventData eventData)
        {
            AudioManager.Instance.PlayRandomSFX(SubmitSound);
        }
    }
}
