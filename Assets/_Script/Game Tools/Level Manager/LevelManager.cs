using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    /// <summary>
    /// 可复用的关卡管理器
    /// 目前功能：管理关卡的解锁条件(包含解锁功能)
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        public List<BaseLevelModule> Level = new List<BaseLevelModule>();

        private void Awake()
        {
            //加载关卡解锁条件
            Level[0].VisibleCondition = _ => true;
            Level[1].VisibleCondition = _ => Level[0].IsPassed;
            Level[2].VisibleCondition = _ => Level[0].IsPassed && Level[1].IsPassed;
            Level[3].VisibleCondition = _ => Level[0].IsPassed && Level[1].IsPassed;
            Level[4].VisibleCondition = _ => true;
            Level[5].VisibleCondition = _ => true;
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

    }
}