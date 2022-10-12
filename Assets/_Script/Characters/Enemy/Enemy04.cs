using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class Enemy04 : MonoBehaviour
    {
        [SerializeField] GameObject ArrowHorizontalPrefab;
        [SerializeField] GameObject ArrowVerticalPrefab;
        [SerializeField] float moveSpeed;
        [SerializeField] float JumpForce;
        [SerializeField] float AttackPause;
        [SerializeField] float GroundedPause;
        [SerializeField] VoidEventChannel LevelEnd;
        [SerializeField] GameObjectEventChannel EnemyDied;
        [SerializeField] GroundedDetector groundedDetector;

        private int moveDirection;

        private Transform mTransform;
        private Animator anim;
        private Rigidbody2D mRigidbody2D;
        private Coroutine AttackCorotine;
        private WaitForSeconds waitForAttackPause;
        private WaitForSeconds waitForGroundedPause;

        private void Awake()
        {
            mTransform = transform;
            anim = GetComponent<Animator>();
            mRigidbody2D = GetComponent<Rigidbody2D>();
            waitForGroundedPause = new WaitForSeconds(GroundedPause);
            waitForAttackPause = new WaitForSeconds(AttackPause);
        }

        private void OnEnable()
        {
            LevelEnd.AddListener(SetActiveFalse);
            if (AttackCorotine == null)
                StartCoroutine(nameof(Attack));
        }

        private void OnDisable()
        {
            LevelEnd.RemoveListenner(SetActiveFalse);
            EnemyDied.Broadcast(gameObject);
            StopAllCoroutines();
        }

        private void OnDestroy()
        {
            mTransform = null;
        }

        private void SetActiveFalse()
        {
            gameObject.SetActive(false);
        }

        private void SetMoveDirection()
        {
            if (mTransform.position.x > 0)
                moveDirection = -1;
            else
                moveDirection = 1;
        }

        private void SetLocalScale()
        {
            if (mTransform.localScale.x * moveDirection < 0)
            {
                var Scale = mTransform.localScale;
                Scale.x *= -1;
                mTransform.localScale = Scale;
            }
        }

        IEnumerator Attack()
        {
            while (true)
            {
                SetMoveDirection();
                SetLocalScale();
                var targetPos = -2.02581f * Vector2.up - moveDirection * 7 * Vector2.right;

                while (Vector3.Distance(mTransform.position, targetPos) > 0.1f)
                {
                    mTransform.position += Time.deltaTime * moveSpeed * Vector3.right * moveDirection;
                    yield return null;
                }

                mTransform.position = targetPos;
                PoolManager.Instance.Release(ArrowHorizontalPrefab, mTransform.position);
                var t = 2 * JumpForce / (mRigidbody2D.gravityScale * -Physics2D.gravity.y);
                mRigidbody2D.velocity = Vector2.up * JumpForce + Vector2.right * -moveDirection * (1 / t);

                while (mRigidbody2D.velocity.y > 0)
                {
                    yield return null;
                }

                PoolManager.Instance.Release(ArrowVerticalPrefab, mTransform.position);

                while (!groundedDetector.IsGrounded)
                {
                    yield return null;
                }

                mRigidbody2D.velocity = Vector2.zero;

                yield return waitForGroundedPause;
            }
        }
    }
}