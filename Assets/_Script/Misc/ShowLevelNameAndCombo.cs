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
            int historicHighCombo = 0;
            for (var i = 0; i < levelManager.Level.Count; i++)
            {
                if (levelManager.Level[i].Key == BaseLevelModule.LastLevelKey)
                {
                    historicHighCombo = levelManager.Level[i].LevelMaxCombo;
                }
            }

            详细.text = BaseLevelModule.LastLevelKey + "\n本次最高连击数 " + PlayerProperty.CurrentMaxCombo + "\n历史最高连击数 " + historicHighCombo;
        }
    }
}