using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class Level3 : BaseLevelModule
    {
        public override string Key => nameof(Level3);

        [SerializeField] GameObject Enemy04;//弓兵
        [SerializeField] float Level3Duration;
        [SerializeField] float Level3ReleaseInterval1;
        [SerializeField] float Level3ReleaseInterval2;
        [SerializeField] float Level3ReleaseInterval3;

        private WaitForSeconds waitForReleaseInterval1;
        private WaitForSeconds waitForReleaseInterval2;
        private WaitForSeconds waitForReleaseInterval3;

        protected override void Awake()
        {
            base.Awake();
            waitForReleaseInterval1 = new WaitForSeconds(Level3ReleaseInterval1);
            waitForReleaseInterval2 = new WaitForSeconds(Level3ReleaseInterval2);
            waitForReleaseInterval3 = new WaitForSeconds(Level3ReleaseInterval3);
        }

        protected override void ReleaseEnemyEvent()
        {
            waitForReleaseInterval1 = new WaitForSeconds(Level3ReleaseInterval1);
            waitForReleaseInterval2 = new WaitForSeconds(Level3ReleaseInterval2);
            waitForReleaseInterval3 = new WaitForSeconds(Level3ReleaseInterval3);
            StartCoroutine(nameof(Release));
        }

        protected override void RunAfterEnemysDied()
        {

        }

        IEnumerator Release()
        {
            GameObject GenerationPos = null;
            var startTime = Time.time;
            while (Time.time - startTime < Level3Duration)
            {
                var random = Random.Range(1, 3);//第二次随机用于释放位置
                if (random == 1)
                    GenerationPos = EnemyGenerationPosition1;
                else if (random == 2)
                    GenerationPos = EnemyGenerationPosition3;

                ReleaseEnemy(Enemy04, GenerationPos.transform.position);
                if (Time.time - startTime < Level3Duration / 3)
                    yield return waitForReleaseInterval1;
                else if (Time.time - startTime > Level3Duration / 3
                        && Time.time - startTime < Level3Duration * 2 / 3)
                    yield return waitForReleaseInterval2;
                else if (Time.time - startTime > Level3Duration * 2 / 3)
                    yield return waitForReleaseInterval3;
            }
        }
    }
}