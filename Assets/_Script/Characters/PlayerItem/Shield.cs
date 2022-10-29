using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class Shield : MonoBehaviour
    {
        [SerializeField] bool DontFalse;
        [SerializeField] VoidEventChannel LevelEnd;
        private const string EnemyTag = "Enemy";
        private const string EnemyAttackTag = "EnemyAttack";

        private void Awake()
        {
            LevelEnd.AddListener(() =>
            {
                gameObject.SetActive(false);
            });
            gameObject.SetActive(false);
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag(EnemyTag))
            {
                //TODO播放音效
                //关闭
                if (!DontFalse)
                    gameObject.SetActive(false);
            }
            else if (col.CompareTag(EnemyAttackTag))
            {
                //TODO播放音效
                //关闭
                col.gameObject.SetActive(false);
                if (!DontFalse)
                    gameObject.SetActive(false);
            }
        }
    }
}
