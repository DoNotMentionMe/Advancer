using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Adv
{
    public class ShowLevelNameAndCombo : MonoBehaviour
    {
        [SerializeField] Text 详细;
        [SerializeField] LevelManager levelManager;

        private void OnEnable()
        {
            // if (PlayerProperty.CurrentMaxCombo < PlayerProperty.Combo)
            //     PlayerProperty.CurrentMaxCombo = PlayerProperty.Combo;
            // for (var i = 0; i < levelManager.Level.Count;i++)
            // {
            //     if(levelManager.Level[i].Key==BaseLevelModule.currentRunningLevelKey)
            // }
            //     var levelMaxCombo =
            //     详细.text = BaseLevelModule.currentRunningLevelKey + "\n本次最高连击数 " + PlayerProperty.CurrentMaxCombo + "\n历史最高连击数" +
        }
    }
}