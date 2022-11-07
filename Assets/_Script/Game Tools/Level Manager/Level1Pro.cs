using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class Level1Pro : BaseLevelModule
    {
        public override string Key { get; } = nameof(Level1Pro);

        [SerializeField] GameObject Enemy01;//小猪
        [SerializeField] GameObject Enemy02;//雷鸟
        [SerializeField] GameObject Enemy03;//刀客
        [SerializeField] float Level1Duration;
        [SerializeField] float Level1ReleaseInterval;
        [SerializeField] VoidEventChannel Level4Passed;

        private WaitForSeconds waitForReleaseInterval;

        protected override void Awake()
        {
            base.Awake();
            waitForReleaseInterval = new WaitForSeconds(Level1ReleaseInterval);
        }

        protected override void ReleaseEnemyEvent()
        {
            StartCoroutine(nameof(RandomReleaseLevel1Enemy));
        }

        protected override void RunAfterEnemysDied()
        {
            Level4Passed.Broadcast();
        }

        IEnumerator RandomReleaseLevel1Enemy()
        {
            GameObject obj = null;
            GameObject GenerationPos = null;
            var startTime = Time.time;
            //float currentDuration = 0;
            while (Time.time - startTime < Level1Duration)
            {
                int random = 0;
                random = Random.Range(1, 5);//通过增加右边的值提高Enemy03出现概率

                if (random >= 3)
                {
                    if (CheckLiveListTheEnemyCount(Enemy03.name) < 4)
                        obj = Enemy03;
                    else
                        random = Random.Range(1, 3);

                    // currentDuration = Time.time - startTime;
                    // //根据当前Enemy03数量选择召唤敌人类型
                    // if (currentDuration < Level1Duration / 4
                    //     && CheckLiveListTheEnemyCount(Enemy03.name) < 1)
                    //     obj = Enemy03;
                    // else if (currentDuration > Level1Duration / 4
                    //     && currentDuration < Level1Duration * 1 / 2
                    //     && CheckLiveListTheEnemyCount(Enemy03.name) < 2)
                    //     obj = Enemy03;
                    // else if (currentDuration > Level1Duration * 1 / 2
                    //     && currentDuration < Level1Duration * 3 / 4
                    //     && CheckLiveListTheEnemyCount(Enemy03.name) < 3)
                    //     obj = Enemy03;
                    // else if (currentDuration > Level1Duration * 3 / 4
                    //     && CheckLiveListTheEnemyCount(Enemy03.name) < 4)
                    //     obj = Enemy03;
                    // else
                    //     random = Random.Range(1, 3);
                }
                if (random == 1)
                    obj = Enemy01;
                else if (random == 2)
                {
                    obj = Enemy02;
                    GenerationPos = EnemyGenerationPosition2;
                }

                if (random == 1 || random >= 3)
                    random = Random.Range(1, 3);//第二次随机用于释放位置
                if (random == 1)
                    GenerationPos = EnemyGenerationPosition1;
                else if (random == 2)
                    GenerationPos = EnemyGenerationPosition3;
                //liveEnemyList.Add(PoolManager.Instance.Release(obj, GenerationPos.transform.position));
                ReleaseEnemy(obj, GenerationPos.transform.position);
                yield return waitForReleaseInterval;
            }
        }
    }

}
