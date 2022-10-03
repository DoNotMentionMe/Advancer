using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class AttackObj : MonoBehaviour
    {
        [SerializeField] FloatEventChannel AttackHit;

        private int fixedFrameDuration;
        private string EnemyTag = "Enemy";
        private string EnemyAttackTag = "EnemyAttack";
        private PlayerProperty playerProperty;
        private HashSet<GameObject> EnemySet = new HashSet<GameObject>();

        private void OnEnable()
        {
            fixedFrameDuration = 0;
            playerProperty = GetComponentInParent<PlayerProperty>();
        }

        private void FixedUpdate()
        {
            fixedFrameDuration++;
        }

        private void OnDestroy()
        {
            playerProperty = null;
            EnemySet.Clear();
        }

        private void OnTriggerStay2D(Collider2D col)
        {
            if (col.tag.Equals(EnemyTag) && !EnemySet.Contains(col.gameObject))
            {
                //AttackHit.Broadcast(playerProperty.ATK);
                EnemySet.Add(col.gameObject);

                if (col.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    if (fixedFrameDuration <= playerProperty.PerfectDefenceFrame)
                    {
                        enemy.Hitted(playerProperty.ATK);
                    }
                    else if (fixedFrameDuration > playerProperty.PerfectDefenceFrame)
                    {
                        enemy.Hitted(playerProperty.ATK);
                        playerProperty.Hitted(0.5f);//敌人伤害的一部分
                        Debug.Log("不完美格挡");
                    }
                }
            }
            else if (col.tag.Equals(EnemyAttackTag))//格挡目标不是怪
            {
                if (fixedFrameDuration <= playerProperty.PerfectDefenceFrame)
                {
                    //完美格挡
                }
                else if (fixedFrameDuration > playerProperty.PerfectDefenceFrame)
                {
                    //不完美格挡
                    playerProperty.Hitted(0.5f);
                    Debug.Log("不完美格挡");
                }
            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.tag.Equals(EnemyTag))
            {
                EnemySet.Remove(col.gameObject);
            }
        }
    }
}