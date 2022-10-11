using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerProperty : MonoBehaviour
    {
        public static int Combo = 0;
        public static int CurrentMaxCombo = 0;
        private static FloatEventChannel ComboChange;
        public float ATK => attack;

        [SerializeField] FloatEventChannel healtChange;
        [SerializeField] VoidEventChannel LevelStart;
        [SerializeField] VoidEventChannel LevelEnd;
        [SerializeField] float attack;
        [SerializeField] float Maxhealth;

        private float health;
        private PlayerAudio playerAudio;

        private void Awake()
        {
            //静态变量无法编辑器中赋值，通过索引进行文件获取
            ComboChange = Resources.Load<FloatEventChannel>("EventChannels/FloatEventChannel_ComboChange");

            playerAudio = GetComponent<PlayerAudio>();
            LevelStart.AddListener(() =>
            {
                Combo = 0;
                health = Maxhealth;
                healtChange.Broadcast(health);
            });
            LevelEnd.AddListener(() =>
            {
                if (CurrentMaxCombo < Combo)
                    CurrentMaxCombo = Combo;
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
            ResetCombo();
            health -= damage;
            healtChange.Broadcast(health);
            if (health <= 0)
            {
                //失败
                LevelEnd.Broadcast();
            }
            AudioManager.Instance.PlaySFX(playerAudio.BeHittedAudio);
        }

        public static void AddCombo()
        {
            Combo++;
            ComboChange.Broadcast(Combo);
        }

        public static void ResetCombo()
        {
            if (CurrentMaxCombo < Combo)
                CurrentMaxCombo = Combo;
            Combo = 0;
            ComboChange.Broadcast(Combo);
        }


    }
}