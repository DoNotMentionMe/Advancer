using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class Level1 : BaseLevelModule
    {
        public override string Key { get; } = nameof(Level1);

        [SerializeField] GameObject Enemy01;//小猪
        [SerializeField] GameObject Enemy02;//雷鸟
        [SerializeField] GameObject Enemy03;//刀客
        [SerializeField] float Level1Duration;
        [SerializeField] float Level1ReleaseInterval;

        private WaitForSeconds waitForReleaseInterval;

        protected override void Awake()
        {
            base.Awake();
            waitForReleaseInterval = new WaitForSeconds(Level1ReleaseInterval);
        }

        protected override void ReleaseEnemyEvent()
        {
            StartCoroutine(nameof(RandomReleaseLevel1Enemy));
        }

        protected override void RunAfterEnemysDied()
        {

        }

        IEnumerator RandomReleaseLevel1Enemy()
        {
            GameObject obj = null;
            GameObject GenerationPos = null;
            var startTime = Time.time;
            while (Time.time - startTime < Level1Duration)
            {
                var random = Random.Range(1, 4);
                if (random == 1)
                    obj = Enemy01;
                else if (random == 2)
                {
                    obj = Enemy02;
                    GenerationPos = EnemyGenerationPosition2;
                }
                else if (random == 3)
                {
                    obj = Enemy03;
                }
                if (random == 1 || random == 3)
                    random = Random.Range(1, 3);
                if (random == 1)
                    GenerationPos = EnemyGenerationPosition1;
                else if (random == 2)
                    GenerationPos = EnemyGenerationPosition3;
                //liveEnemyList.Add(PoolManager.Instance.Release(obj, GenerationPos.transform.position));
                ReleaseEnemy(obj, GenerationPos.transform.position);
                yield return waitForReleaseInterval;
            }
        }
    }

}