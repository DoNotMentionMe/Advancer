using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class Level1 : BaseLevelModule
    {
        public override string Key { get; } = nameof(Level1);


        [SerializeField] GameObject Enemy01;//小猪
        [SerializeField] GameObject Enemy02;//雷鸟
        [SerializeField] GameObject Enemy03;//刀客
        [SerializeField, Range(1, 40)] int tageCount = 1;
        [SerializeField] float Level1Duration;
        [SerializeField] float CurrentInterval;
        [SerializeField] float Level1ReleaseIntervalStart;
        [SerializeField] float Level1ReleaseIntervalEnd;

        private float LevelStartTime;
        private WaitForSeconds waitForReleaseInterval;

        protected override void Awake()
        {
            base.Awake();
            CurrentInterval = Level1ReleaseIntervalStart;
            waitForReleaseInterval = new WaitForSeconds(Level1ReleaseIntervalStart);
        }

        protected override void ReleaseEnemyEvent()
        {
            StartCoroutine(nameof(ChangeReleaseInterval));
            StartCoroutine(nameof(RandomReleaseLevel1Enemy));
        }

        protected override void RunAfterEnemysDied()
        {

        }

        IEnumerator RandomReleaseLevel1Enemy()
        {
            GameObject obj = null;
            GameObject GenerationPos = null;
            float currentDuration = 0;
            while (Time.time - LevelStartTime < Level1Duration)
            {
                int random = 0;
                random = Random.Range(1, 5);//通过增加右边的值提高Enemy03出现概率

                if (random >= 3)
                {
                    currentDuration = Time.time - LevelStartTime;
                    //根据当前Enemy03数量选择召唤敌人类型
                    if (currentDuration < Level1Duration / 4
                        && CheckLiveListTheEnemyCount(Enemy03.name) < 1)
                        obj = Enemy03;
                    else if (currentDuration > Level1Duration / 4
                        && currentDuration < Level1Duration * 1 / 2
                        && CheckLiveListTheEnemyCount(Enemy03.name) < 2)
                        obj = Enemy03;
                    else if (currentDuration > Level1Duration * 1 / 2
                        && currentDuration < Level1Duration * 3 / 4
                        && CheckLiveListTheEnemyCount(Enemy03.name) < 3)
                        obj = Enemy03;
                    else if (currentDuration > Level1Duration * 3 / 4
                        && CheckLiveListTheEnemyCount(Enemy03.name) < 4)
                        obj = Enemy03;
                    else
                        random = Random.Range(1, 3);
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

        IEnumerator ChangeReleaseInterval()
        {
            LevelStartTime = Time.time;
            CurrentInterval = Level1ReleaseIntervalStart;

            var difference = Level1ReleaseIntervalStart - Level1ReleaseIntervalEnd;
            difference /= tageCount - 1;
            for (var i = 1; i < tageCount; i++)
            {
                while (Time.time - LevelStartTime < i * Level1Duration / tageCount)
                {
                    yield return null;
                }
                CurrentInterval -= difference;
                waitForReleaseInterval = new WaitForSeconds(CurrentInterval);
            }
        }
    }

}