using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    //应用于玩家
    public class PlayerHittedEffect_Right : MonoBehaviour
    {
        [SerializeField] PlayerHittedEventChannel playerhitted;
        [SerializeField] GameObject PlayerBeHittedEffect;

        void Awake()
        {
            var PlayerPont = new Vector2(0, -2.3f);
            playerhitted.AddListener((playerhitted, contactPoint) =>
            {
                //PoolManager.Instance.Release(PlayerBeHittedEffect, PlayerPont);
            });
        }
    }
}
