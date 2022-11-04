using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class Arrow : MonoBehaviour
    {
        public enum AttackDirection
        { Horizontal, Vertical }
        [SerializeField] AttackDirection attackDirection;
        [SerializeField] VoidEventChannel attackHit;
        [SerializeField] float attack = 1f;
        [SerializeField] float moveSpeed;
        [SerializeField] float RotationSpeed;

        private int moveDirection;
        private string PlayerTag = "Player";
        private string PlayerAttackTag = "PlayerAttack";
        private Quaternion InitRotation;
        private Rigidbody2D mRigidbody2D;
        private Transform mTransform;

        private void OnTriggerEnter2D(Collider2D col)
        {

            if (col.tag.Equals(PlayerTag))
            {
                if (col.gameObject.TryGetComponent<PlayerProperty>(out PlayerProperty playerProperty))
                {
                    playerProperty.Hitted(attack);
                }
                gameObject.SetActive(false);
            }
            if (col.tag.Equals(PlayerAttackTag))//被命中
            {
                //特效
                var contactPoint = col.ClosestPoint(transform.position);
                if (attackDirection == AttackDirection.Horizontal)
                    ParticleEffectController.Instance.PlayBreakArrow_Horizontal(contactPoint, -(int)mTransform.localScale.x);
                else
                    ParticleEffectController.Instance.PlayBreakArrow_Vertical(contactPoint, -(int)mTransform.localScale.x);
                attackHit.Broadcast();
                gameObject.SetActive(false);
            }
        }

        private void Awake()
        {
            mRigidbody2D = GetComponent<Rigidbody2D>();
            mTransform = GetComponent<Transform>();
            InitRotation = new Quaternion(0, 0, 0, 0);
        }

        private void OnDestroy()
        {
            mRigidbody2D = null;
            mTransform = null;
        }

        private void OnEnable()
        {
            SetMoveDirection();
            SetLocalScale();
            mTransform.rotation = InitRotation;
            mRigidbody2D.velocity = Vector2.right * moveDirection * moveSpeed;
            StartCoroutine(nameof(Rotation));
        }

        private void OnDisable()
        {
            StopAllCoroutines();
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

        IEnumerator Rotation()
        {
            while (true)
            {
                mTransform.rotation *= Quaternion.AngleAxis(Time.deltaTime * RotationSpeed * -moveDirection, Vector3.forward);
                yield return null;
            }
        }
    }
}