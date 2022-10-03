using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PersistentSingletonComponent : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}