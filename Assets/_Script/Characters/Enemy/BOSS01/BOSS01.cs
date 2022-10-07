using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class BOSS01 : MonoBehaviour
    {
        public enum BossTage
        {
            One, Two, Three
        }
        [SerializeField] float LifeTime = 30f;
        [SerializeField] string SkillRelaseSequence;
        [SerializeField] float moveSpeed;
        [SerializeField] float JumpForce;
        [SerializeField] float DistanceWithPlayer = 4.4f;
        [SerializeField] float AttackLeft2Interval;
        [SerializeField] float TageChangeInterval = 1;
        [SerializeField] Color TageTwoColor;
        [SerializeField] Color TageThreeColor;
        [SerializeField] Vector2 InitialPosition;
        [SerializeField] Vector2 AttackLeft2ReleasePos = new Vector2(7, -1.7f);
        [SerializeField] GameObject BOSS01WeaponUp;
        [SerializeField] GameObject BOSS01WeaponLeft;
        [SerializeField] Collider2D childrenColl;
        [SerializeField] FloatEventChannel bossTageChange;
        [SerializeField] VoidEventChannel Fail;
        [SerializeField] VoidEventChannel Level2Achieve;

        private const string IdleName = "Idle";
        private const string WalkName = "Walk";
        private const string AttackLeft1Name = "AttackLeft1";
        private float AttackLeft1Length = 0.833f;
        private const string AttackUp1Name = "AttackUp1";
        private float AttackUp1Length = 1.071f;

        [SerializeField] bool TageTwoChange = false;
        [SerializeField] bool TageThreeChange = false;
        private int moveDirection;
        private List<int> SkillList;

        private Animator anim;
        private GroundedDetector groundedDetector;
        private Transform mTransform;
        private Rigidbody2D mRigidbody2D;
        private SpriteRenderer mRenderer;
        [SerializeField] BossTage bossTage = BossTage.One;

        private WaitForSeconds waitForAttackLeft1;
        private WaitForSeconds waitForAttackUp1;
        private WaitForSeconds waitForAttackLeft2Interval;
        private WaitForSeconds waitForTageChangeInterval;
        private Coroutine currentCor;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            mTransform = transform;
            mRigidbody2D = GetComponent<Rigidbody2D>();
            groundedDetector = GetComponentInChildren<GroundedDetector>();
            mRenderer = GetComponent<SpriteRenderer>();

            waitForAttackLeft1 = new WaitForSeconds(AttackLeft1Length);
            waitForAttackUp1 = new WaitForSeconds(AttackUp1Length);
            waitForAttackLeft2Interval = new WaitForSeconds(AttackLeft2Interval);
            waitForTageChangeInterval = new WaitForSeconds(TageChangeInterval);

            Fail.AddListener(() => { gameObject.SetActive(false); });
        }

        private void OnDisable()
        {
            ResetProperty();
            StopAllCoroutines();
            currentCor = null;
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

            //1
            // yield return StartCoroutine(nameof(AttackUp1));
            // yield return StartCoroutine(nameof(AttackLeft1));
            // yield return StartCoroutine(nameof(AttackUp2));

            //2
            // int currentIndex = 0;
            // while (currentIndex < SkillList.Count)
            // {
            //     yield return StartCoroutine(PorcessSkillIndex(SkillList[currentIndex]));
            //     currentIndex++;
            // }

            //3
            var startTime = Time.time;
            while (Time.time - startTime < LifeTime)
            {
                if (!TageTwoChange && Time.time - startTime > (LifeTime / 3))
                {
                    bossTage = BossTage.Two;
                }
                else if (!TageThreeChange && Time.time - startTime > (2 * LifeTime / 3) && bossTage == BossTage.Two)
                {
                    bossTage = BossTage.Three;
                }

                if (!TageTwoChange && bossTage == BossTage.Two)
                {
                    anim.speed = 1.333f;
                    AttackLeft1Length *= 0.75f;
                    AttackUp1Length *= 0.75f;
                    waitForAttackLeft1 = new WaitForSeconds(AttackLeft1Length);
                    waitForAttackUp1 = new WaitForSeconds(AttackUp1Length);
                    AttackLeft2Interval *= 0.75f;
                    bossTageChange.Broadcast(2);
                    TageTwoChange = true;
                    mRenderer.color = TageTwoColor;

                    yield return waitForTageChangeInterval;
                }
                else if (!TageThreeChange && bossTage == BossTage.Three && TageTwoChange)
                {
                    anim.speed = 1.55f;
                    AttackLeft1Length /= 0.75f;
                    AttackUp1Length /= 0.75f;
                    AttackLeft1Length *= 0.6451f;
                    AttackUp1Length *= 0.6451f;
                    waitForAttackLeft1 = new WaitForSeconds(AttackLeft1Length);
                    waitForAttackUp1 = new WaitForSeconds(AttackUp1Length);
                    AttackLeft2Interval /= 0.75f;
                    AttackLeft2Interval *= 0.5f;
                    mRigidbody2D.gravityScale = 8;
                    JumpForce *= 1.2f;
                    bossTageChange.Broadcast(3);
                    TageThreeChange = true;
                    mRenderer.color = TageThreeColor;

                    yield return waitForTageChangeInterval;
                }

                var random = Random.Range(1, 5);
                yield return StartCoroutine(PorcessSkillIndex(random));
            }

            Level2Achieve.Broadcast();

            currentCor = null;
        }

        private void ResetProperty()
        {
            anim.speed = 1f;
            AttackLeft1Length /= 0.6451f;
            AttackUp1Length /= 0.6451f;
            waitForAttackLeft1 = new WaitForSeconds(AttackLeft1Length);
            waitForAttackUp1 = new WaitForSeconds(AttackUp1Length);
            AttackLeft2Interval /= 0.5f;
            mRigidbody2D.gravityScale = 5;
            JumpForce /= 1.2f;
            bossTageChange.Broadcast(1);
            TageThreeChange = false;
            TageTwoChange = false;
            bossTage = BossTage.One;
        }

        IEnumerator Appearance()
        {
            Debug.Log(groundedDetector.IsGrounded);
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
            SetPos();
            anim.Play(IdleName);
        }

        //1,劈左
        IEnumerator AttackLeft1()
        {
            Debug.Log("1、劈左");
            childrenColl.enabled = true;
            anim.Play(AttackLeft1Name);
            yield return waitForAttackLeft1;
            anim.Play(IdleName);
        }
        //2，劈上
        IEnumerator AttackUp1()
        {
            Debug.Log("2、劈上");
            childrenColl.enabled = true;
            anim.Play(AttackUp1Name);
            yield return waitForAttackUp1;
            anim.Play(IdleName);
        }
        //3，掉斧头
        IEnumerator AttackUp2()
        {
            Debug.Log("3、掉斧头");
            var moveDistance = Mathf.Abs(mTransform.position.x) * 2;
            var t = 2 * JumpForce / (mRigidbody2D.gravityScale * -Physics2D.gravity.y);
            mRigidbody2D.velocity = Vector2.up * JumpForce + Vector2.right * moveDirection * (moveDistance / t);
            //速度小于0时,位于玩家正上方
            while (mRigidbody2D.velocity.y > 0)
            {
                yield return null;
            }
            //放斧头
            PoolManager.Instance.Release(BOSS01WeaponUp, mTransform.position);
            //落地
            while (!groundedDetector.IsGrounded)
            {
                yield return null;
            }
            mRigidbody2D.velocity = Vector2.zero;
            SetScaleToPlayer();
            SetPos();
            anim.Play(IdleName);

        }
        //4，飞斧头 
        IEnumerator AttackLeft2()
        {
            Debug.Log("4、飞斧头");
            Vector2 releasePos;
            if (mTransform.position.x > 0)
                releasePos = AttackLeft2ReleasePos;
            else
            {
                releasePos = AttackLeft2ReleasePos;
                releasePos.x *= -1;
            }
            PoolManager.Instance.Release(BOSS01WeaponLeft, releasePos);
            yield return waitForAttackLeft2Interval;
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

        private void SetPos()
        {
            var pos = mTransform.position;
            var currentDirection = -mTransform.localScale.x;
            pos.x = currentDirection * DistanceWithPlayer;
            mTransform.position = pos;
        }
    }
}