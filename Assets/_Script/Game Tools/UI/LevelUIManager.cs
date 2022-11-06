using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Adv
{
    public class LevelUIManager : MonoBehaviour
    {
        [SerializeField] Button LevelSelectLabelButton;
        [SerializeField] Button Level0;
        [SerializeField] Button Level1Easy;


        private void Awake()
        {
            LevelSelectLabelButton.onClick.AddListener(() =>
            {
                if (!Level0.enabled)
                {
                    Level1Easy.Select();
                }
                else
                {
                    Level0.Select();
                }
            });
        }

    }
}
