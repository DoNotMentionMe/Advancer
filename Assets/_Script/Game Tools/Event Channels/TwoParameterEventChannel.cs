using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class TwoParameterEventChannel<T, V> : ScriptableObject
    {
        [SerializeField, TextArea(2, 5)] string comment;
        event System.Action<T, V> Delegate;

        public void Broadcast(T obj1, V obj2)
        {
            Delegate?.Invoke(obj1, obj2);
        }

        public void AddListener(System.Action<T, V> action)
        {
            Delegate += action;
        }

        public void RemoveListenner(System.Action<T, V> action)
        {
            Delegate -= action;
        }
    }
}
