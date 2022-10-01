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

        public int axesX => axes.x >= 0 ? (axes.x == 0 ? 0 : 1) : -1;
        public int axesY => axes.y >= 0 ? (axes.y == 0 ? 0 : 1) : -1;
        public bool Attack => attack;

        private Vector2 axes;
        private bool attack;

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
        #endregion
    }
}