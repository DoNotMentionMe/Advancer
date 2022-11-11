using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerController : MonoBehaviour
    {
        public float LocalScaleX => mTransform.localScale.x;
        public bool AttackCanBreak { get => attackCanBreak; set => attackCanBreak = value; }
        public bool DoubleAttack => doubleAttack;
        public float DoubleAttackEffectiveTime => doubleAttackEffectiveTime;

        public VoidEventChannel RightAttack;
        public VoidEventChannel LeftAttack;
        public VoidEventChannel UpAttack;

        public AttackObj UpAttackObj;
        public AttackObj LeftAttackObj;
        public AttackObj RightAttackObj;
        //------作废-----
        //[SerializeField] float moveSpeed = 15f;
        //--------------
        [SerializeField] bool attackCanBreak;
        [SerializeField] bool doubleAttack;
        [SerializeField] bool LongAttack;
        [SerializeField] float doubleAttackEffectiveTime;
        public float AttackStartTime;//攻击前摇
        public float EffectiveAttackTime;
        public float AttackEndTime;//攻击后摇
        [SerializeField] Vector3 LongAttackReleasePos;
        [SerializeField] GameObject upAttack;
        [SerializeField] GameObject rightAttack;
        [SerializeField] GameObject leftAttack;
        [SerializeField] GameObject LeftLongAttack;
        [SerializeField] GameObject RightLongAttack;
        [SerializeField] PlayerInput Input;

        private Rigidbody2D mRigidbody;
        private Transform mTransform;
        private float GeneralAttackStartTime;
        //private GameObject currentAttack;

        private void Awake()
        {
            mTransform = transform;
            mRigidbody = GetComponent<Rigidbody2D>();
            Input.onLeft_Long += LeftLong;
            Input.onRight_Long += RightLong;
            GeneralAttackStartTime = AttackStartTime;
            //GameSaver.Instance.SaveCache<bool>("AttackCanBreak", attackCanBreak);
        }

        private void Start()
        {
            //读取数据
            if (GameSaver.Instance.Exists("AttackCanBreak"))
                attackCanBreak = GameSaver.Instance.Load<bool>("AttackCanBreak");

            GameSaver.Instance.SaveDataEventCall(() =>
            {
                BayatGames.SaveGameFree.SaveGame.Save<bool>("AttackCanBreak", attackCanBreak);
            });


            //SetAttackStartTime();

        }

        public void SetAttackStartTime()
        {
            if (attackCanBreak)
                AttackStartTime = 0f;
            else
                AttackStartTime = GeneralAttackStartTime;
        }

        private void OnDestroy()
        {
            mTransform = null;
            mRigidbody = null;
            //currentAttack = null;
            Input.onLeft_Long -= LeftLong;
            Input.onRight_Long -= RightLong;
        }

        private void LeftLong()
        {
            if (LongAttack)
                PoolManager.Instance.Release(LeftLongAttack, LongAttackReleasePos);
        }

        private void RightLong()
        {
            if (LongAttack)
                PoolManager.Instance.Release(RightLongAttack, LongAttackReleasePos);
        }

        /// <summary>
        /// direction=-1/1，对应要转向左/右
        /// </summary>
        /// <param name="direction"></param>
        public void ChangeScale(int direction)
        {
            if (mTransform.localScale.x * direction < 0)
            {
                var scale = mTransform.localScale;
                scale.x *= -1;
                mTransform.localScale = scale;
            }
        }

        /// <summary>
        /// direction-1\2\3\4，对应左上右下
        /// </summary>
        /// <param name="direction"></param>
        public void AttackStart(int direction)
        {
            if (direction == 1)
            {
                leftAttack.SetActive(true);
            }
            else if (direction == 2)
            {
                upAttack.SetActive(true);
            }
            else if (direction == 3)
            {
                rightAttack.SetActive(true);
            }
        }

        /// <summary>
        /// direction-1\2\3\4，对应左上右下
        /// </summary>
        /// <param name="direction"></param>
        public void AttackEnd(int direction)
        {
            if (direction == 1)
            {
                leftAttack.SetActive(false);
            }
            else if (direction == 2)
            {
                upAttack.SetActive(false);
            }
            else if (direction == 3)
            {
                rightAttack.SetActive(false);
            }
        }

        // public void MoveX(int direction, float speedRatio)
        // {
        //     if (direction != 0)
        //         mTransform.localScale = Vector3.up + Vector3.right * direction + Vector3.forward;
        //     mRigidbody.velocity = direction * moveSpeed * Vector2.right;
        // }

        // public void Stop()
        // {
        //     mRigidbody.velocity = Vector2.zero;
        // }
    }
}