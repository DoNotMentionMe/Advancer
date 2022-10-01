using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class VoidEventChannelTest : MonoBehaviour
    {
        [SerializeField] VoidEventChannel testChannel;

        private void OnTriggerEnter2D(Collider2D col)
        {
            testChannel.Broadcast();
        }

        private void OnEnable()
        {
            testChannel.AddListener(triggered);
        }

        private void OnDisable()
        {
            testChannel.RemoveListenner(triggered);
        }

        private void triggered()
        {
            Destroy(gameObject);
        }
    }
}