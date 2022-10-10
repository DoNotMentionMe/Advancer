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
        [SerializeField] VoidEventChannel LevelStart;
        [SerializeField] VoidEventChannel LevelEnd;
        [SerializeField] float attack;
        [SerializeField] float Maxhealth;

        private float health;
        private PlayerAudio playerAudio;

        private void Awake()
        {
            playerAudio = GetComponent<PlayerAudio>();
            LevelStart.AddListener(() =>
            {
                health = Maxhealth;
                healtChange.Broadcast(health);
            });
            LevelEnd.AddListener(() =>
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
            if (BaseLevelModule.currentRunningLevelKey.Equals(BaseLevelModule.EndKey)) return;
            health -= damage;
            if (health <= 0)
            {
                LevelEnd.Broadcast();
            }
            healtChange.Broadcast(health);
            AudioManager.Instance.PlaySFX(playerAudio.BeHittedAudio);
        }


    }
}