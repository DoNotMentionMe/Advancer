using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Adv
{
    //管理商品解锁(可视)情况；物品购买后判断是否物品已经消失，是否需要重新选择按键
    public class Shop : MonoBehaviour
    {
        public List<Goods> Goods = new List<Goods>();

        [SerializeField] VoidEventChannel Level4Passed;

        private void Awake()
        {
            Goods[0].VisibleCondition = _ => Goods[0].IsUnlocked;//血量
            Goods[1].VisibleCondition = _ => Goods[1].IsUnlocked;//护盾解锁
            Goods[2].VisibleCondition = _ => Goods[2].IsUnlocked;//护盾升级，解锁后出现
            Goods[3].VisibleCondition = _ => Goods[3].IsUnlocked;//攻击可打断

            Level4Passed.AddListener(() =>
            {
                Goods[3].IsUnlocked = true;
                Debug.Log(Goods[3].IsUnlocked);
                CheckAllLevelIsUnLocked();
            });
        }

        private void Start()
        {
            //读取商品存档
            for (var i = 0; i < Goods.Count; i++)
            {
                Goods[i].LoadData();
            }
            CheckAllLevelIsUnLocked();
        }

        public void CheckAllLevelIsUnLocked()
        {
            //检查解锁情况，判断关卡开关是否打开
            for (var i = 0; i < Goods.Count; i++)
            {
                if (Goods[i].CheckUnLcock())
                {
                    Goods[i].SetActive(true);
                    //Goods[i].CheckBugCount();
                }
                else
                    Goods[i].SetActive(false);
                if (EventSystem.current.currentSelectedGameObject == Goods[i].BugButton.gameObject && !Goods[i].gameObject.activeSelf)
                {
                    Goods[0].BugButton.Select();
                }
            }
            // if (EventSystem.current.currentSelectedGameObject == null)
            // {
            //     Goods[0].BugButton.Select();
            // }
        }

    }
}
