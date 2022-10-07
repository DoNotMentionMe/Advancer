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
        [SerializeField] VoidEventChannel Fail;
        [SerializeField] VoidEventChannel Level1ButtonClick;
        [SerializeField] VoidEventChannel Level2ButtonClick;
        [SerializeField] float attack;
        [SerializeField] float Maxhealth;

        private float health;
        private PlayerAudio playerAudio;

        private void Awake()
        {
            playerAudio = GetComponent<PlayerAudio>();
            Level1ButtonClick.AddListener(() =>
            {
                health = Maxhealth;
                healtChange.Broadcast(health);
            });
            Level2ButtonClick.AddListener(() =>
            {
                health = Maxhealth;
                healtChange.Broadcast(health);
            });
        }

        private void OnEnable()
        {
            health = Maxhealth;
            healtChange.Broadcast(health);
        }

        public void Hitted(float damage)
        {
            health -= damage;
            if (health <= 0)
            {
                Fail.Broadcast();
            }
            healtChange.Broadcast(health);
            AudioManager.Instance.PlaySFX(playerAudio.BeHittedAudio);
        }


    }
}