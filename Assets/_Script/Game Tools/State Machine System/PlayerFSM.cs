using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerFSM : StateMachine
    {
        public static GameObject player;

        public PlayerState lastState;

        [SerializeField] PlayerController playerController;
        [SerializeField] PlayerInput playerInput;
        [SerializeField] Animator anim;
        [SerializeField] Animator anim2;
        [SerializeField] GameObject anim2Obj;


        private void Awake()
        {
            if (player == null)
            {
                player = this.gameObject;
            }
            else if (player != this.gameObject)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);

            Register(new PlayerState_Idle());
            //Register(new PlayerState_Move());
            Register(new PlayerState_RightAttack());
            Register(new PlayerState_LeftAttack());
            Register(new PlayerState_UpAttack());
            Register(new PlayerState_RightUpAttack());
        }

        private void OnEnable()
        {
            SwitchOn(typeof(PlayerState_Idle));
        }

        private void OnDestroy()
        {
            stateTable.Clear();
            lastState = null;
        }

        public void Register(PlayerState newState)
        {
            newState.Initialize(playerController, playerInput, anim, anim2, anim2Obj, this);
            stateTable.Add(newState.GetType(), newState);
        }

        public override void SwitchState(Type stateKey)
        {
            lastState = (PlayerState)currentState;
            base.SwitchState(stateKey);
        }

        private void OnGUI()
        {
#if UNITY_EDITOR
            GUILayout.Label(currentState.GetType().ToString());
            GUILayout.Label(playerInput.axesX.ToString());
            GUILayout.Label(Time.timeScale.ToString());
#endif
        }
    }
}