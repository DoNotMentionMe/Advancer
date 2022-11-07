using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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
            playerInput.onEsc += EscEvent_Open;
            playerInput.onExitInQuitExitUI += EscEvent_Close;
            confirm.onClick.AddListener(() =>
            {

                Time.timeScale = 1;
                //这两行和死亡相同
                LevelEnd.Broadcast();//这里关闭了所有Input
                //LevelClosing.Broadcast();
                EarlyOutLevel.Broadcast();//这里开启的UI的Input
                LevelManager.Instance.SelectButtonWithKey(BaseLevelModule.LastLevelKey);
                gameObject.SetActive(false);
            });
            cancel.onClick.AddListener(() =>
            {
                playerInput.DisableAllInputs();
                playerInput.EnableGameplayInput();
                Time.timeScale = 1;
                gameObject.SetActive(false);
            });

            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            playerInput.onEsc -= EscEvent_Open;
            playerInput.onExitInQuitExitUI -= EscEvent_Close;
        }

        private void EscEvent_Open()//开启UI
        {
            if (gameObject.activeSelf)
            {
                //关闭
                playerInput.DisableAllInputs();
                playerInput.EnableGameplayInput();
                Time.timeScale = 1;
                gameObject.SetActive(false);
            }
            else
            {
                //开启
                playerInput.DisableAllInputs();
                playerInput.EnableQuitExitUIInput();
                Time.timeScale = 0;
                cancel.Select();
                gameObject.SetActive(true);
            }
        }

        private void EscEvent_Close()//关闭UI
        {
            if (gameObject.activeSelf)
            {
                //关闭
                playerInput.DisableAllInputs();
                playerInput.EnableGameplayInput();
                Time.timeScale = 1;
                gameObject.SetActive(false);
            }
        }
    }
}
