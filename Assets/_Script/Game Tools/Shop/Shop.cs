using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Adv
{
    //管理商品解锁(可视)情况；物品购买后判断是否物品已经消失，是否需要重新选择按键
    public class Shop : MonoBehaviour
    {
        public List<Goods> Goods = new List<Goods>();

        [SerializeField] Image Background_Font;
        [SerializeField] Text Money;
        [SerializeField] Text BtnTips;
        [SerializeField] Text GoodsComment;
        [SerializeField] VoidEventChannel Level4Passed;
        [SerializeField] Button ShopBtn;
        [SerializeField] GameObject HealthPlus1;
        [SerializeField] PlayerInput input;
        [SerializeField] BoolEventChannel CloseAllLabelOption;
        [SerializeField] VoidEventChannel SaveLastBtnBeforeCloseAllUI;
        [SerializeField] LabelOptionsUI ShopLabel;
        [SerializeField] SettingUI settingUI;

        private Button HealthPlus1Btn;
        private Button LastSelectBtn;

        private void Awake()
        {
            Goods[0].VisibleCondition = _ => Goods[0].IsUnlocked;//血量
            Goods[1].VisibleCondition = _ => Goods[1].IsUnlocked;//护盾解锁
            Goods[2].VisibleCondition = _ => Goods[2].IsUnlocked;//护盾升级，解锁后出现
            Goods[3].VisibleCondition = _ => Goods[3].IsUnlocked;//攻击可打断

            Level4Passed.AddListener(() =>
            {
                Goods[3].IsUnlocked = true;
                CheckAllLevelIsUnLocked();
            });

            SaveLastBtnBeforeCloseAllUI.AddListener(() =>
            {
                if (ShopLabel.enabled && ShopLabel.IsOpen)
                    LastSelectBtn = EventSystem.current.currentSelectedGameObject?.GetComponent<Button>();
            });

            CloseAllLabelOption.AddListener(IsOpen =>
            {
                Background_Font.enabled = IsOpen;
                Money.enabled = IsOpen;
                BtnTips.enabled = IsOpen;
                GoodsComment.enabled = IsOpen;
            });

            ShopBtn.onClick.AddListener(() =>
            {
                Background_Font.enabled = ShopLabel.IsOpen;
                Money.enabled = ShopLabel.IsOpen;
                BtnTips.enabled = ShopLabel.IsOpen;
                GoodsComment.enabled = ShopLabel.IsOpen;

                if (ShopLabel.IsOpen)
                {
                    if (LastSelectBtn != null)
                    {
                        LastSelectBtn.Select();
                    }
                    else if (HealthPlus1.activeSelf)
                    {
                        if (HealthPlus1Btn == null)
                            HealthPlus1Btn = HealthPlus1.GetComponentInChildren<Button>();
                        HealthPlus1Btn.Select();
                    }
                }
                else
                {
                    LastSelectBtn = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
                }
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

            input.onShop += OpenShop;
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

        private void OpenShop()
        {
            if (!settingUI.IsOpen)
                ShopBtn.onClick.Invoke();
        }

    }
}
