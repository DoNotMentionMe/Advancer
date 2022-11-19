using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class ParticleSystemPlayOnenable : MonoBehaviour
    {
        private ParticleSystem mParSys;

        private void Awake()
        {
            mParSys = GetComponent<ParticleSystem>();
        }

        private void OnEnable()
        {
            mParSys.Play();
        }
    }
}
