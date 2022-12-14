using System.Collections;
using System.Collections.Generic;
using BayatGames.SaveGameFree;
using UnityEngine;

namespace Adv
{
    public class GameSaver : PersistentSingleton<GameSaver>
    {
        [SerializeField] VoidEventChannel SaveDataEvent;//在ClearingUI关闭和商品购买时被执行

        public void SaveDataEventCall(System.Action action)
        {
            SaveGame.SavePath = SaveGamePath.DataPath;
            SaveDataEvent.AddListener(action);
        }

        public void SaveAllData()
        {
            SaveGame.SavePath = SaveGamePath.DataPath;
            SaveDataEvent.Broadcast();
        }

        public T Load<T>(string identifier)
        {
            SaveGame.SavePath = SaveGamePath.DataPath;
            return SaveGame.Load<T>(identifier, SaveGamePath.DataPath);
        }

        public bool Exists(string identifier)
        {
            SaveGame.SavePath = SaveGamePath.DataPath;
            return SaveGame.Exists(identifier, SaveGamePath.DataPath);
        }

        public void Clear()
        {
            SaveGame.Clear(SaveGamePath.DataPath);
        }

        private void OnDestroy()
        {
            SaveDataEvent.RemoveAllListenners();
        }
    }
}
