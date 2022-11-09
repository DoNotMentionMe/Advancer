using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Adv
{
    public class LevelInfinite : BaseLevelModule
    {
        public override string Key => nameof(LevelInfinite);

        [SerializeField] GameObject Enemy01;//小猪
        [SerializeField] GameObject Enemy02;//雷鸟
        [SerializeField] GameObject Enemy03;//刀客
        [SerializeField] GameObject Enemy04;//弓箭手
        [SerializeField] GameObject Enemy05;//枪兵
        [SerializeField] float CurrentInterval;
        [SerializeField] float Enemy05CurrentRelaseInterval;
        [SerializeField] float DurationPerTage;
        [SerializeField] float DifferencePerTage;
        [SerializeField] float LevelInfiniteIntervalStart;
        [SerializeField] float Enemy05ReleaseIntervalStart;
        [SerializeField] LiveEndUI liveEndUI;
        [SerializeField] Text MaxLiveTimeShow;

        private GameObject nullObj = null;
        private float LevelStartTime;
        private float CurrentTageDuration;
        private WaitForSeconds waitforReleaseInterval;
        private WaitForSeconds waitForEnemy05RelaseInterval;

        protected override void Awake()
        {
            base.Awake();
            CurrentInterval = LevelInfiniteIntervalStart;
            Enemy05CurrentRelaseInterval = Enemy05ReleaseIntervalStart;
            waitforReleaseInterval = new WaitForSeconds(CurrentInterval);
            waitForEnemy05RelaseInterval = new WaitForSeconds(Enemy05CurrentRelaseInterval);
        }

        private void Start()
        {
            if (ChineseEnglishShift.language == Language.Chinese)
                MaxLiveTimeShow.text = $"最长存活: {liveEndUI.MaxLiveTime} 秒";
            else if (ChineseEnglishShift.language == Language.English)
                MaxLiveTimeShow.text = $"Longest survived: \n{liveEndUI.MaxLiveTime} seconds";
        }

        public override void LoadData()
        {
            base.LoadData();
            if (GameSaver.Instance.Exists("MaxLiveTime"))
            {
                liveEndUI.MaxLiveTime = GameSaver.Instance.Load<float>("MaxLiveTime");

                //失效，可能是因为加载时，Text组件时关闭状态
                // if (ChineseEnglishShift.language == Language.Chinese)
                //     MaxLiveTimeShow.text = $"最长存活: {liveEndUI.MaxLiveTime} 秒";
                // else if (ChineseEnglishShift.language == Language.English)
                //     MaxLiveTimeShow.text = $"Longest survived: \n{liveEndUI.MaxLiveTime} seconds";
            }
        }

        protected override void ReleaseEnemyEvent()
        {
            StartCoroutine(nameof(ChangeReleaseInterval));
            StartCoroutine(nameof(RandomReleaseLevel1Enemy));
        }

        protected override void RunAfterEnemysDied()
        {
            Destroy(nullObj);
        }

        IEnumerator RandomReleaseLevel1Enemy()
        {
            nullObj = new GameObject();
            liveEnemyList.Add(nullObj);//防止提前结束

            GameObject obj = null;
            GameObject GenerationPos = null;
            while (true)
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

                yield return waitforReleaseInterval;
            }

            // liveEnemyList.Remove(nullObj);//去除空对象
        }

        IEnumerator ChangeReleaseInterval()
        {
            LevelStartTime = Time.time;
            CurrentTageDuration = Time.time;
            CurrentInterval = LevelInfiniteIntervalStart;
            Enemy05CurrentRelaseInterval = Enemy05ReleaseIntervalStart;
            waitforReleaseInterval = new WaitForSeconds(CurrentInterval);
            waitForEnemy05RelaseInterval = new WaitForSeconds(Enemy05CurrentRelaseInterval);

            while (true)
            {
                while (Time.time - CurrentTageDuration < DurationPerTage)
                {
                    yield return null;
                }
                CurrentTageDuration = Time.time;
                CurrentInterval *= (1 - DifferencePerTage);
                Enemy05CurrentRelaseInterval *= (1 - DifferencePerTage);
                waitforReleaseInterval = new WaitForSeconds(CurrentInterval);
                waitForEnemy05RelaseInterval = new WaitForSeconds(Enemy05CurrentRelaseInterval);
            }
        }

        IEnumerator ReleaseEnemy05()
        {
            yield return waitForEnemy05RelaseInterval;
            ReleaseEnemy(Enemy05, EnemyGenerationPosition3.transform.position);
        }
    }
}
