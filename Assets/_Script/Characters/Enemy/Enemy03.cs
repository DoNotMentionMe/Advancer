using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class Enemy03 : MonoBehaviour
    {
        [SerializeField] VoidEventChannel attackHit;
        [SerializeField] VoidEventChannel LevelEnd;
        [SerializeField] GameObjectEventChannel EnemyDied;
        [SerializeField] PlayerHittedEventChannel playerHitted;
        [SerializeField] float attack;
        [SerializeField] float moveSpeed;
        [SerializeField] float JumpForce;
        [SerializeField] float AirStandStill;
        [SerializeField] float AttackFallStartSpeed;
        [SerializeField] float LeaveJumpForce;
        [SerializeField] float HitBackJumpForce;
        [SerializeField] float AttackMoveSpeed;
        [SerializeField] float HitBackMoveSpeed;
        [SerializeField] Collider2D mCollider2D;
        [SerializeField] AudioData HitKnife;
        [SerializeField] CharacterDynamicController animController;
        private const string Idle = "Idle";
        private const string Jump = "Jump";
        private const string Attack = "Attack";
        private const string PlayerTag = "Player";
        private const string PlayerAttackTag = "PlayerAttack";

        private bool IsAttackedPlayer = false;
        private Animator anim;
        private Transform mTransform;
        private Rigidbody2D mRigidbody2D;
        private GroundedDetector groundedDetector;
        private Coroutine AttackCorotine;
        private WaitForSeconds waitForAirStandstill;


        private void Awake()
        {
            anim = GetComponent<Animator>();
            mTransform = transform;
            mRigidbody2D = GetComponent<Rigidbody2D>();
            groundedDetector = GetComponentInChildren<GroundedDetector>();
            waitForAirStandstill = new WaitForSeconds(AirStandStill);
        }

        private void OnDestroy()
        {
            anim = null;
            mTransform = null;
            mRigidbody2D = null;
            groundedDetector = null;
        }

        private void OnEnable()
        {
            IsAttackedPlayer = false;
            //Fail.AddListener(SetActiveFalse);
            LevelEnd.AddListener(SetActiveFalse);
            if (AttackCorotine == null)
                AttackCorotine = StartCoroutine(nameof(AttackCor));
        }

        private void OnDisable()
        {
            //Fail.RemoveListenner(SetActiveFalse);
            LevelEnd.RemoveListenner(SetActiveFalse);
            EnemyDied.Broadcast(gameObject);
            StopCoroutine(AttackCorotine);
            AttackCorotine = null;
            IsAttackedPlayer = false;
            mTransform.localScale = Vector3.one;
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

        private void SetActiveFalse()
        {
            gameObject.SetActive(false);
        }

        IEnumerator AttackCor()
        {
            mCollider2D.enabled = false;
            int moveDirection;
            //????????????????????????
            if (mTransform.position.x > 0)
                moveDirection = -1;
            else
                moveDirection = 1;
            if (mTransform.localScale.x * moveDirection < 0)
            {
                var scale = mTransform.localScale;
                scale.x *= -1;
                mTransform.localScale = scale;
            }
            mRigidbody2D.velocity = Vector2.right * moveSpeed * moveDirection;
            //??????5?????????
            while (Mathf.Abs(mTransform.position.x) > 5)
            {
                yield return null;
            }
            //?????????????????????????????????????????????
            //--????????????
            //1
            //var t = 2 * JumpForce / (mRigidbody2D.gravityScale * -Physics2D.gravity.y);
            //2
            var t = JumpForce / (mRigidbody2D.gravityScale * -Physics2D.gravity.y);
            mRigidbody2D.velocity = Vector2.up * JumpForce + Vector2.right * moveDirection * (4 / t);
            anim.Play(Jump);
            //????????????0???,??????????????????
            while (mRigidbody2D.velocity.y > 0)
            {
                yield return null;
            }
            mRigidbody2D.velocity = Vector2.zero;
            //??????????????????
            yield return waitForAirStandstill;
            anim.Play(Attack);
            //2
            mRigidbody2D.velocity = Vector2.down * AttackFallStartSpeed;
            mCollider2D.enabled = true;
            //??????????????????????????????
            while (!groundedDetector.IsGrounded)
            {
                //?????????????????????
                if (!mCollider2D.enabled)
                {
                    break;
                }
                yield return null;
            }

            if (!mCollider2D.enabled)//?????????
            {
                anim.Play(Jump);
                //????????????
                mCollider2D.enabled = false;
                if (mTransform.localScale.x > 0)
                    animController.StartRotation(RotationDirection.Anticlockwise, 1f);
                else
                    animController.StartRotation(RotationDirection.Clockwise, 1f);
            }
            else
                anim.Play(Idle);



            t = 2 * LeaveJumpForce / (mRigidbody2D.gravityScale * -Physics2D.gravity.y);
            var StartPoint = mCollider2D.enabled ? 8 : 5f;
            var backJumpForce = mCollider2D.enabled ? LeaveJumpForce : HitBackJumpForce;
            var distanceToStartPoint = Mathf.Abs(-moveDirection * StartPoint - mTransform.position.x);
            mRigidbody2D.velocity = Vector2.up * backJumpForce + Vector2.right * -moveDirection * (distanceToStartPoint / t);
            //????????????
            while (mRigidbody2D.velocity.y > 0)
            {
                yield return null;
            }
            while (!groundedDetector.IsGrounded)
            {
                yield return null;
            }
            anim.Play(Idle);
            mRigidbody2D.velocity = Vector2.zero;
            yield return new WaitForSeconds(0.5f);
            anim.Play(Attack);
            mCollider2D.enabled = true;
            mRigidbody2D.velocity = Vector2.right * AttackMoveSpeed * moveDirection;

            while (mCollider2D.enabled)
            {
                yield return null;
            }
            if (IsAttackedPlayer)
            {
                //?????????????????????
                anim.Play(Idle);
                mRigidbody2D.velocity = Vector2.right * HitBackMoveSpeed / 2f * -moveDirection + Vector2.up * backJumpForce * 3;
                var scale = mTransform.localScale;
                scale.x *= -1;
                mTransform.localScale = scale;
                animController.StartScaleSmall();
            }
            else
            {
                //??????????????????
                anim.Play(Jump);
                mRigidbody2D.velocity = Vector2.right * HitBackMoveSpeed / 1.5f * -moveDirection + Vector2.up * backJumpForce * 2;
                mCollider2D.enabled = false;
                if (mTransform.localScale.x > 0)
                    animController.StartRotationWithSpeed(RotationDirection.Clockwise, 1300f);
                else
                    animController.StartRotationWithSpeed(RotationDirection.Anticlockwise, 1300f);
            }

        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag.Equals(PlayerTag))
            {
                IsAttackedPlayer = true;
                mCollider2D.enabled = false;
                if (col.gameObject.TryGetComponent<PlayerProperty>(out PlayerProperty playerProperty))
                {
                    var contactPoint = col.ClosestPoint(transform.position);
                    if (contactPoint.x > 0)
                    {
                        playerHitted.Broadcast(PlayerHitted.Hitted_Right, contactPoint);
                        //PlayerHittedEffect_Right.Instance.Effect_Right(contactPoint);
                    }
                    else
                    {
                        playerHitted.Broadcast(PlayerHitted.Hitted_Left, contactPoint);
                        //PlayerHittedEffect_Left.Instance.Effect_Left(contactPoint);
                    }
                    playerProperty.Hitted(attack);
                }
            }
            if (col.tag.Equals(PlayerAttackTag))//?????????
            {
                attackHit.Broadcast();
                AudioManager.Instance.PlayRandomSFX(HitKnife);
                mCollider2D.enabled = false;
            }
        }
    }
}