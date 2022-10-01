using UnityEngine;

namespace Adv
{
    public class OneParameterEventChannel<T> : ScriptableObject
    {
        event System.Action<T> Delegate;

        public void Broadcast(T obj)
        {
            Delegate?.Invoke(obj);
        }

        public void AddListener(System.Action<T> action)
        {
            Delegate += action;
        }

        public void RemoveListenner(System.Action<T> action)
        {
            Delegate -= action;
        }
    }
}