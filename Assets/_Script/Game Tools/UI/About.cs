using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Adv
{
    public class About : MonoBehaviour,
                        IPointerEnterHandler,
                        IPointerExitHandler,
                        IPointerClickHandler
    {
        [SerializeField] Canvas AboutCanvas;
        [SerializeField] PlayerInput input;

        Button mButton;
        bool isClick = false;

        private void Awake()
        {
            mButton = GetComponent<Button>();
            AboutCanvas.enabled = false;
            input.onCloseUI += CloseCanvas;
            mButton.onClick.AddListener(() =>
            {
                EventSystem.current.SetSelectedGameObject(null);
            });
        }

        private void CloseCanvas()
        {
            AboutCanvas.enabled = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!isClick && AboutCanvas.enabled == true)
            {
                isClick = true;
                return;
            }
            else if (!isClick)
            {
                isClick = true;
            }
            else if (isClick)
            {
                isClick = false;
            }
            AboutCanvas.enabled = !AboutCanvas.enabled;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            AboutCanvas.enabled = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!isClick)
                AboutCanvas.enabled = false;
        }
    }
}
