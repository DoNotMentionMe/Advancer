using System.Collections;
using System.Collections.Generic;
using BayatGames.SaveGameFree;
using UnityEngine;

namespace Adv
{
    public class PlayerAsset : MonoBehaviour
    {
        public static float Money = 0;

        private void Awake()
        {
            //GameSaver.Instance.SaveCache<float>("PlayerMoney", Money);
        }

        private void Start()
        {
            GameSaver.Instance.SaveDataEventCall(() =>
            {
                SaveGame.Save<float>("PlayerMoney", Money);
            });
        }
    }
}
