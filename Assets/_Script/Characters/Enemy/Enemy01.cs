using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class Enemy01 : Enemy
    {
        public bool IsGrounded => groundedDetector.IsGrounded;
        public bool IsFalling => mRigidbody2D.velocity.y < 0f && !IsGrounded;

        [SerializeField] FloatEventChannel attackHit;
        [SerializeField] float moveSpeed;
        [SerializeField] float hitBackSpeedX;
        [SerializeField] float hitBackSpeedY;

        private float moveDirection;

        private Rigidbody2D mRigidbody2D;
        private Transform mTransform;
        private GroundedDetector groundedDetector;
        private Coroutine DetectGroundedStateCor;

        //Test
        [SerializeField] bool IsFixedPositin = false;
        [SerializeField] Vector2 fixedPosition;

        private void Awake()
        {
            mTransform = transform;
            mRigidbody2D = GetComponent<Rigidbody2D>();
            groundedDetector = GetComponentInChildren<GroundedDetector>();

            if (IsFixedPositin)
            {
                fixedPosition = mTransform.position;
            }
        }

        protected override void OnEnable()
        {
            //attackHit.AddListener(Hited);
            base.OnEnable();

            if (IsFixedPositin)
            {
                mTransform.position = fixedPosition;
            }

            if (mTransform.localPosition.x > 0)
            {
                moveDirection = -1;
            }
            else
            {
                moveDirection = 1;
            }
            if (mTransform.localScale.x * moveDirection < 0)
            {
                var Scale = mTransform.localScale;
                Scale.x *= -1;
                mTransform.localScale = Scale;
            }
            if (DetectGroundedStateCor == null)
                DetectGroundedStateCor = StartCoroutine(nameof(DetectGroundedState));
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            DetectGroundedStateCor = null;
        }

        private void Update()
        {
            if (mTransform.position.x < -11)
            {
                gameObject.SetActive(false);
            }
            if (mTransform.position.x > 11)
            {
                gameObject.SetActive(false);
            }

        }

        private void OnDestroy()
        {
            mTransform = null;
            mRigidbody2D = null;
        }

        public override void Hitted(float damage)
        {
            var backDirection = -moveDirection;
            mRigidbody2D.velocity = Vector2.right * backDirection * hitBackSpeedX + Vector2.up * hitBackSpeedY;
            if (DetectGroundedStateCor == null)
                DetectGroundedStateCor = StartCoroutine(nameof(DetectGroundedState));
            base.Hitted(damage);
        }

        IEnumerator DetectGroundedState()
        {
            while (!IsFalling)
            {
                yield return null;
            }
            while (!IsGrounded)
            {
                yield return null;
            }
            mRigidbody2D.velocity = Vector2.right * moveDirection * moveSpeed;
            DetectGroundedStateCor = null;
        }




    }
}