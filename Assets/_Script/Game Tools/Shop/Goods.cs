using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Adv
{
    public abstract class Goods : MonoBehaviour
    {
        public Func<Goods, bool> VisibleCondition = _ => false;

        public abstract bool IsUnlocked { get; set; }//TODO 存档
        protected abstract void GoodsFunction();
        [SerializeField] protected bool CanDestroy;
        [SerializeField] protected int DestroyCount;
        [SerializeField] int MaxLevel;
        [SerializeField] int InitalValue;
        [SerializeField, Range(1f, 10f)] protected float ValueIncreaseRate = 1;
        public Button BugButton;
        [SerializeField] Text Value;
        [SerializeField] FloatEventChannel MoneyChange;
        [SerializeField] protected Shop shop;

        protected int CurrentLevel = 0;//TODO 存档

        protected virtual void Awake()
        {
            //读档

            Value.text = (Mathf.Pow(ValueIncreaseRate, CurrentLevel) * InitalValue).ToString();

            BugButton.onClick.AddListener(() =>
            {
                var NeedMoney = 0f;
                NeedMoney = Mathf.Pow(ValueIncreaseRate, CurrentLevel) * InitalValue;
                if (PlayerAsset.Money >= NeedMoney)
                {
                    PlayerAsset.Money -= NeedMoney;
                    MoneyChange.Broadcast(PlayerAsset.Money);
                    if (CurrentLevel < MaxLevel)
                        CurrentLevel += 1;
                    Value.text = (NeedMoney * ValueIncreaseRate).ToString();
                    GoodsFunction();
                    CheckBugCount();
                    shop.CheckAllLevelIsUnLocked();
                }
            });
        }

        private void OnEnable()
        {
            CheckBugCount();
        }

        /// <summary>
        /// 判断是否达到购买上限，是则返回true
        /// </summary>
        public virtual bool CheckBugCount()
        {
            if (CanDestroy && CurrentLevel >= DestroyCount)
            {
                BugButton.enabled = false;
                gameObject.SetActive(false);
                GameSaver.Instance.SaveAllData();//购买后顺便保存游戏
                return true;
            }
            return false;
        }

        public virtual bool CheckUnLcock()
        {
            return VisibleCondition(this);
        }

        public virtual void SetActive(bool enabled)
        {
            gameObject.SetActive(enabled);
        }
    }
}
