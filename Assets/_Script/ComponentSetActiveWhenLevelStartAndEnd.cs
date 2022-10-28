using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Adv
{
    public class ComponentSetActiveWhenLevelStartAndEnd : MonoBehaviour
    {
        [SerializeField] bool LevelStartSet = false;
        [SerializeField] bool LevelEndSet = false;
        [SerializeField] bool LevelClosingSet = true;
        [SerializeField] bool ClearingUICloseSet = true;
        [SerializeField] List<Behaviour> ComponentList = new List<Behaviour>();
        [SerializeField] VoidEventChannel LevelStart;
        [SerializeField] VoidEventChannel LevelEnd;
        [SerializeField] VoidEventChannel LevelClosing;
        [SerializeField] VoidEventChannel ClearingUIClose;

        private void Awake()
        {
            LevelStart.AddListener(LevelStartSeting);
            LevelEnd.AddListener(LevelEndSeting);
            LevelClosing.AddListener(LevelClosingSeting);
            ClearingUIClose.AddListener(ClearingUICloseSeting);
        }

        private void OnDestroy()
        {
            LevelStart.RemoveListenner(LevelStartSeting);
            LevelEnd.RemoveListenner(LevelEndSeting);
            LevelClosing.RemoveListenner(LevelClosingSeting);
            ClearingUIClose.RemoveListenner(ClearingUICloseSeting);
        }

        private void LevelStartSeting()
        {
            for (var i = 0; i < ComponentList.Count; i++)
            {
                ComponentList[i].enabled = LevelStartSet;
            }
        }

        private void LevelEndSeting()
        {
            for (var i = 0; i < ComponentList.Count; i++)
            {
                ComponentList[i].enabled = LevelEndSet;
            }
        }

        private void LevelClosingSeting()
        {
            for (var i = 0; i < ComponentList.Count; i++)
            {
                ComponentList[i].enabled = LevelClosingSet;
            }
        }
        private void ClearingUICloseSeting()
        {
            for (var i = 0; i < ComponentList.Count; i++)
            {
                ComponentList[i].enabled = ClearingUICloseSet;
            }
        }
    }
}