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

            if (ChineseEnglishShift.language == Language.Chinese)
                //详细.text = BaseLevelModule.LastLevelKey + "\n本次最高连击数 " + PlayerProperty.CurrentMaxCombo + "\n历史最高连击数 " + historicHighCombo;
                详细.text = "本次最高连击数: " + PlayerProperty.CurrentMaxCombo + "\n历史最高连击数: " + historicHighCombo;
            else if (ChineseEnglishShift.language == Language.English)
                详细.text = "Highest combo this time: " + PlayerProperty.CurrentMaxCombo + "\nHighest combo on record: " + historicHighCombo;

        }
    }
}