using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class Enemy04 : MonoBehaviour
    {
        [SerializeField] bool DontSetFalse = false;
        [SerializeField] GameObject ArrowHorizontalPrefab;
        [SerializeField] GameObject ArrowVerticalPrefab;
        [SerializeField] float moveSpeed;
        [SerializeField] float JumpForce;
        [SerializeField] float HorizontalAttackBeforePause;
        [SerializeField] float HorizontalAttackAfterPause;
        [SerializeField] float AttackFallBackTime;
        [SerializeField] float AttackFallBackSpeed;
        [SerializeField] float AttackAfterJumpTime;
        [SerializeField] VoidEventChannel LevelEnd;
        [SerializeField] GameObjectEventChannel EnemyDied;
        [SerializeField] GroundedDetector groundedDetector;

        private const string AttackAfter = "AttackAfter";
        private const string Idle = "Idle";

        private int moveDirection;

        private Transform mTransform;
        private Animator anim;
        private Rigidbody2D mRigidbody2D;
        private Coroutine AttackCorotine;
        private WaitForSeconds waitForHorizontalAttackBeforePause;
        private WaitForSeconds waitForHorizontalAttackAfterPause;
        private WaitForSeconds waitForAttackFallBackTime;
        private WaitForSeconds waitForAttackAfterJumpTime;

        private void Awake()
        {
            mTransform = transform;
            anim = GetComponent<Animator>();
            mRigidbody2D = GetComponent<Rigidbody2D>();
            waitForHorizontalAttackAfterPause = new WaitForSeconds(HorizontalAttackAfterPause);
            waitForHorizontalAttackBeforePause = new WaitForSeconds(HorizontalAttackBeforePause);
            waitForAttackFallBackTime = new WaitForSeconds(AttackFallBackTime);
            waitForAttackAfterJumpTime = new WaitForSeconds(AttackAfterJumpTime);
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
                mRigidbody2D.velocity = Vector2.zero;
                SetMoveDirection();
                SetLocalScale();
                var targetPos = -2.02581f * Vector2.up - moveDirection * 7 * Vector2.right;

                while (Vector3.Distance(mTransform.position, targetPos) > 0.1f)
                {
                    mTransform.position += Time.deltaTime * moveSpeed * Vector3.right * moveDirection;
                    yield return null;
                }

                mTransform.position = targetPos;

                yield return waitForHorizontalAttackBeforePause;
                anim.Play(AttackAfter);
                PoolManager.Instance.Release(ArrowHorizontalPrefab, mTransform.position);
                mRigidbody2D.velocity = -moveDirection * Vector2.right * AttackFallBackSpeed;
                while (Mathf.Abs(mRigidbody2D.velocity.x) > 0.05f)
                {
                    var velocity = mRigidbody2D.velocity;
                    velocity.x = Mathf.Lerp(velocity.x, 0, AttackFallBackSpeed / AttackFallBackTime * Time.deltaTime);//错误用法，但是参数设置好了，还是不动这里了
                    mRigidbody2D.velocity = velocity;
                    yield return null;
                }
                mRigidbody2D.velocity = Vector2.zero;
                yield return waitForHorizontalAttackAfterPause;
                anim.Play(Idle);
                var t = 2 * JumpForce / (mRigidbody2D.gravityScale * -Physics2D.gravity.y);
                mRigidbody2D.velocity = Vector2.up * JumpForce + Vector2.right * -moveDirection * (2.5f / t);

                yield return waitForAttackAfterJumpTime;

                anim.Play(AttackAfter);
                PoolManager.Instance.Release(ArrowVerticalPrefab, mTransform.position);

                while (!groundedDetector.IsGrounded)
                {
                    yield return null;
                }
                anim.Play(Idle);
                //mRigidbody2D.velocity = Vector2.zero;

                while (Mathf.Abs(mTransform.position.x) < 9.5f)
                {
                    yield return null;
                }
                if (!DontSetFalse)
                    gameObject.SetActive(false);

            }
        }
    }
}