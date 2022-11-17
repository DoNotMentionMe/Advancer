using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Adv
{
    public class NormalGameplayBtn : MonoBehaviour,
                                    ISelectHandler
    {
        [SerializeField] AudioData SelectSound;
        public void OnSelect(BaseEventData eventData)
        {
            AudioManager.Instance.PlayRandomSFX(SelectSound);
        }
    }
}
