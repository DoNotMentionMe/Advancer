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

        private void Awake()
        {
            Goods[0].VisibleCondition = _ => Goods[0].IsUnlocked;//血量
            Goods[1].VisibleCondition = _ => Goods[1].IsUnlocked;//护盾解锁
            Goods[2].VisibleCondition = _ => !Goods[1].gameObject.activeSelf;//护盾升级


        }

        private void OnEnable()
        {
            CheckAllLevelIsUnLocked();
        }
        public void CheckAllLevelIsUnLocked()
        {
            //检查解锁情况，判断关卡开关是否打开
            for (var i = 0; i < Goods.Count; i++)
            {
                if (Goods[i].CheckUnLcock())
                    Goods[i].SetActive(true);
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
