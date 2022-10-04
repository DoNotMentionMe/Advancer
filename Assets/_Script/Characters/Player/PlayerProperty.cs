using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerProperty : MonoBehaviour
    {
        public float ATK => attack;

        //----作废----
        public float PerfectDefenceFrame = 7;
        //-----------

        [SerializeField] FloatEventChannel healtChange;
        [SerializeField] float attack;
        [SerializeField] float Maxhealth;

        private float health;
        private PlayerAudio playerAudio;

        private void Awake()
        {
            playerAudio = GetComponent<PlayerAudio>();
        }

        private void OnEnable()
        {
            health = Maxhealth;
            healtChange.Broadcast(health);
        }

        public void Hitted(float damage)
        {
            health -= damage;
            healtChange.Broadcast(health);
            AudioManager.Instance.PlaySFX(playerAudio.BeHittedAudio);
        }


    }
}