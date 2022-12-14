using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    /// <summary>
    /// 可复用的关卡管理器
    /// 目前功能：管理关卡的解锁条件(包含解锁功能)
    ///          选择指定Key的按钮
    /// </summary>
    public class LevelManager : PersistentSingleton<LevelManager>
    {
        public List<BaseLevelModule> Level = new List<BaseLevelModule>();

        protected override void Awake()
        {
            base.Awake();
            //加载关卡解锁条件
            Level[0].VisibleCondition = _ => !Level[0].IsPassed;//Level0
            Level[11].VisibleCondition = _ => Level[0].IsPassed;//Level1Easy
            Level[1].VisibleCondition = _ => Level[14].IsPassed;//Level1
            Level[2].VisibleCondition = _ => Level[8].IsPassed;//Level1Pro
            Level[12].VisibleCondition = _ => Level[11].IsPassed;//Level2Easy
            Level[3].VisibleCondition = _ => Level[1].IsPassed;//Level2
            Level[4].VisibleCondition = _ => Level[2].IsPassed;//Level2 Pro
            Level[13].VisibleCondition = _ => Level[12].IsPassed;//Level3Easy
            Level[5].VisibleCondition = _ => Level[3].IsPassed;//Level3
            Level[6].VisibleCondition = _ => Level[4].IsPassed;//Level3 Pro
            Level[7].VisibleCondition = _ => Level[4].IsPassed;//Level KIA 1
            Level[14].VisibleCondition = _ => Level[13].IsPassed;//Level4Easy
            Level[8].VisibleCondition = _ => Level[5].IsPassed;//Level4
            Level[9].VisibleCondition = _ => Level[6].IsPassed;//Level4 Pro
            Level[10].VisibleCondition = _ => Level[8].IsPassed;//Level 无限
            //    || Level[6].IsPassed
            //    || Level[4].IsPassed
            //    || Level[2].IsPassed;

        }

        private void Start()
        {
            //读取关卡存档信息
            for (var i = 0; i < Level.Count; i++)
            {
                Level[i].LoadData();
            }
            CheckAllLevelIsUnLocked();
        }

        public void CheckAllLevelIsUnLocked()
        {
            //检查解锁情况，判断关卡开关是否打开
            for (var i = 0; i < Level.Count; i++)
            {
                if (Level[i].CheckUnLcock())
                    Level[i].SetActive(true);
                else
                    Level[i].SetActive(false);
            }
        }

        public void SelectButtonWithKey(string Key)
        {
            for (var i = 0; i < Level.Count; i++)
            {
                if (Level[i].Key == Key)
                {
                    Level[i].levelButton.Select();
                    break;
                }
            }
        }
    }
}