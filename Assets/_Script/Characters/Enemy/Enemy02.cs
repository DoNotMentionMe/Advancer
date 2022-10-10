using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class Enemy02 : MonoBehaviour
    {
        private const string ThunderBallName = "Thunder Ball(Clone)";
        private Vector3 ThunderBallReleasePos = new Vector3(-0.00800000038f, 2.2650001f, 0);

        [SerializeField] float moveSpeed;
        [SerializeField] float attackStartInterval = 1f;
        [SerializeField] float attackInterval = 2f;
        [SerializeField] GameObject ThunderBallPrefab;
        [SerializeField] VoidEventChannel LevelEnd;
        [SerializeField] GameObjectEventChannel EnemyDied;

        private Transform mTransform;
        private Coroutine AttackCoro;
        private WaitForSeconds waitForAttackStartInterval;
        private WaitForSeconds waitForAttackInterval;
        private GameObject MyBall;
        private Dictionary<GameObject, GameObject> MyReleasedDic = new Dictionary<GameObject, GameObject>();

        private void Awake()
        {
            mTransform = transform;
            waitForAttackStartInterval = new WaitForSeconds(attackStartInterval);
            waitForAttackInterval = new WaitForSeconds(attackInterval);
        }

        private void OnEnable()
        {
            // Fail.AddListener(SetActiveFalse);
            LevelEnd.AddListener(SetActiveFalse);
            if (AttackCoro == null)
                AttackCoro = StartCoroutine(nameof(AirAttack));
        }

        private void OnDisable()
        {
            // Fail.RemoveListenner(SetActiveFalse);
            LevelEnd.RemoveListenner(SetActiveFalse);
            EnemyDied.Broadcast(gameObject);
            StopAllCoroutines();
            AttackCoro = null;
        }

        private void OnDestroy()
        {
            mTransform = null;
        }

        private void SetActiveFalse()
        {
            gameObject.SetActive(false);
        }

        IEnumerator AirAttack()
        {
            //重置位置（0,6）
            mTransform.position = Vector3.up * 6;
            //移动到（0，2.5）
            var targetPos = Vector3.up * 2.5f;
            while (Vector3.Distance(mTransform.position, targetPos) > 0.1f)
            {
                mTransform.position -= Time.deltaTime * moveSpeed * Vector3.up;
                yield return null;
            }
            mTransform.position = Vector3.up * 2.5f;

            yield return waitForAttackStartInterval;

            //不断循环放球，直到被击中
            while (true)
            {
                var obj = PoolManager.Instance.Release(ThunderBallPrefab, ThunderBallReleasePos);
                if (!MyReleasedDic.ContainsKey(obj))
                    MyReleasedDic.Add(obj, gameObject);
                yield return waitForAttackInterval;
            }


        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.name.Equals(ThunderBallName))
            {
                if (!MyReleasedDic.ContainsKey(col.gameObject)) return;
                if (!MyReleasedDic[col.gameObject].Equals(gameObject)) return;

                MyReleasedDic.Clear();
                StopCoroutine(AttackCoro);
                AttackCoro = null;
                col.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }
}