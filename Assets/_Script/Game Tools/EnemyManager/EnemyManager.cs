using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        [SerializeField] VoidEventChannel Fail;
        [SerializeField] VoidEventChannel Level1Achieve;
        [SerializeField] VoidEventChannel Level1ButtonClick;
        [SerializeField] VoidEventChannel Level2ButtonClick;
        [SerializeField] GameObjectEventChannel EnemyDied;

        private List<GameObject> liveEnemyList = new List<GameObject>();
        private float LevelStartTime;
        private GameObject LastEnemy;
        private CurrentLevel currentLevel;

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
            if (currentLevel == CurrentLevel.Level1)
                Level1Achieve.Broadcast();
            else if (currentLevel == CurrentLevel.Level2)
                Debug.Log("通关");
        }

        private void Awake()
        {
            Level1ButtonClick.AddListener(StartLevel1);
            Level2ButtonClick.AddListener(StartLevel2);
            EnemyDied.AddListener((gameObject) =>
            {
                liveEnemyList.Remove(gameObject);
            });
            Fail.AddListener(() => { StopAllCoroutines(); });
        }

        private void OnDestroy()
        {
            Level1ButtonClick.RemoveListenner(StartLevel1);
            Level1ButtonClick.RemoveListenner(StartLevel2);
        }

        private void StartLevel1()
        {
            StartCoroutine(LevelProcessing(Level1));
            currentLevel = CurrentLevel.Level1;
        }

        private void StartLevel2()
        {
            StartCoroutine(LevelProcessing(Level2));
            currentLevel = CurrentLevel.Level2;
        }
    }
}