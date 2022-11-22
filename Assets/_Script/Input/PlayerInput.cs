using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Adv
{
    [CreateAssetMenu(menuName = "Data/PlayerInput", fileName = "PlayerInput")]
    public class PlayerInput :
                ScriptableObject,
                PlayerInputActions.IGameplayActions,
                PlayerInputActions.IUIActions,
                PlayerInputActions.IGlobalActions,
                PlayerInputActions.IQuitExitUIActions
    {
        [SerializeField] VoidEventChannel LevelStart;
        [SerializeField] VoidEventChannel LevelEnd;
        [SerializeField] VoidEventChannel ClearingUIClose;
        [SerializeField] VoidEventChannel EarlyOutLevel;
        [SerializeField] AudioData BtnSubmitSound;
        PlayerInputActions playerInput;

        //---作废-----
        public int axesX => axes.x >= 0 ? (axes.x == 0 ? 0 : 1) : -1;
        public int axesY => axes.y >= 0 ? (axes.y == 0 ? 0 : 1) : -1;
        private Vector2 axes;
        //-----------

        public event UnityAction onDown = delegate { };
        public event UnityAction onLeft_Long = delegate { };
        public event UnityAction onRight_Long = delegate { };
        public event UnityAction onEsc = delegate { };
        public event UnityAction onBattle = delegate { };
        public event UnityAction onShop = delegate { };
        public event UnityAction onCloseUI = delegate { };
        public event UnityAction onExitInQuitExitUI = delegate { };
        public bool Attack => attack;
        public bool Up { get => up; set => up = value; }
        public bool Right { get => right; set => right = value; }
        public bool Left { get => left; set => left = value; }
        private bool attack;
        private bool up;
        private bool right;
        private bool left;

        private void OnEnable()
        {
            playerInput = new PlayerInputActions();

            playerInput.Global.SetCallbacks(this);
            playerInput.Global.Enable();
            playerInput.Gameplay.SetCallbacks(this);
            playerInput.UI.SetCallbacks(this);
            playerInput.QuitExitUI.SetCallbacks(this);

            LevelStart.AddListener(() =>
            {
                DisableAllInputs();
                EnableGameplayInput();
            });
            LevelEnd.AddListener(() =>
            {
                DisableAllInputs();
            });
            ClearingUIClose.AddListener(() =>
            {
                EnableUIInput();
            });
            EarlyOutLevel.AddListener(() =>
            {
                EnableUIInput();
            });
        }

        private void OnDisable()
        {
            DisableAllInputs();
        }

        public void EnableGameplayInput() => playerInput.Gameplay.Enable();
        public void EnableUIInput() => playerInput.UI.Enable();
        public void EnableQuitExitUIInput() => playerInput.QuitExitUI.Enable();

        public void DisableQuitExitUIInput() => playerInput.QuitExitUI.Disable();

        public void DisableAllInputs()
        {
            playerInput.Gameplay.Disable();
            playerInput.UI.Disable();
            playerInput.QuitExitUI.Disable();
        }

        #region Gameplay
        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                attack = true;
            }
            else if (context.canceled)
            {
                attack = false;
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {

        }

        //-----------作废---------
        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                axes = context.ReadValue<Vector2>();
            }
            else if (context.canceled)
            {
                axes = Vector2.zero;
            }
        }
        //-------------------------

        public void OnUp(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                up = true;
            }
            else if (context.canceled)
            {
                up = false;
            }
        }

        public void OnRight(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                right = true;
            }
            else if (context.canceled)
            {
                right = false;
            }
        }

        public void OnLeft(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                left = true;
            }
            else if (context.canceled)
            {
                left = false;
            }
        }

        public void OnDown(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                onDown?.Invoke();
            }
        }

        public void OnLeft_Long(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                onLeft_Long.Invoke();
            }
        }

        public void OnRight_Long(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                onRight_Long.Invoke();
            }
        }

        public void OnEsc(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                onEsc?.Invoke();
            }
        }

        public void OnBattle(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                onBattle?.Invoke();
            }
        }

        public void OnShop(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                onShop?.Invoke();
            }
        }

        public void On关闭界面(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                onCloseUI?.Invoke();
            }
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                var obj = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
                if (obj == null) return;
                if (obj.activeSelf)
                {
                    var btn = obj.GetComponent<UnityEngine.UI.Button>();
                    btn.OnSubmit(null);
                    AudioManager.Instance.PlayRandomSFX(BtnSubmitSound);
                }
            }
        }

        public void On关闭(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                onExitInQuitExitUI?.Invoke();
            }
        }
        #endregion
    }
}