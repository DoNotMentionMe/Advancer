using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class Enemy05 : MonoBehaviour
    {
        private static List<int> faceList = new List<int>() { 1, 2, 3 };


        [SerializeField] VoidEventChannel LevelEnd;
        [SerializeField] GameObjectEventChannel EnemyDied;
        [SerializeField] bool DontSetFalse;
        [SerializeField] float attack;
        [SerializeField] float AppearTime;
        [SerializeField] Vector2 AppearPosLeft;
        [SerializeField] Vector2 AppearPosRight;
        [SerializeField] Vector2 AppearPosUp;
        [SerializeField] AnimationClip AttackClip;
        [SerializeField] SpriteRenderer WeaponRenderer;
        [SerializeField] VoidEventChannel attackHit;

        private const string Idle = "Idle";
        private const string PlayerTag = "Player";
        private const string PlayerAttackTag = "PlayerAttack";

        private int faceRandom = 0;
        private int faceDirection = 0;
        private float AttackLength;
        private string AttackName;
        private Quaternion initalRota;
        private Transform mTransform;
        private Animator anim;
        private SpriteRenderer spriteRenderer;
        private Collider2D mColl;
        private WaitForSeconds waitForAttackLength;

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
            LevelEnd.AddListener(SetActiveFalse);
            StartCoroutine(nameof(AttackCor));
        }

        private void OnDisable()
        {
            LevelEnd.RemoveListenner(SetActiveFalse);
            EnemyDied.Broadcast(gameObject);
            if (faceRandom != 0)
            {
                faceList.Add(faceRandom);
                faceRandom = 0;
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
            Debug.Log("取出位置：" + faceRandom + "faceList: " + ForEachList(faceList));
            SetAppearPos(faceRandom);
            mColl.enabled = false;
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
            while (Time.time - startTime < AttackLength * 1 / 8)
            { yield return null; }
            mColl.enabled = true;
            while (Time.time - startTime < AttackLength)
            { yield return null; }
            mColl.enabled = false;
            anim.Play("Idle");
            if (faceRandom != 0)
            {
                faceList.Add(faceRandom);
                Debug.Log("放回位置：" + faceRandom + "faceList: " + ForEachList(faceList));
                faceRandom = 0;
            }

            if (!DontSetFalse)
                gameObject.SetActive(false);
            //整个流程耗时: 1.67s
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag.Equals(PlayerTag))
            {
                mColl.enabled = false;
                if (col.gameObject.TryGetComponent<PlayerProperty>(out PlayerProperty playerProperty))
                {
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
                StopAllCoroutines();
                attackHit.Broadcast();
                mColl.enabled = false;
                anim.Play("Idle");
                if (faceRandom != 0)
                {
                    faceList.Add(faceRandom);
                    faceRandom = 0;
                }
                Debug.Log("放回位置：" + faceRandom + "faceList: " + ForEachList(faceList));
                gameObject.SetActive(false);
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