using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class BOSS01 : MonoBehaviour
    {
        [SerializeField] string SkillRelaseSequence;
        [SerializeField] float moveSpeed;
        [SerializeField] float JumpForce;
        [SerializeField] float DistanceWithPlayer = 4.4f;
        [SerializeField] Vector2 InitialPosition;
        [SerializeField] GameObject BOSS01WeaponUp;
        [SerializeField] GameObject BOSS01WeaponLeft;
        [SerializeField] Collider2D childrenColl;

        private const string IdleName = "Idle";
        private const string WalkName = "Walk";
        private const string AttackLeft1Name = "AttackLeft1";
        private const float AttackLeft1Length = 0.833f;
        private const string AttackUp1Name = "AttackUp1";
        private const float AttackUp1Length = 1.071f;

        private int moveDirection;
        private List<int> SkillList;

        private Animator anim;
        private GroundedDetector groundedDetector;
        private Transform mTransform;
        private Rigidbody2D mRigidbody2D;

        private WaitForSeconds waitForAttackLeft1;
        private WaitForSeconds waitForAttackUp1;
        private Coroutine currentCor;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            mTransform = transform;
            mRigidbody2D = GetComponent<Rigidbody2D>();
            groundedDetector = GetComponentInChildren<GroundedDetector>();

            waitForAttackLeft1 = new WaitForSeconds(AttackLeft1Length);
            waitForAttackUp1 = new WaitForSeconds(AttackUp1Length);
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
            PorcessSkillSequence();

            //出现在初始位置
            mTransform.position = InitialPosition;

            if (currentCor == null)
                currentCor = StartCoroutine(nameof(LifePoscess));
        }

        //面向玩家
        private void SetScaleToPlayer()
        {
            if (mTransform.position.x < 0)
                moveDirection = 1;
            else
                moveDirection = -1;

            if (mTransform.localScale.x * moveDirection < 0)
            {
                var scale = mTransform.localScale;
                scale.x *= -1;
                mTransform.localScale = scale;
            }
        }

        IEnumerator LifePoscess()
        {
            yield return StartCoroutine(nameof(Appearance));

            int currentIndex = 0;
            while (currentIndex < SkillList.Count)
            {
                yield return StartCoroutine(PorcessSkillIndex(SkillList[currentIndex]));
                currentIndex++;
            }
            // yield return StartCoroutine(nameof(AttackUp1));
            // yield return StartCoroutine(nameof(AttackLeft1));
            // yield return StartCoroutine(nameof(AttackUp2));
            currentCor = null;
        }

        IEnumerator Appearance()
        {
            //等待落地
            while (!groundedDetector.IsGrounded)
            {
                yield return null;
            }
            //转向
            SetScaleToPlayer();
            //根据所在位置走到4.41/-4.41位置
            anim.Play(WalkName);
            var targetPos = -moveDirection * DistanceWithPlayer;
            mRigidbody2D.velocity = Vector2.right * moveDirection * moveSpeed;
            while (Mathf.Abs(targetPos - mTransform.position.x) > 0.1f)
            {
                yield return null;
            }
            mRigidbody2D.velocity = Vector2.zero;
            anim.Play(IdleName);
        }

        //1
        IEnumerator AttackLeft1()
        {
            childrenColl.enabled = true;
            anim.Play(AttackLeft1Name);
            yield return waitForAttackLeft1;
            anim.Play(IdleName);
        }
        //2
        IEnumerator AttackUp1()
        {
            childrenColl.enabled = true;
            anim.Play(AttackUp1Name);
            yield return waitForAttackUp1;
            anim.Play(IdleName);
        }
        //3
        IEnumerator AttackUp2()
        {
            var moveDistance = Mathf.Abs(mTransform.position.x) * 2;
            var t = 2 * JumpForce / (mRigidbody2D.gravityScale * -Physics2D.gravity.y);
            mRigidbody2D.velocity = Vector2.up * JumpForce + Vector2.right * moveDirection * (moveDistance / t);
            //速度小于0时,位于玩家正上方
            while (mRigidbody2D.velocity.y > 0)
            {
                yield return null;
            }
            //放斧头
            //PoolManager.Instance.Release(BOSS01WeaponUp, mTransform.position);
            //落地
            while (!groundedDetector.IsGrounded)
            {
                yield return null;
            }
            mRigidbody2D.velocity = Vector2.zero;
            SetScaleToPlayer();
            anim.Play(IdleName);
        }
        //4
        IEnumerator AttackLeft2()
        {
            PoolManager.Instance.Release(BOSS01WeaponLeft, mTransform.position);
            yield return null;
        }

        private void PorcessSkillSequence()
        {
            SkillList = SkillRelaseSequence.Split(",").Select(int.Parse).ToList<int>();
        }

        private string PorcessSkillIndex(int index)
        {
            if (index == 1)
                return nameof(AttackLeft1);
            else if (index == 2)
                return nameof(AttackUp1);
            else if (index == 3)
                return nameof(AttackUp2);
            else
                return nameof(AttackLeft2);
        }
    }
}