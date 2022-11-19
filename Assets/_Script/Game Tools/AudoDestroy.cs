using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class AudoDestroy : MonoBehaviour
    {
        [SerializeField] bool IsDestroy = false;
        [SerializeField] float LifeTime;

        private WaitForSeconds waitForLifeTime;

        private void Awake()
        {
            waitForLifeTime = new WaitForSeconds(LifeTime);
        }

        private void OnEnable()
        {
            StartCoroutine(nameof(SetActiveself));
        }

        IEnumerator SetActiveself()
        {
            yield return waitForLifeTime;
            if (IsDestroy)
                Destroy(gameObject);
            else
                gameObject.SetActive(false);
        }
    }
}
