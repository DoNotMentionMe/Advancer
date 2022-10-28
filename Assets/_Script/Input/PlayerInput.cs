using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Adv
{
    [CreateAssetMenu(menuName = "Data/PlayerInput", fileName = "PlayerInput")]
    public class PlayerInput :
                ScriptableObject,
                PlayerInputActions.IGameplayActions
    {
        [SerializeField] VoidEventChannel LevelStart;
        [SerializeField] VoidEventChannel LevelEnd;
        PlayerInputActions playerInput;

        //---作废-----
        public int axesX => axes.x >= 0 ? (axes.x == 0 ? 0 : 1) : -1;
        public int axesY => axes.y >= 0 ? (axes.y == 0 ? 0 : 1) : -1;
        private Vector2 axes;
        //-----------

        public event UnityAction onLeft_Long = delegate { };
        public event UnityAction onRight_Long = delegate { };
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

            playerInput.Gameplay.SetCallbacks(this);

            LevelStart.AddListener(() =>
            {
                EnableGameplayInput();
            });
            LevelEnd.AddListener(() =>
            {
                DisableAllInputs();
            });
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnDisable()
        {
            DisableAllInputs();
        }

        public void EnableGameplayInput() => playerInput.Gameplay.Enable();

        public void DisableAllInputs()
        {
            playerInput.Gameplay.Disable();
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
        #endregion
    }
}