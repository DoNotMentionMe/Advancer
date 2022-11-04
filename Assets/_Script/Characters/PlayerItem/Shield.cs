using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class Shield : MonoBehaviour
    {
        [SerializeField] bool DontFalse;
        [SerializeField] VoidEventChannel LevelEnd;
        [SerializeField] AudioData ShieldBroken;
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
                //播放音效
                AudioManager.Instance.PlaySFX(ShieldBroken);
                //关闭
                if (!DontFalse)
                    gameObject.SetActive(false);
            }
            else if (col.CompareTag(EnemyAttackTag))
            {
                //播放音效
                AudioManager.Instance.PlaySFX(ShieldBroken);
                //关闭
                col.gameObject.SetActive(false);
                if (!DontFalse)
                    gameObject.SetActive(false);
            }
        }
    }
}
