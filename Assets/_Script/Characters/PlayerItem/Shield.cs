using System.Collections;
using System.Collections.Generic;
using BayatGames.SaveGameFree;
using UnityEngine;

namespace Adv
{
    public class Shield : MonoBehaviour
    {
        public bool canRecover = false;//TODO存档
        [SerializeField] bool DontFalse;
        [SerializeField] VoidEventChannel LevelEnd;
        [SerializeField] AudioData ShieldBroken;
        [SerializeField] PlayerInput input;
        [SerializeField] PlayerProperty playerProperty;
        private const string EnemyTag = "Enemy";
        private const string EnemyAttackTag = "EnemyAttack";

        private void Awake()
        {
            LevelEnd.AddListener(() =>
            {
                if (gameObject.activeSelf)
                    gameObject.SetActive(false);
            });

            input.onDown += Recover;

            gameObject.SetActive(false);

            if (SaveGame.Exists("canRecover"))
                canRecover = SaveGame.Load<bool>("canRecover");
        }

        private void Recover()
        {
            if (!canRecover) return;
            if (gameObject.activeSelf)
            {
                playerProperty.health += 1;
                playerProperty.healtChange.Broadcast(playerProperty.health);
                gameObject.SetActive(false);
            }
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
