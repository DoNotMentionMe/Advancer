using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    //枪兵特别关卡
    public class LevelKIA1 : BaseLevelModule
    {
        public override string Key => nameof(LevelKIA1);

        [SerializeField] GameObject Enemy05;
        [SerializeField] float LevelKIA1Duration;
        [SerializeField] float LevelKIA1ReleaseInterval;

        private WaitForSeconds waitForReleaseInterval;

        protected override void Awake()
        {
            base.Awake();
            waitForReleaseInterval = new WaitForSeconds(LevelKIA1ReleaseInterval);
        }

        protected override void ReleaseEnemyEvent()
        {
            waitForReleaseInterval = new WaitForSeconds(LevelKIA1ReleaseInterval);
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
            while (Time.time - startTime < LevelKIA1Duration)
            {
                ReleaseEnemy(Enemy05, EnemyGenerationPosition1.transform.position);

                yield return waitForReleaseInterval;

            }

            liveEnemyList.Remove(nullObj);//去除空对象
            Destroy(nullObj);
        }
    }
}
