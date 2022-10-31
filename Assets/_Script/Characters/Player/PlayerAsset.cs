using System.Collections;
using System.Collections.Generic;
using BayatGames.SaveGameFree;
using UnityEngine;

namespace Adv
{
    public class PlayerAsset : MonoBehaviour
    {
        public static float Money = 0;

        [SerializeField] FloatEventChannel MoneyChange;

        private void Awake()
        {
            //GameSaver.Instance.SaveCache<float>("PlayerMoney", Money);
        }

        private void Start()
        {
            //读取数据
            if (GameSaver.Instance.Exists("PlayerMoney"))
            {
                Money = GameSaver.Instance.Load<float>("PlayerMoney");
                MoneyChange.Broadcast(Money);
            }

            GameSaver.Instance.SaveDataEventCall(() =>
            {
                SaveGame.Save<float>("PlayerMoney", Money);
            });
        }
    }
}
