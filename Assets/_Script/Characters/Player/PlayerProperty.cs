using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class PlayerProperty : MonoBehaviour
    {
        public static int ShieldCombo = 0;
        public static int Combo = 0;
        public static int CurrentMaxCombo = 0;
        private static FloatEventChannel ComboChange;
        public static bool NotHurtCurrentLevel = true;
        public static bool NotEmptyAttackCurrentLevel = true;
        public float ATK => attack;

        [SerializeField] FloatEventChannel healtChange;
        [SerializeField] VoidEventChannel LevelStart;
        [SerializeField] VoidEventChannel LevelEnd;
        [SerializeField] VoidEventChannel LevelClosing;
        [SerializeField] VoidEventChannel Charge;
        [SerializeField] GameObject Shield;
        public bool CanGetShield;
        public int CountComboToGetShield;
        [SerializeField] float attack;
        [SerializeField] float maxhealth;

        private bool IsHitted = false;//用于护盾功能连击
        private float health;
        private PlayerAudio playerAudio;

        private void Awake()
        {
            //静态变量无法编辑器中赋值，通过索引进行文件获取
            ComboChange = Resources.Load<FloatEventChannel>("EventChannels/FloatEventChannel_ComboChange");

            ComboChange.AddListener((combo) =>
            {
                if (combo != 0 && !Shield.activeSelf)//命中且无护盾
                {
                    ShieldCombo++;
                }
                else if (combo == 0 && !IsHitted)//攻击落空或开头
                {

                }
                else if (combo == 0 && IsHitted)//受伤
                {
                    ShieldCombo = 0;
                    IsHitted = false;
                }

                if (CanGetShield && ShieldCombo != 0 && ShieldCombo % CountComboToGetShield == 0)
                {
                    ShieldCombo = 0;
                    Shield.SetActive(true);
                }
            });

            playerAudio = GetComponent<PlayerAudio>();
            LevelStart.AddListener(() =>
            {
                NotHurtCurrentLevel = true;
                NotEmptyAttackCurrentLevel = true;
                CurrentMaxCombo = 0;
                Combo = 0;
                ComboChange.Broadcast(Combo);
                health = maxhealth;
                healtChange.Broadcast(maxhealth);
            });
            LevelEnd.AddListener(() =>
            {
                health = maxhealth;
                healtChange.Broadcast(maxhealth);
                ShieldCombo = 0;
                IsHitted = false;
            });

        }

        private void Start()
        {
            if (GameSaver.Instance.Exists("CanGetShield"))
                CanGetShield = GameSaver.Instance.Load<bool>("CanGetShield");
            if (GameSaver.Instance.Exists("CountComboToGetShield"))
                CountComboToGetShield = GameSaver.Instance.Load<int>("CountComboToGetShield");
            if (GameSaver.Instance.Exists("maxHealth"))
            {
                maxhealth = GameSaver.Instance.Load<float>("maxHealth");
                health = maxhealth;
                healtChange.Broadcast(maxhealth);
            }
            GameSaver.Instance.SaveDataEventCall(() =>
            {
                BayatGames.SaveGameFree.SaveGame.Save<bool>("CanGetShield", CanGetShield);
                BayatGames.SaveGameFree.SaveGame.Save<int>("CountComboToGetShield", CountComboToGetShield);
                BayatGames.SaveGameFree.SaveGame.Save<float>("maxHealth", maxhealth);
            });

        }

        private void OnEnable()
        {
            health = maxhealth;
            healtChange.Broadcast(maxhealth);
        }

        public void MaxHealthPlus1()
        {
            maxhealth += 1;
            health = maxhealth;
            healtChange.Broadcast(maxhealth);
        }

        public void Hitted(float damage)
        {
            if (BaseLevelModule.CurrentRunningLevelKey.Equals(BaseLevelModule.EndKey)) return;
            IsHitted = true;
            NotHurtCurrentLevel = false;
            ResetCombo();
            ShieldCombo = 0;
            health -= damage;
            healtChange.Broadcast(health);
            if (health <= 0)
            {
                //失败
                LevelEnd.Broadcast();
                LevelClosing.Broadcast();
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