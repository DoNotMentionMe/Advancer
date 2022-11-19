using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public enum PlayerHitted
    {
        Hitted_Right,
        Hitted_Left,
    }
    [CreateAssetMenu(menuName = "Data/EventChannels/PlayerHittedEventChannels", fileName = "PlayerHittedEventChannel_")]
    public class PlayerHittedEventChannel : TwoParameterEventChannel<PlayerHitted, Vector2>
    {

    }
}
