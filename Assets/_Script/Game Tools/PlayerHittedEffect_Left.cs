using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    //应用于敌人
    public class PlayerHittedEffect_Left : MonoBehaviour
    {
        private ParticleSystem mParSys;
        private Transform mTransform;
        private WaitForSeconds waitForLiftTime;
        [SerializeField] float LifeTime = 1f;

        void Awake()
        {
            mParSys = GetComponent<ParticleSystem>();
            mTransform = transform;
            waitForLiftTime = new WaitForSeconds(LifeTime);
        }

        private void OnEnable()
        {
            Effect();
        }

        public void Effect()
        {
            mParSys.Play();
            StartCoroutine(nameof(AutoSetFalse));
        }

        private void SetLocalScaleZ(float z)
        {
            var LocalScale = mTransform.localScale;
            LocalScale.z = z;
            mTransform.localScale = LocalScale;
        }

        IEnumerator AutoSetFalse()
        {
            yield return waitForLiftTime;
            gameObject.SetActive(false);
        }
    }
}
