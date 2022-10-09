using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Adv
{
    public enum GenerationObj
    {
        Enemy01, Enemy02, Enemy03, BOSS01
    }
    public enum GenerationPos
    {
        Left, Up, Right
    }
    public enum CurrentLevel
    {
        Level1, Level2
    }
    /// <summary>
    /// 管理敌人生成、管理关卡（未实施）
    /// </summary>
    public class EnemyManager : MonoBehaviour
    {

        [Header("===== EnemyPrefabs =====")]
        [SerializeField] GameObject Enemy01;
        [SerializeField] GameObject Enemy02;
        [SerializeField] GameObject Enemy03;
        [SerializeField] GameObject BOSS01;
        [Space]

        [SerializeField] List<EnemyGenerationInformation> Level1 = new List<EnemyGenerationInformation>();
        [SerializeField] List<EnemyGenerationInformation> Level2 = new List<EnemyGenerationInformation>();
        [SerializeField] GameObject EnemyGenerationPosition1;
        [SerializeField] GameObject EnemyGenerationPosition2;
        [SerializeField] GameObject EnemyGenerationPosition3;
        [SerializeField] Text Level1ChallengeCountText;
        [SerializeField] Text Level2ChallengeCountText;
        [SerializeField] Text Level1DeathsText;
        [SerializeField] Text Level2DeathsText;
        [SerializeField] Text Level1WinText;
        [SerializeField] Text Level2WinText;
        [SerializeField] GameObject 通关界面;
        [SerializeField] Text 用时;
        //[SerializeField] Text LevelTime;
        [SerializeField] VoidEventChannel Fail;
        [SerializeField] VoidEventChannel Level1Achieve;
        [SerializeField] VoidEventChannel Level2Achieve;
        [SerializeField] VoidEventChannel Level1ButtonClick;
        [SerializeField] VoidEventChannel Level2ButtonClick;
        [SerializeField] GameObjectEventChannel EnemyDied;
        [SerializeField] float Level1Duration;
        [SerializeField] float Level1ReleaseInterval;


        private bool FirstLevel2Success = true;
        private float Level1ChallengeCount = 0;
        private float Level2ChallengeCount = 0;
        private float Level1Deaths = 0;
        private float Level2Deaths = 0;
        private float Level1Win = 0;
        private float Level2Win = 0;
        private float Enemy03LiveCount = 0;
        private float GameStartTime;

        private List<GameObject> liveEnemyList = new List<GameObject>();
        private float LevelStartTime;
        private GameObject LastEnemy;
        private CurrentLevel currentLevel;
        private WaitForSeconds waitForReleaseInterval;

        IEnumerator LevelProcessing(List<EnemyGenerationInformation> level)
        {
            LevelStartTime = Time.time;
            int currentOrder = 0;
            while (level.Count > currentOrder)
            {
                var generationTime = level[currentOrder].GenrationTimePoint;
                while (generationTime > (Time.time - LevelStartTime))
                {
                    yield return null;
                }
                RelaseEnemy(level[currentOrder]);
                currentOrder++;
                if (currentOrder == level.Count)
                {
                    StartCoroutine(nameof(DetectEnemyIsDied));
                }
            }
        }

        IEnumerator RandomReleaseLevel1Enemy()
        {
            GameObject obj = null;
            GameObject GenerationPos = null;
            var startTime = Time.time;
            while (Time.time - startTime < Level1Duration)
            {
                var random = Random.Range(1, 4);
                if (random == 1)
                    obj = Enemy01;
                else if (random == 2)
                {
                    obj = Enemy02;
                    GenerationPos = EnemyGenerationPosition2;
                }
                else if (random == 3)
                {
                    if (Enemy03LiveCount < 2)
                    {
                        Enemy03LiveCount++;
                        obj = Enemy03;
                    }
                    else
                    {
                        obj = Enemy01;
                    }

                }
                if (random == 1 || random == 3)
                    random = Random.Range(1, 3);
                if (random == 1)
                    GenerationPos = EnemyGenerationPosition1;
                else if (random == 2)
                    GenerationPos = EnemyGenerationPosition3;
                liveEnemyList.Add(PoolManager.Instance.Release(obj, GenerationPos.transform.position));
                yield return waitForReleaseInterval;
            }
            StartCoroutine(nameof(DetectEnemyIsDied));
        }

        private void RelaseEnemy(EnemyGenerationInformation information)
        {
            GameObject obj = null;
            GameObject GenerationPos = null;
            if (information.GenerationObj == GenerationObj.Enemy01)
                obj = Enemy01;
            else if (information.GenerationObj == GenerationObj.Enemy02)
                obj = Enemy02;
            else if (information.GenerationObj == GenerationObj.Enemy03)
                obj = Enemy03;
            else if (information.GenerationObj == GenerationObj.BOSS01)
                obj = BOSS01;

            if (information.GenerationPosition == Adv.GenerationPos.Left)
                GenerationPos = EnemyGenerationPosition1;
            else if (information.GenerationPosition == Adv.GenerationPos.Up)
                GenerationPos = EnemyGenerationPosition2;
            else if (information.GenerationPosition == Adv.GenerationPos.Right)
                GenerationPos = EnemyGenerationPosition3;

            //LastEnemy = PoolManager.Instance.Release(obj, GenerationPos.transform.position);
            liveEnemyList.Add(PoolManager.Instance.Release(obj, GenerationPos.transform.position));
            //Debug.Log("释放" + obj.name + "于" + GenerationPos.transform.position + "位置");
        }

        IEnumerator DetectEnemyIsDied()
        {
            while (liveEnemyList.Count != 0)
            {
                yield return null;
            }
            ShowTextEnable(true);
            if (currentLevel == CurrentLevel.Level1)
            {
                Level1Achieve.Broadcast();
                Level1Win++;
                Level1WinText.text = "Level1通关次数: " + Level1Win;
            }
            else if (currentLevel == CurrentLevel.Level2)
            {
                Level2Achieve.Broadcast();
                Level2Win++;
                Level2WinText.text = "Level2通关次数: " + Level2Win;
                if (FirstLevel2Success)
                {
                    FirstLevel2Success = false;
                    //显示通关界面
                    通关界面.SetActive(true);
                    var time = (int)(Time.time - GameStartTime);
                    var min = time / 60;
                    var sec = time % 60;
                    用时.text = "总用时: " + min + "分 " + sec + "秒";
                }
            }
            //LevelTime.text = ((int)60).ToString();
        }

        // IEnumerator CountDown()
        // {
        //     var timer = 60f;
        //     while (timer > 0)
        //     {
        //         timer -= Time.deltaTime;
        //         LevelTime.text = ((int)timer).ToString();
        //         yield return null;
        //     }
        //     LevelTime.text = ((int)60).ToString();
        // }


        private void Awake()
        {
            GameStartTime = Time.time;

            waitForReleaseInterval = new WaitForSeconds(Level1ReleaseInterval);

            Level1ButtonClick.AddListener(StartLevel1);
            Level2ButtonClick.AddListener(StartLevel2);
            EnemyDied.AddListener((gameObject) =>
            {
                if (gameObject.name == "Enemy03(Clone)")
                    Enemy03LiveCount--;
                liveEnemyList.Remove(gameObject);
            });
            Fail.AddListener(() =>
            {
                StopAllCoroutines();
                ShowTextEnable(true);
                if (currentLevel == CurrentLevel.Level1)
                {
                    Level1Deaths++;
                    Level1DeathsText.text = "Level1失败次数: " + Level1Deaths.ToString();
                }
                else if (currentLevel == CurrentLevel.Level2)
                {
                    Level2Deaths++;
                    Level2DeathsText.text = "Level2失败次数: " + Level2Deaths.ToString();
                }
                //LevelTime.text = ((int)60).ToString();
            });
            Level1ChallengeCountText.text = "Level1挑战次数: " + Level1ChallengeCount.ToString();
            Level2ChallengeCountText.text = "Level2挑战次数: " + Level2ChallengeCount.ToString();
            Level1DeathsText.text = "Level1失败次数: " + Level1Deaths.ToString();
            Level2DeathsText.text = "Level2失败次数: " + Level2Deaths.ToString();
            Level1WinText.text = "Level1通关次数: " + Level1Win;
            Level2WinText.text = "Level2通关次数: " + Level2Win;
            //LevelTime.text = ((int)60).ToString();
        }

        private void OnDestroy()
        {
            Level1ButtonClick.RemoveListenner(StartLevel1);
            Level1ButtonClick.RemoveListenner(StartLevel2);
        }

        private void StartLevel1()
        {
            liveEnemyList.Clear();
            //StartCoroutine(LevelProcessing(Level1));
            StartCoroutine(nameof(RandomReleaseLevel1Enemy));
            //StartCoroutine(nameof(CountDown));
            Level1ChallengeCount++;
            Level1ChallengeCountText.text = "Level1挑战次数: " + Level1ChallengeCount.ToString();
            currentLevel = CurrentLevel.Level1;
            ShowTextEnable(false);
        }

        private void StartLevel2()
        {
            liveEnemyList.Clear();
            liveEnemyList.Add(Instantiate(BOSS01));
            StartCoroutine(nameof(DetectEnemyIsDied));
            //StartCoroutine(nameof(CountDown));
            Level2ChallengeCount++;
            Level2ChallengeCountText.text = "Level2挑战次数: " + Level2ChallengeCount.ToString();
            currentLevel = CurrentLevel.Level2;
            ShowTextEnable(false);
        }

        private void ShowTextEnable(bool enable)
        {
            Level1ChallengeCountText.enabled = enable;
            Level2ChallengeCountText.enabled = enable;
            Level1DeathsText.enabled = enable;
            Level2DeathsText.enabled = enable;
            Level1WinText.enabled = enable;
            Level2WinText.enabled = enable;
        }
    }
}