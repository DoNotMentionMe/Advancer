using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class Enemy05 : MonoBehaviour
    {
        private static List<int> faceList = new List<int>() { 1, 2, 3 };

        [SerializeField] bool DebugText;
        [SerializeField] VoidEventChannel LevelEnd;
        [SerializeField] GameObjectEventChannel EnemyDied;
        [SerializeField] bool DontSetFalse;
        [SerializeField] float attack;
        [SerializeField] float AppearTime;
        [SerializeField] float HittedBackSpeed;
        [SerializeField] Vector2 AppearPosLeft;
        [SerializeField] Vector2 AppearPosRight;
        [SerializeField] Vector2 AppearPosUp;
        [SerializeField] AnimationClip AttackClip;
        [SerializeField] SpriteRenderer WeaponRenderer;
        [SerializeField] VoidEventChannel attackHit;
        [SerializeField] PlayerHittedEventChannel playerHitted;
        [SerializeField] GameObject HittedWeapon_right;
        [SerializeField] GameObject HittedWeapon_left;
        [SerializeField] GameObject HittedWeapon_upLeft;
        [SerializeField] GameObject HittedWeapon_upRight;
        [SerializeField] AudioData HittedSFX;

        private const string Idle = "Idle";
        private const string PlayerTag = "Player";
        private const string PlayerAttackTag = "PlayerAttack";

        private bool IsTirggerEnter = false;
        private bool HasAttackOnce = false;
        private int faceRandom = 0;
        private int faceDirection = 0;
        private float AttackLength;
        private string AttackName;
        private Quaternion initalRota;
        private Transform mTransform;
        private Transform playerTransform;
        private Animator anim;
        private SpriteRenderer spriteRenderer;
        private Collider2D mColl;
        private WaitForSeconds waitForAttackLength;
        private Coroutine HittedBackCoroutine;
        private Coroutine AttackCoroutine;

        private void Awake()
        {
            mTransform = transform;
            anim = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            mColl = GetComponent<Collider2D>();
            initalRota = new Quaternion(0, 0, 0, 0);
            AttackLength = AttackClip.length;
            AttackName = AttackClip.name;
            waitForAttackLength = new WaitForSeconds(AttackLength);
        }

        private void OnEnable()
        {
            HasAttackOnce = false;
            IsTirggerEnter = false;
            LevelEnd.AddListener(SetActiveFalse);
            if (AttackCoroutine != null)
            {
                StopCoroutine(AttackCoroutine);
                AttackCoroutine = null;
            }
            AttackCoroutine = StartCoroutine(nameof(AttackCor));
            WeaponRenderer.enabled = true;
        }

        private void OnDisable()
        {
            mColl.enabled = false;
            HasAttackOnce = false;
            IsTirggerEnter = false;
            LevelEnd.RemoveListenner(SetActiveFalse);
            EnemyDied.Broadcast(gameObject);
            if (faceRandom != 0)
            {
                faceList.Add(faceRandom);
                faceRandom = 0;
            }
            HittedBackCoroutine = null;

            if (AttackCoroutine != null)
            {
                StopCoroutine(AttackCoroutine);
                AttackCoroutine = null;
            }
        }

        private void OnDestroy()
        {
            mTransform = null;
            anim = null;
            spriteRenderer = null;
            mColl = null;
        }

        IEnumerator AttackCor()
        {
            int randomIndex = 0;
            if (faceList.Count > 0)
            {
                randomIndex = Random.Range(0, faceList.Count);
                faceRandom = faceList[randomIndex];
                faceList.Remove(faceRandom);
            }
            else
            {
                faceRandom = 0;
                faceList.Remove(faceRandom);
            }
            if (DebugText)
                Debug.Log("取出位置：" + faceRandom + "faceList: " + ForEachList(faceList));
            SetAppearPos(faceRandom);
            anim.Play("Idle");
            float t = 0f;
            var color = spriteRenderer.color;
            while (t < 1f)
            {
                t += Time.deltaTime / AppearTime;
                color.a = Mathf.Lerp(0f, 1f, t);
                spriteRenderer.color = color;
                WeaponRenderer.color = color;
                yield return null;
            }
            var startTime = Time.time;
            anim.Play(AttackName);
            if (!IsTirggerEnter)
                mColl.enabled = true;
            while (Time.time - startTime < AttackLength)
            { yield return null; }
            //结尾
            mColl.enabled = false;
            anim.Play("Idle");
            if (faceRandom != 0)
            {
                faceList.Add(faceRandom);
                if (DebugText)
                    Debug.Log("放回位置：" + faceRandom + "faceList: " + ForEachList(faceList));
                faceRandom = 0;
            }

            AttackCoroutine = null;
            if (!DontSetFalse)
                gameObject.SetActive(false);
            //整个流程耗时: 1.67s
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            IsTirggerEnter = true;
            if (col.tag.Equals(PlayerTag) && !HasAttackOnce)
            {
                mColl.enabled = false;
                HasAttackOnce = true;
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
                    //碰到玩家时已经时生命周期结尾了 不需要在做一个次结尾
                    //StopAllCoroutines();
                    // anim.Play("Idle");
                    //faceList.Add(faceRandom);
                    // Debug.Log("放回位置：" + faceRandom + "faceList: " + ForEachList(faceList));
                    // gameObject.SetActive(false);
                }
            }
            else if (col.tag.Equals(PlayerAttackTag))//被命中
            {
                mColl.enabled = false;
                StopAllCoroutines();
                AttackCoroutine = null;
                AudioManager.Instance.PlayRandomSFX(HittedSFX);
                attackHit.Broadcast();
                //anim.Play("Idle");
                int random = 0;
                if (faceRandom != 0)
                {
                    random = faceRandom;
                    faceList.Add(faceRandom);
                    faceRandom = 0;
                    if (DebugText)
                        Debug.Log("放回位置：" + faceRandom + "faceList: " + ForEachList(faceList));
                }
                //释放飞出的武器
                if (random == 1)
                    PoolManager.Instance.Release(HittedWeapon_left, mTransform.position, Quaternion.Euler(0, 0, 0));
                else if (random == 2)
                    PoolManager.Instance.Release(HittedWeapon_right, mTransform.position, Quaternion.Euler(0, 0, 0));
                else if (random == 3)//playerScaleX==1，顺时针往左
                {
                    if (playerTransform == null)
                        playerTransform = PlayerFSM.player.transform;
                    if (playerTransform.localScale.x > 0)
                        PoolManager.Instance.Release(HittedWeapon_upLeft, mTransform.position, Quaternion.Euler(0, 0, -90));
                    else if (playerTransform.localScale.x < 0)
                        PoolManager.Instance.Release(HittedWeapon_upRight, mTransform.position, Quaternion.Euler(0, 0, -90));
                }

                //击退效果
                if (HittedBackCoroutine == null && gameObject.activeSelf)
                    HittedBackCoroutine = StartCoroutine(HittedBack(random));

            }
        }

        private void SetAppearPos(int random)
        {
            if (random == 1)
            {
                faceDirection = 1;
                mTransform.position = AppearPosLeft;
                mTransform.rotation = initalRota;
                SetLocalScale();
            }
            else if (random == 2)
            {
                faceDirection = -1;
                mTransform.position = AppearPosRight;
                mTransform.rotation = initalRota;
                SetLocalScale();
            }
            else if (random == 3)
            {
                faceDirection = -1;
                mTransform.position = AppearPosUp;
                mTransform.rotation = initalRota;
                mTransform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
                SetLocalScale();
            }
            else if (random == 0)
            {
                Debug.Log("Enemy05没位置了");
                gameObject.SetActive(false);
            }
        }

        IEnumerator HittedBack(int faceRandom)
        {
            WeaponRenderer.enabled = false;

            float t = 0f;
            var color = spriteRenderer.color;
            while (t < 1f)
            {
                t += Time.deltaTime * 1.2f / AppearTime;
                color.a = Mathf.Lerp(1f, 0f, t);
                spriteRenderer.color = color;
                if (faceRandom == 3)
                    mTransform.Translate(Vector3.right * HittedBackSpeed * Time.deltaTime);
                else if (faceRandom == 1)
                    mTransform.Translate(Vector3.left * HittedBackSpeed * Time.deltaTime);
                else if (faceRandom == 2)
                    mTransform.Translate(Vector3.right * HittedBackSpeed * Time.deltaTime);
                yield return null;
            }

            gameObject.SetActive(false);
            HittedBackCoroutine = null;
        }

        private void SetLocalScale()
        {
            if (mTransform.localScale.x * faceDirection < 0)
            {
                var Scale = mTransform.localScale;
                Scale.x *= -1;
                mTransform.localScale = Scale;
            }
        }

        private void SetActiveFalse()
        {
            gameObject.SetActive(false);
        }

        private string ForEachList(List<int> list)
        {
            string str = "";
            if (list.Count == 0) return str = "";
            for (int i = 0; i < list.Count; i++)
            {
                if (i == 0)
                    str += list[i].ToString();
                else
                    str += "," + list[i];
            }
            return str;
        }
    }
}