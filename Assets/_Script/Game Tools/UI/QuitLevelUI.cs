using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Adv
{
    public class QuitLevelUI : MonoBehaviour
    {
        [SerializeField] Button confirm;
        [SerializeField] Button cancel;
        [SerializeField] PlayerInput playerInput;
        [SerializeField] VoidEventChannel LevelEnd;
        [SerializeField] VoidEventChannel EarlyOutLevel;

        private void Awake()
        {
            playerInput.onEsc += EscEvent;
            confirm.onClick.AddListener(() =>
            {
                Time.timeScale = 1;
                //这两行和死亡相同
                LevelEnd.Broadcast();
                //LevelClosing.Broadcast();
                EarlyOutLevel.Broadcast();
                LevelManager.Instance.SelectButtonWithKey(BaseLevelModule.LastLevelKey);
                gameObject.SetActive(false);
            });
            cancel.onClick.AddListener(() =>
            {
                Time.timeScale = 1;
                gameObject.SetActive(false);
            });

            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            playerInput.onEsc -= EscEvent;
        }

        private void EscEvent()
        {
            if (gameObject.activeSelf)
            {
                //关闭
                Time.timeScale = 1;
                gameObject.SetActive(false);
            }
            else
            {
                //开启
                Time.timeScale = 0;
                cancel.Select();
                gameObject.SetActive(true);
            }
        }
    }
}
