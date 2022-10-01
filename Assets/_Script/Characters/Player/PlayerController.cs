using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerController : MonoBehaviour
    {
        public float LocalScaleX => mTransform.localScale.x;
        [SerializeField] float moveSpeed = 15f;

        [SerializeField] GameObject rightAttack;
        [SerializeField] GameObject leftAttack;

        private Rigidbody2D mRigidbody;
        private Transform mTransform;
        private GameObject currentAttack;

        private void Awake()
        {
            mTransform = transform;
            mRigidbody = GetComponent<Rigidbody2D>();
        }

        private void OnDestroy()
        {
            mTransform = null;
            mRigidbody = null;
            currentAttack = null;
        }

        public void MoveX(int direction, float speedRatio)
        {
            if (direction != 0)
                mTransform.localScale = Vector3.up + Vector3.right * direction + Vector3.forward;
            mRigidbody.velocity = direction * moveSpeed * Vector2.right;
        }

        public void Stop()
        {
            mRigidbody.velocity = Vector2.zero;
        }

        public void AttackStart(int direction)
        {
            if (direction == 1)
            {
                rightAttack.SetActive(true);
                currentAttack = rightAttack;
            }
            else if (direction == -1)
            {
                leftAttack.SetActive(true);
                currentAttack = leftAttack;
            }
        }

        public void AttackEnd()
        {
            currentAttack.SetActive(false);
        }
    }
}