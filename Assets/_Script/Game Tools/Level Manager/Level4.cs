using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class Level4 : BaseLevelModule
    {
        public override string Key => nameof(Level4);

        [SerializeField] GameObject Enemy01;//小猪
        [SerializeField] GameObject Enemy02;//雷鸟
        [SerializeField] GameObject Enemy03;//刀客
        [SerializeField] GameObject Enemy04;//弓箭手
        [SerializeField] GameObject Enemy05;//枪兵
        [SerializeField] float Level4Duration;
        [SerializeField] float Level4ReleaseInterval;
        [SerializeField] float Enemy05RelaseInterval;
        [SerializeField] VoidEventChannel Level4Passed;

        private WaitForSeconds waitForReleaseInterval;
        private WaitForSeconds waitForEnemy05RelaseInterval;

        protected override void Awake()
        {
            base.Awake();
            waitForReleaseInterval = new WaitForSeconds(Level4ReleaseInterval);
            waitForEnemy05RelaseInterval = new WaitForSeconds(Enemy05RelaseInterval);
        }

        protected override void ReleaseEnemyEvent()
        {
            StartCoroutine(nameof(ReleaseEnemy));
        }

        protected override void RunAfterEnemysDied()
        {
            Level4Passed.Broadcast();
            SteamAchievement.Instance.Reach_Achievement(AchievementType.AllMedium_Clearance);
            SteamAchievement.Instance.achievementList[1].UnlockAchievementIcon();
        }

        IEnumerator ReleaseEnemy()
        {
            //GameObject GenerationPos = null;
            var nullObj = new GameObject();
            liveEnemyList.Add(nullObj);//防止提前结束

            GameObject obj = null;
            GameObject GenerationPos = null;
            var startTime = Time.time;
            while (Time.time - startTime < Level4Duration)
            {
                int random = 0;
                random = Random.Range(1, 4);
                if (random == 1)
                {
                    obj = Enemy01;
                    random = Random.Range(1, 3);//第二次随机用于释放位置
                    if (random == 1)
                        GenerationPos = EnemyGenerationPosition1;
                    else if (random == 2)
                        GenerationPos = EnemyGenerationPosition3;

                    ReleaseEnemy(obj, GenerationPos.transform.position);
                    if (CheckLiveListTheEnemyCount(Enemy05.name) == 0)
                    {
                        StartCoroutine(nameof(ReleaseEnemy05));
                    }
                }
                else if (random == 2)
                {
                    obj = Enemy02;
                    GenerationPos = EnemyGenerationPosition2;
                    ReleaseEnemy(obj, GenerationPos.transform.position);
                    if (CheckLiveListTheEnemyCount(Enemy05.name) == 0)
                    {
                        StartCoroutine(nameof(ReleaseEnemy05));
                    }
                }
                else if (random == 3)
                {
                    obj = Enemy04;
                    random = Random.Range(1, 3);//第二次随机用于释放位置
                    if (random == 1)
                        GenerationPos = EnemyGenerationPosition1;
                    else if (random == 2)
                        GenerationPos = EnemyGenerationPosition3;

                    ReleaseEnemy(obj, GenerationPos.transform.position);
                    if (CheckLiveListTheEnemyCount(Enemy05.name) == 0)
                    {
                        StartCoroutine(nameof(ReleaseEnemy05));
                    }
                }
                else if (random == 4)
                {
                    obj = Enemy03;
                    random = Random.Range(1, 3);//第二次随机用于释放位置
                    if (random == 1)
                        GenerationPos = EnemyGenerationPosition1;
                    else if (random == 2)
                        GenerationPos = EnemyGenerationPosition3;

                    ReleaseEnemy(obj, GenerationPos.transform.position);
                    if (CheckLiveListTheEnemyCount(Enemy05.name) == 0)
                    {
                        StartCoroutine(nameof(ReleaseEnemy05));
                    }
                }

                yield return waitForReleaseInterval;

            }

            liveEnemyList.Remove(nullObj);//去除空对象
            Destroy(nullObj);
        }

        IEnumerator ReleaseEnemy05()
        {
            yield return waitForEnemy05RelaseInterval;
            ReleaseEnemy(Enemy05, EnemyGenerationPosition3.transform.position);
        }
    }
}