using System;
using System.Collections;
using System.Collections.Generic;
using BayatGames.SaveGameFree;
using UnityEngine;
using UnityEngine.UI;

namespace Adv
{
    /// <summary>
    /// 关卡对象的基础模块，继承该类的父类需要挂载在Level按键对象上
    /// </summary>
    public abstract class BaseLevelModule : MonoBehaviour
    {
        public static bool IsVictory = false;//胜利时必须要LevelEnd之前置1
        public static string CurrentRunningLevelKey = EndKey;
        public static string LastLevelKey = EndKey;
        public const string EndKey = "EndKey";

        public abstract string Key { get; }

        public int LevelMaxCombo = 0;
        [HideInInspector] public Button levelButton;
        [SerializeField] GameObjectEventChannel EnemyDied;
        //这两个事件用来 1、协调所有Level需要同时改变的事件 以及 2、方便其他对象的调用
        [SerializeField] VoidEventChannel LevelStart;
        [SerializeField] VoidEventChannel LevelEnd;
        [SerializeField] VoidEventChannel LevelClosing;
        [SerializeField] protected VoidEventChannel EarlyOutLevel;
        [SerializeField] VoidEventChannel ClearingUIClose;
        [SerializeField] VoidEventChannel SaveDataEvent;
        //=======================================================================
        [SerializeField] LevelManager levelManager;
        [SerializeField] protected GameObject EnemyGenerationPosition1;
        [SerializeField] protected GameObject EnemyGenerationPosition2;
        [SerializeField] protected GameObject EnemyGenerationPosition3;

        public bool IsPassed = false;
        public Func<BaseLevelModule, bool> VisibleCondition = _ => false;

        protected List<GameObject> liveEnemyList = new List<GameObject>();

        private const string CloneString = "(Clone)";

        private Image ButtonImage;
        private Text ButtonText;

        protected virtual void Awake()
        {
            levelButton = GetComponent<Button>();
            ButtonImage = GetComponent<Image>();
            ButtonText = GetComponentInChildren<Text>();
            levelButton.onClick.AddListener(OnClickEvent);
            EnemyDied.AddListener((gameObject) =>
            {
                liveEnemyList.Remove(gameObject);
            });
            LevelStart.AddListener(() =>
            {
                IsVictory = false;
                ButtonImage.enabled = false;
                ButtonText.enabled = false;
                levelButton.enabled = false;
            });
            LevelEnd.AddListener(() =>
            {
                StopAllCoroutines();
                if (PlayerProperty.CurrentMaxCombo < PlayerProperty.Combo)
                    PlayerProperty.CurrentMaxCombo = PlayerProperty.Combo;
                if (CurrentRunningLevelKey == Key && LevelMaxCombo < PlayerProperty.CurrentMaxCombo)
                {
                    LevelMaxCombo = PlayerProperty.CurrentMaxCombo;
                }
            });
            EarlyOutLevel.AddListener(() =>
            {
                CurrentRunningLevelKey = EndKey;
                levelManager.CheckAllLevelIsUnLocked();
                if (!gameObject.activeSelf) return;
                ButtonImage.enabled = true;
                ButtonText.enabled = true;
                levelButton.enabled = true;
            });
            ClearingUIClose.AddListener(() =>
            {
                CurrentRunningLevelKey = EndKey;
                levelManager.CheckAllLevelIsUnLocked();
                if (!gameObject.activeSelf) return;
                ButtonImage.enabled = true;
                ButtonText.enabled = true;
                levelButton.enabled = true;
            });
        }

        public virtual void LoadData()
        {
            if (GameSaver.Instance.Exists(Key + "_IsPassed"))
            {
                IsPassed = GameSaver.Instance.Load<bool>(Key + "_IsPassed");
            }
            if (GameSaver.Instance.Exists(Key + "_LevelMaxCombo"))
            {
                LevelMaxCombo = GameSaver.Instance.Load<int>(Key + "_LevelMaxCombo");
            }
        }

        private void Start()
        {
            GameSaver.Instance.SaveDataEventCall(() =>
            {
                SaveGame.Save<bool>(Key + "_IsPassed", IsPassed);
                SaveGame.Save<int>(Key + "_LevelMaxCombo", LevelMaxCombo);
            });
        }
        private void OnDestroy()
        {
            levelButton.onClick.RemoveListener(OnClickEvent);
        }

        public virtual bool CheckUnLcock()
        {
            return VisibleCondition(this);
        }

        public virtual void SetActive(bool enabled)
        {
            gameObject.SetActive(enabled);
        }

        protected virtual GameObject ReleaseEnemy(GameObject obj, Vector3 GenerationPos)
        {
            var obj1 = PoolManager.Instance.Release(obj, GenerationPos);
            liveEnemyList.Add(obj1);
            return obj1;
        }

        protected virtual void InstantiateEnemy(GameObject obj)
        {
            liveEnemyList.Add(Instantiate(obj));
        }

        /// <summary>
        /// 只能查看由PoolManager释放的对象
        /// </summary>
        /// <param name="enemyName">敌人名字</param>
        protected virtual int CheckLiveListTheEnemyCount(string enemyName)
        {
            int count = 0;
            for (var i = 0; i < liveEnemyList.Count; i++)
            {
                var concat = string.Concat(enemyName, CloneString);
                if (liveEnemyList[i].name.Equals(concat))
                    count++;
            }
            return count;
        }

        protected abstract void ReleaseEnemyEvent();
        protected abstract void RunAfterEnemysDied();

        private void OnClickEvent()
        {
            CurrentRunningLevelKey = Key;
            LastLevelKey = Key;
            LevelStart.Broadcast();
            liveEnemyList.Clear();
            ReleaseEnemyEvent();
            StartDetectAllEnemysIsDied();
        }

        private void StartDetectAllEnemysIsDied()
        {
            StartCoroutine(nameof(DetectEnemyIsDied));
        }

        IEnumerator DetectEnemyIsDied()
        {
            while (liveEnemyList.Count != 0)
            {
                yield return null;
            }
            //胜利
            IsVictory = true;//必须要LevelEnd之前
            IsPassed = true;
            LevelEnd.Broadcast();//
            LevelClosing.Broadcast();//
            RunAfterEnemysDied();

        }
    }
}