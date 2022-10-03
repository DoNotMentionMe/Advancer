using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Adv
{
    [CreateAssetMenu(menuName = "Data/PlayerInput", fileName = "PlayerInput")]
    public class PlayerInput :
                ScriptableObject,
                PlayerInputActions.IGameplayActions
    {
        PlayerInputActions playerInput;

        //---作废-----
        public int axesX => axes.x >= 0 ? (axes.x == 0 ? 0 : 1) : -1;
        public int axesY => axes.y >= 0 ? (axes.y == 0 ? 0 : 1) : -1;
        private Vector2 axes;
        //-----------

        public bool Attack => attack;
        public bool Up => up;
        public bool Right => right;
        public bool Left => left;
        private bool attack;
        private bool up;
        private bool right;
        private bool left;

        private void OnEnable()
        {
            playerInput = new PlayerInputActions();

            playerInput.Gameplay.SetCallbacks(this);

            EnableGameplayInput();
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
        #endregion
    }
}