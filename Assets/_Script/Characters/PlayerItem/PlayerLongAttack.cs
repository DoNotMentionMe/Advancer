using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerLongAttack : MonoBehaviour
    {
        public enum AttackDirection
        { Left, Right }

        [SerializeField] AttackDirection attackDirection;
        [SerializeField] float moveSpeed;

        private const string EnemyTag = "Enemy";
        private const string EnemyAttackTag = "EnemyAttack";
        private const string ReceptorTag = "PlayerLongAttackReceptor";

        private float moveDirection;
        private Transform mTransform;
        // private HashSet<GameObject> EnemySet = new HashSet<GameObject>();

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag.Equals(ReceptorTag))
            {
                col.transform.parent.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }
        }

        private void OnTriggerStay2D(Collider2D col)
        {
            // if (col.tag.Equals(ReceptorTag))
            // {
            //     Debug.Log("触发");
            //     col.transform.parent.gameObject.SetActive(false);
            //     gameObject.SetActive(false);
            //     //消灭敌人爆炸音效
            // }

            // if (col.tag.Equals(EnemyTag) && !EnemySet.Contains(col.gameObject))
            // {

            //     EnemySet.Add(col.gameObject);
            //     //消灭敌人
            //     col.gameObject.SetActive(true);
            //     gameObject.SetActive(false);
            //     
            // }
            // else if (col.tag.Equals(EnemyAttackTag))//格挡目标不是怪
            // {
            //     col.gameObject.SetActive(false);
            // }
        }

        // private void OnTriggerExit2D(Collider2D col)
        // {
        //     if (col.tag.Equals(EnemyTag))
        //     {
        //         EnemySet.Remove(col.gameObject);
        //     }
        // }

        private void Awake()
        {
            mTransform = transform;
        }

        private void OnEnable()
        {
            if (attackDirection == AttackDirection.Left)
                moveDirection = -1;
            else if (attackDirection == AttackDirection.Right)
                moveDirection = 1;
            StartCoroutine(nameof(Move));
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


        IEnumerator Move()
        {
            while (gameObject.activeSelf)
            {
                mTransform.Translate(moveDirection * Vector2.right * moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }
}