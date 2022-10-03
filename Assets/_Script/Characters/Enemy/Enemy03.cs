using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class Enemy03 : MonoBehaviour
    {
        [SerializeField] float attack;
        [SerializeField] float moveSpeed;
        [SerializeField] float JumpForce;
        [SerializeField] float LeaveJumpForce;
        [SerializeField] float AttackMoveSpeed;
        private const string Idle = "Idle";
        private const string Jump = "Jump";
        private const string Attack = "Attack";
        private const string PlayerTag = "Player";
        private const string PlayerAttackTag = "PlayerAttack";
        private Animator anim;
        private Transform mTransform;
        private Rigidbody2D mRigidbody2D;
        private Collider2D mCollider2D;
        private GroundedDetector groundedDetector;
        private Coroutine AttackCorotine;


        private void Awake()
        {
            anim = GetComponent<Animator>();
            mTransform = transform;
            mRigidbody2D = GetComponent<Rigidbody2D>();
            mCollider2D = GetComponent<Collider2D>();
            groundedDetector = GetComponentInChildren<GroundedDetector>();

        }

        private void OnDestroy()
        {
            anim = null;
            mTransform = null;
            mRigidbody2D = null;
            mCollider2D = null;
            groundedDetector = null;
        }

        private void OnEnable()
        {
            if (AttackCorotine == null)
                AttackCorotine = StartCoroutine(nameof(AttackCor));
        }

        private void OnDisable()
        {
            StopCoroutine(AttackCorotine);
            AttackCorotine = null;
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

        IEnumerator AttackCor()
        {
            int moveDirection;
            //面向玩家，并移动
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
            //接近一定距离时跳起
            while (Mathf.Abs(mTransform.position.x) > 5)
            {
                yield return null;
            }
            //跳起，落点为玩家位置
            //--落地时间
            var t = 2 * JumpForce / (mRigidbody2D.gravityScale * -Physics2D.gravity.y);
            mRigidbody2D.velocity = Vector2.up * JumpForce + Vector2.right * moveDirection * (5 / t);
            anim.Play(Jump);
            //速度小于0时,转为攻击状态
            while (mRigidbody2D.velocity.y > 0)
            {
                yield return null;
            }
            anim.Play(Attack);
            mCollider2D.enabled = true;
            //落地，转为Jump跳到屏幕另一边
            while (!groundedDetector.IsGrounded)
            {
                yield return null;
            }
            anim.Play(Idle);
            t = 2 * LeaveJumpForce / (mRigidbody2D.gravityScale * -Physics2D.gravity.y);
            mRigidbody2D.velocity = Vector2.up * LeaveJumpForce + Vector2.right * moveDirection * (8 / t);
            //直线攻击
            while (mRigidbody2D.velocity.y > 0)
            {
                yield return null;
            }
            while (!groundedDetector.IsGrounded)
            {
                yield return null;
            }
            anim.Play(Attack);
            mCollider2D.enabled = true;
            moveDirection *= -1;
            if (mTransform.localScale.x * moveDirection < 0)
            {
                var scale = mTransform.localScale;
                scale.x *= -1;
                mTransform.localScale = scale;
            }
            mRigidbody2D.velocity = Vector2.right * AttackMoveSpeed * moveDirection;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag.Equals(PlayerTag))
            {
                if (col.gameObject.TryGetComponent<PlayerProperty>(out PlayerProperty playerProperty))
                {
                    playerProperty.Hitted(attack);
                }
            }
            if (col.tag.Equals(PlayerAttackTag))
            {
                mCollider2D.enabled = false;
            }
        }
    }
}