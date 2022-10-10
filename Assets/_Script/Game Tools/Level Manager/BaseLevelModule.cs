using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Adv
{
    /// <summary>
    /// 关卡对象的基础模块，继承该类的父类需要挂载在Level按键对象上
    /// </summary>
    public abstract class BaseLevelModule : MonoBehaviour
    {
        public static string currentRunningLevelKey;
        public const string EndKey = "EndKey";

        public abstract string Key { get; }

        [SerializeField] GameObjectEventChannel EnemyDied;
        [SerializeField] VoidEventChannel LevelStart;
        [SerializeField] VoidEventChannel LevelEnd;
        [SerializeField] LevelManager levelManager;
        [SerializeField] protected GameObject EnemyGenerationPosition1;
        [SerializeField] protected GameObject EnemyGenerationPosition2;
        [SerializeField] protected GameObject EnemyGenerationPosition3;

        public bool IsPassed = false;
        public Func<BaseLevelModule, bool> VisibleCondition = _ => false;

        protected List<GameObject> liveEnemyList = new List<GameObject>();

        private const string CloneString = "(Clone)";

        private Button levelButton;
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
                ButtonImage.enabled = false;
                ButtonText.enabled = false;
                levelButton.enabled = false;
            });
            LevelEnd.AddListener(() =>
            {
                currentRunningLevelKey = EndKey;
                StopAllCoroutines();
                ButtonImage.enabled = true;
                ButtonText.enabled = true;
                levelButton.enabled = true;
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
            currentRunningLevelKey = Key;
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
            LevelEnd.Broadcast();
            IsPassed = true;
            RunAfterEnemysDied();
            levelManager.CheckAllLevelIsUnLocked();
        }
    }
}