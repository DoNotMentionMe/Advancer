using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class Enemy05HittedWeapon : MonoBehaviour
    {
        [SerializeField] CharacterDynamicController animController;
        [SerializeField] RotationDirection rotationDirection;
        [SerializeField] Vector2 flyForce;
        [SerializeField] Rigidbody2D mRigidbody2D;

        private void OnEnable()
        {
            animController.StartRotation(rotationDirection);
            mRigidbody2D.velocity = flyForce;
        }

        private void OnDisable()
        {
            animController.StopRotation();
        }
    }
}
