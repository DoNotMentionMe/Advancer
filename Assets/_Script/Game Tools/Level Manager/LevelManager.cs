using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    /// <summary>
    /// 可服用的关卡管理器
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] List<BaseLevelModule> Level = new List<BaseLevelModule>();

        private void Awake()
        {
            //加载关卡解锁条件
            Level[0].VisibleCondition = self => !self.IsPassed;
            Level[1].VisibleCondition = _ => Level[0].IsPassed;
            Level[2].VisibleCondition = _ => Level[0].IsPassed && Level[1].IsPassed;
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