using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class Level4 : BaseLevelModule
    {
        public override string Key => nameof(Level4);

        [SerializeField] GameObject Enemy05;
        [SerializeField] float Level4Duration;
        [SerializeField] float Level4ReleaseInterval;

        private WaitForSeconds waitForReleaseInterval;

        protected override void Awake()
        {
            base.Awake();
            waitForReleaseInterval = new WaitForSeconds(Level4ReleaseInterval);
        }

        protected override void ReleaseEnemyEvent()
        {
            waitForReleaseInterval = new WaitForSeconds(Level4ReleaseInterval);
            StartCoroutine(nameof(ReleaseEnemy));
        }

        protected override void RunAfterEnemysDied()
        {

        }

        IEnumerator ReleaseEnemy()
        {
            //GameObject GenerationPos = null;
            var nullObj = new GameObject();
            liveEnemyList.Add(nullObj);//防止提前结束

            var startTime = Time.time;
            while (Time.time - startTime < Level4Duration)
            {
                ReleaseEnemy(Enemy05, EnemyGenerationPosition1.transform.position);

                yield return waitForReleaseInterval;

            }

            liveEnemyList.Remove(nullObj);//去除空对象
        }
    }
}