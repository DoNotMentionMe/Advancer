using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class Enemy02 : MonoBehaviour
    {
        [SerializeField] float moveSpeed;
        [SerializeField] float attackInterval = 1f;
        [SerializeField] GameObject ThunderBallPrefab;

        private Transform mTransform;
        private Coroutine AttackCoro;
        private WaitForSeconds waitForAttackInterval;

        private void Awake()
        {
            mTransform = transform;
            waitForAttackInterval = new WaitForSeconds(attackInterval);
        }

        private void OnEnable()
        {
            if (AttackCoro == null)
                AttackCoro = StartCoroutine(nameof(AirAttack));
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            AttackCoro = null;
        }

        private void OnDestroy()
        {
            mTransform = null;
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

            yield return waitForAttackInterval;

            PoolManager.Instance.Release(ThunderBallPrefab, mTransform.position);

            AttackCoro = null;

            gameObject.SetActive(false);
        }
    }
}