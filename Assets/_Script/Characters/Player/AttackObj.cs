using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class AttackObj : MonoBehaviour
    {
        [SerializeField] float ImpulseAmplitudeGain = 0.15f;
        [SerializeField] float ImpulseFrequencyGain = 0.9f;
        [SerializeField] float AttackPauseRecoverTime = 0.2f;
        [SerializeField] VoidEventChannel ClearingUIClose;
        [SerializeField] float PauseStartTimeScale = 0.2f;

        //private int fixedFrameDuration;
        private string EnemyTag = "Enemy";
        private string EnemyAttackTag = "EnemyAttack";
        private PlayerProperty playerProperty;
        private PlayerAudio playerAudio;
        private HashSet<GameObject> EnemySet = new HashSet<GameObject>();
        private HashSet<Collider2D> colSet = new HashSet<Collider2D>();
        private Coroutine AttackPauseCoroutine;

        public void StopAttackPauseCoroutine()
        {
            if (AttackPauseCoroutine != null)
            {
                StopCoroutine(AttackPauseCoroutine);
                AttackPauseCoroutine = null;
            }
        }

        private void Awake()
        {
            playerAudio = GetComponentInParent<PlayerAudio>();
            playerProperty = GetComponentInParent<PlayerProperty>();

            ClearingUIClose.AddListener(() =>
            {
                if (AttackPauseCoroutine != null)
                {
                    StopCoroutine(AttackPauseCoroutine);
                    AttackPauseCoroutine = null;
                }
                colSet.Clear();
                EnemySet.Clear();
            });
        }

        private void OnEnable()
        {
            AudioManager.Instance.PlayRandomSFX(playerAudio.AttackAudio);
            //fixedFrameDuration = 0;
        }

        private void FixedUpdate()
        {
            //fixedFrameDuration++;
        }

        private void OnDestroy()
        {
            playerProperty = null;
            EnemySet.Clear();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag.Equals(EnemyTag) || col.tag.Equals(EnemyAttackTag))
            {
                if (colSet.Contains(col)) return;
                colSet.Add(col);
                //最近的点
                var contactPoint = col.ClosestPoint(transform.position);
                //震动
                ImpulseController.Instance.ProduceImpulse(contactPoint, ImpulseAmplitudeGain, ImpulseFrequencyGain);
                //特效
                ParticleEffectController.Instance.PlayHitEffect(contactPoint);
                //停顿
                if (AttackPauseCoroutine == null)
                {
                    //StopCoroutine(AttackPauseCoroutine);
                    Time.timeScale = 1;
                    AttackPauseCoroutine = StartCoroutine(nameof(AttackPause));
                }
                else if ((AttackPauseCoroutine != null))
                {
                    Time.timeScale = 1;
                    StopCoroutine(AttackPauseCoroutine);
                    AttackPauseCoroutine = null;
                    Time.timeScale = 1;
                    AttackPauseCoroutine = StartCoroutine(nameof(AttackPause));
                }
            }
        }

        private void OnTriggerStay2D(Collider2D col)
        {
            if (col.tag.Equals(EnemyTag) && !EnemySet.Contains(col.gameObject))
            //if (col.tag.Equals(EnemyTag))
            {
                //AttackHit.Broadcast(playerProperty.ATK);

                EnemySet.Add(col.gameObject);

                if (col.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    enemy.Hitted(playerProperty.ATK);
                    // if (fixedFrameDuration <= playerProperty.PerfectDefenceFrame)
                    // {

                    // }
                    // else if (fixedFrameDuration > playerProperty.PerfectDefenceFrame)
                    // {
                    //     enemy.Hitted(playerProperty.ATK);
                    //     playerProperty.Hitted(0.5f);//敌人伤害的一部分
                    //     Debug.Log("不完美格挡");
                    // }
                }
            }
            else if (col.tag.Equals(EnemyAttackTag))//格挡目标不是怪
            {

                // if (fixedFrameDuration <= playerProperty.PerfectDefenceFrame)
                // {
                //     //完美格挡
                // }
                // else if (fixedFrameDuration > playerProperty.PerfectDefenceFrame)
                // {
                //     //不完美格挡
                //     playerProperty.Hitted(0.5f);
                //     Debug.Log("不完美格挡");
                // }
            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.tag.Equals(EnemyTag))
            {
                EnemySet.Remove(col.gameObject);
            }
            if (colSet.Contains(col))
                colSet.Remove(col);
        }

        IEnumerator AttackPause()
        {
            Time.timeScale = PauseStartTimeScale;
            float i = 0f;
            while (i < 1f)
            {
                i += Time.unscaledDeltaTime / AttackPauseRecoverTime;
                Time.timeScale = Mathf.Lerp(PauseStartTimeScale, 1f, i);
                yield return null;
            }
            Time.timeScale = 1;
            AttackPauseCoroutine = null;
        }
    }
}