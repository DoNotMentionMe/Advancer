using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class Level3 : BaseLevelModule
    {
        public override string Key => nameof(Level3);

        [SerializeField] GameObject Enemy01;//小猪
        [SerializeField] GameObject Enemy02;//雷鸟
        [SerializeField] GameObject Enemy03;//刀客
        [SerializeField] GameObject Enemy04;//弓箭手
        [SerializeField] GameObject Enemy05;//枪兵
        [SerializeField] float Level3Duration;
        [SerializeField] float Level3ReleaseInterval1;//敌人释放间隔
        [SerializeField] float Level3ReleaseInterval2;//没用
        [SerializeField] float Level3ReleaseInterval3;//Enemy05释放前摇

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
            var nullObj = new GameObject();
            liveEnemyList.Add(nullObj);//防止提前结束

            GameObject obj = null;
            GameObject GenerationPos = null;
            var startTime = Time.time;
            while (Time.time - startTime < Level3Duration)
            {
                int random = 0;
                random = Random.Range(1, 3);
                if (random == 1)
                {
                    obj = Enemy01;
                    random = Random.Range(1, 3);//第二次随机用于释放位置
                    if (random == 1)
                        GenerationPos = EnemyGenerationPosition1;
                    else if (random == 2)
                        GenerationPos = EnemyGenerationPosition3;

                    ReleaseEnemy(obj, GenerationPos.transform.position);
                    if (CheckLiveListTheEnemyCount(Enemy03.name) == 0)
                    {
                        StartCoroutine(nameof(ReleaseEnemy05));
                    }
                }
                else if (random == 2)
                {
                    obj = Enemy02;
                    GenerationPos = EnemyGenerationPosition2;
                    ReleaseEnemy(obj, GenerationPos.transform.position);
                    if (CheckLiveListTheEnemyCount(Enemy03.name) == 0)
                    {
                        StartCoroutine(nameof(ReleaseEnemy05));
                    }
                }
                else if (random == 3)
                {
                    obj = Enemy03;
                    random = Random.Range(1, 3);//第二次随机用于释放位置
                    if (random == 1)
                        GenerationPos = EnemyGenerationPosition1;
                    else if (random == 2)
                        GenerationPos = EnemyGenerationPosition3;

                    ReleaseEnemy(obj, GenerationPos.transform.position);
                    if (CheckLiveListTheEnemyCount(Enemy03.name) <= 1)
                    {
                        StartCoroutine(nameof(ReleaseEnemy05));
                    }
                }
                else if (random == 4)
                {
                    obj = Enemy05;
                }

                // if (random == 1 || random >= 3)
                //     random = Random.Range(1, 3);//第二次随机用于释放位置
                // if (random == 1)
                //     GenerationPos = EnemyGenerationPosition1;
                // else if (random == 2)
                //     GenerationPos = EnemyGenerationPosition3;

                // ReleaseEnemy(obj, GenerationPos.transform.position);
                // if (Time.time - startTime < Level3Duration / 3)
                yield return waitForReleaseInterval1;
                // else if (Time.time - startTime > Level3Duration / 3
                //         && Time.time - startTime < Level3Duration * 2 / 3)
                //     yield return waitForReleaseInterval2;
                // else if (Time.time - startTime > Level3Duration * 2 / 3)
                //     yield return waitForReleaseInterval3;
            }

            liveEnemyList.Remove(nullObj);
            Destroy(nullObj);
        }

        IEnumerator ReleaseEnemy05()
        {
            yield return waitForReleaseInterval3;
            ReleaseEnemy(Enemy05, EnemyGenerationPosition3.transform.position);
        }
    }
}