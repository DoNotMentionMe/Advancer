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
        [SerializeField] GameObject enemyHittedEffect;
        [SerializeField] GameObject HitEffect;

        //private int fixedFrameDuration;
        private string EnemyTag = "Enemy";
        private string EnemyAttackTag = "EnemyAttack";
        private PlayerProperty playerProperty;
        private PlayerAudio playerAudio;
        private HashSet<GameObject> EnemySet = new HashSet<GameObject>();
        private HashSet<Collider2D> colSet = new HashSet<Collider2D>();
        private Coroutine AttackPauseCoroutine;
        private Vector2 PlayerMidPoint = new Vector2(0, -2.3f);

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
                //????????????
                var contactPoint = col.ClosestPoint(transform.position);
                //??????
                ImpulseController.Instance.ProduceImpulse(contactPoint, ImpulseAmplitudeGain, ImpulseFrequencyGain);
                //??????
                PoolManager.Instance.Release(HitEffect, contactPoint);
                //ParticleEffectController.Instance.PlayHitEffect(contactPoint);
                // var direction = contactPoint - PlayerMidPoint;
                // var angle = Vector2.Angle(Vector2.right, direction);
                // PoolManager.Instance.Release(enemyHittedEffect, contactPoint, Quaternion.Euler(180 - angle, 90, 0));
                //??????
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
                    //     playerProperty.Hitted(0.5f);//????????????????????????
                    //     Debug.Log("???????????????");
                    // }
                }
            }
            else if (col.tag.Equals(EnemyAttackTag))//?????????????????????
            {

                // if (fixedFrameDuration <= playerProperty.PerfectDefenceFrame)
                // {
                //     //????????????
                // }
                // else if (fixedFrameDuration > playerProperty.PerfectDefenceFrame)
                // {
                //     //???????????????
                //     playerProperty.Hitted(0.5f);
                //     Debug.Log("???????????????");
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