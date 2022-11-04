using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Adv
{
    public class LiveEndUI : MonoBehaviour
    {
        [SerializeField] CurrentLiveTimeShow currentLiveTimeShow;
        [SerializeField] FloatEventChannel MoneyChange;
        [SerializeField] Text LiveTimeShow;
        [SerializeField] Text MoneyGetShow;
        [SerializeField] Text MaxLiveTimeShow;
        [SerializeField] int MoneyRate;
        private float CurrentLevelLiveTime;
        //奖励==LiveTime * LiveTime * MoneyRate
        public float MaxLiveTime = 0;

        private const string liveTimeShowStart = "生存时间: ";
        private const string liveTimeShowEnd = " 秒";

        private void Start()
        {
            //不能在这里读取MaxLiveTime，因为会在这之前就关闭该对象
            //实际MaxLiveTime最佳声明位置应该在LevelInfinite脚本中

            GameSaver.Instance.SaveDataEventCall(() =>
            {
                BayatGames.SaveGameFree.SaveGame.Save<float>("MaxLiveTime", MaxLiveTime);
            });
        }

        private void OnEnable()
        {
            //获取当局生存时间CurrentLevelLiveTime
            CurrentLevelLiveTime = currentLiveTimeShow.liveTime;
            //显示时间
            LiveTimeShow.text = string.Concat(liveTimeShowStart, CurrentLevelLiveTime, liveTimeShowEnd);
            //比较是否超过最大生存时间
            if (CurrentLevelLiveTime > MaxLiveTime)
            {
                MaxLiveTime = CurrentLevelLiveTime;
                //start在Onenable之后执行，新纪录会被本地的旧记录覆盖掉
                //BayatGames.SaveGameFree.SaveGame.Save<float>("MaxLiveTime", CurrentLevelLiveTime);
                //显示
                MaxLiveTimeShow.text = $"最长存活: {MaxLiveTime} 秒";
            }

            if (BaseLevelModule.LastLevelKey == BaseLevelModule.EndKey)
            {
                MoneyChange.Broadcast(PlayerAsset.Money);
            }
            else
            {
                var plusMoney = ((int)CurrentLevelLiveTime);
                plusMoney *= plusMoney * MoneyRate;
                MoneyGetShow.text = "";
                MoneyGetShow.text += "生存奖励: " + plusMoney;
                PlayerAsset.Money += plusMoney;
                MoneyChange.Broadcast(PlayerAsset.Money);
            }

            GameSaver.Instance.SaveAllData();
        }
    }
}
