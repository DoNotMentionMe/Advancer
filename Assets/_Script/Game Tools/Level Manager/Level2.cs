using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{

    public class Level2 : BaseLevelModule
    {
        public override string Key => nameof(Level2);

        [SerializeField] GameObject Enemy01;//小猪
        [SerializeField] GameObject Enemy02;//雷鸟
        [SerializeField] GameObject Enemy03;//刀客
        [SerializeField] GameObject Enemy04;//弓箭手
        [SerializeField] float Level2Duration;
        [SerializeField] float CurrentInterval;

        private float LevelStartTime;
        private WaitForSeconds waitForReleaseInterval;

        protected override void Awake()
        {
            base.Awake();
            waitForReleaseInterval = new WaitForSeconds(CurrentInterval);
        }

        protected override void ReleaseEnemyEvent()
        {
            StartCoroutine(nameof(RandomReleaseLevel2Enemy));
        }

        protected override void RunAfterEnemysDied()
        {

        }

        IEnumerator RandomReleaseLevel2Enemy()
        {
            var nullObj = new GameObject();
            liveEnemyList.Add(nullObj);//防止提前结束

            GameObject obj = null;
            GameObject GenerationPos = null;
            LevelStartTime = Time.time;

            while (Time.time - LevelStartTime < Level2Duration)
            {
                int random = 0;
                random = Random.Range(1, 7);
                if (random == 1)
                    obj = Enemy01;
                else if (random == 2)
                {
                    obj = Enemy02;
                    GenerationPos = EnemyGenerationPosition2;
                }
                else if (random == 3 || random == 4)
                {
                    obj = Enemy03;
                }
                else if (random == 5 || random == 6)
                {
                    obj = Enemy04;
                }

                if (random == 1 || random >= 3)
                    random = Random.Range(1, 3);//第二次随机用于释放位置
                if (random == 1)
                    GenerationPos = EnemyGenerationPosition1;
                else if (random == 2)
                    GenerationPos = EnemyGenerationPosition3;

                ReleaseEnemy(obj, GenerationPos.transform.position);
                yield return waitForReleaseInterval;
            }

            liveEnemyList.Remove(nullObj);
            Destroy(nullObj);
        }


    }

    //BOSS关
    // public class Level2 : BaseLevelModule
    // {
    //     public override string Key => nameof(Level2);

    //     [SerializeField] GameObject BOSS01;
    //     [SerializeField] GameObject 通关界面;
    //     private bool FirstLevel2Success = true;

    //     protected override void ReleaseEnemyEvent()
    //     {
    //         InstantiateEnemy(BOSS01);
    //     }

    //     protected override void RunAfterEnemysDied()
    //     {
    //         if (FirstLevel2Success)
    //         {
    //             FirstLevel2Success = false;
    //             通关界面.SetActive(true);
    //         }
    //     }
    // }
}