using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class Enemy01 : Enemy
    {
        public enum EnemyState
        {
            Run, Died
        }
        public bool IsGrounded => groundedDetector.IsGrounded;
        public bool IsFalling => mRigidbody2D.velocity.y < 0f && !IsGrounded;

        [SerializeField] GameObjectEventChannel EnemyDied;
        [SerializeField] VoidEventChannel AttackHit;
        [SerializeField] VoidEventChannel LevelEnd;
        [SerializeField] float moveSpeed;
        [SerializeField] float minHitBackSpeedX;
        [SerializeField] float maxHitBackSpeedX;
        [SerializeField] float minHitBackSpeedY;
        [SerializeField] float maxHitBackSpeedY;
        [SerializeField] CharacterDynamicController animController;
        [SerializeField] AudioData PigDied;

        private float moveDirection;

        private Rigidbody2D mRigidbody2D;
        private Transform mTransform;
        private GroundedDetector groundedDetector;
        private Coroutine DetectGroundedAndRunCor;
        private Coroutine DetectGroundedAndDiedCor;
        private SimpleFSM<EnemyState> mFSM = new SimpleFSM<EnemyState>();

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

            mFSM.State(EnemyState.Run)
                .OnEnter(() =>
                {

                    //开始动画
                    animController.StartDynamicChange();

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
                    if (DetectGroundedAndRunCor == null)
                        DetectGroundedAndRunCor = StartCoroutine(nameof(DetectGroundedAndRun));

                })
                .OnUpdate(() =>
                {
                    if (health <= 0)
                    {
                        if (DetectGroundedAndDiedCor == null)
                            DetectGroundedAndDiedCor = StartCoroutine(nameof(DetectGroundedAndDied));
                    }
                });

            mFSM.State(EnemyState.Died)
                .OnEnter(() =>
                {
                    //死亡逆时针旋转
                    //击退
                    var backDirection = -moveDirection;
                    var hitBackSpeedX = Random.Range(minHitBackSpeedX, maxHitBackSpeedX);
                    var hitBackSpeedY = Random.Range(minHitBackSpeedY, maxHitBackSpeedY);
                    mRigidbody2D.velocity = Vector2.right * backDirection * hitBackSpeedX + Vector2.up * hitBackSpeedY;
                });
        }

        protected override void OnEnable()
        {
            //attackHit.AddListener(Hited);
            StopAllCoroutines();
            base.OnEnable();

            mFSM.StartState(EnemyState.Run);

            //Fail.AddListener(SetActiveFalse);
            LevelEnd.AddListener(SetActiveFalse);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            EnemyDied.Broadcast(gameObject);
            //Fail.RemoveListenner(SetActiveFalse);
            LevelEnd.RemoveListenner(SetActiveFalse);
            StopAllCoroutines();
            DetectGroundedAndRunCor = null;
        }

        private void Update()
        {
            mFSM.Update();
            if (mTransform.position.x < -11)
            {
                gameObject.SetActive(false);
            }
            if (mTransform.position.x > 11)
            {
                gameObject.SetActive(false);
            }
        }

        private void FixedUpdate()
        {
            mFSM.FixedUpdate();
        }

        private void OnDestroy()
        {
            mTransform = null;
            mRigidbody2D = null;
        }

        public override void Hitted(float damage)
        {
            //停止动画
            animController.StopDynamicChange();
            if (mTransform.localScale.x > 0)
                animController.StartRotation(RotationDirection.Clockwise);
            else
                animController.StartRotation(RotationDirection.Anticlockwise);
            //音效
            AudioManager.Instance.PlayRandomSFX(PigDied);
            AttackHit.Broadcast();
            var backDirection = -moveDirection;
            mRigidbody2D.velocity = Vector2.right * backDirection * minHitBackSpeedX + Vector2.up * minHitBackSpeedY;
            if (DetectGroundedAndRunCor == null)
                DetectGroundedAndRunCor = StartCoroutine(nameof(DetectGroundedAndRun));
            base.Hitted(damage);
        }

        protected override void Died()
        {
            health = 0;
        }

        private void SetActiveFalse()
        {
            gameObject.SetActive(false);
        }

        IEnumerator DetectGroundedAndRun()
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
            DetectGroundedAndRunCor = null;
        }

        IEnumerator DetectGroundedAndDied()
        {
            while (!IsFalling)
            {
                yield return null;
            }
            while (!IsGrounded)
            {
                yield return null;
            }
            gameObject.SetActive(false);
            DetectGroundedAndDiedCor = null;
        }


    }
}