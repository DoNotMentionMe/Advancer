using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Adv
{
    public class Level0 : BaseLevelModule
    {
        public override string Key => nameof(Level0);

        [SerializeField] GameObject Enemy01;//小猪
        [SerializeField] GameObject Enemy02;//雷鸟
        [SerializeField] GameObject Enemy03;//刀客
        [SerializeField] float ReleaseInterval1;
        [SerializeField] float ReleaseInterval2;
        [SerializeField] float Level0Tage2Duration;
        [SerializeField] Text Tips;
        [SerializeField, TextArea(2, 5)] string tage1Tips;
        [SerializeField, TextArea(2, 5)] string tage2Tips;
        [SerializeField] float TipsShowTime;

        private WaitForSeconds waitForReleaseInterval1;
        private WaitForSeconds waitForReleaseInterval2;
        private WaitForSeconds waitForTipsShowTime;

        protected override void Awake()
        {
            base.Awake();
            waitForReleaseInterval1 = new WaitForSeconds(ReleaseInterval1);
            waitForReleaseInterval2 = new WaitForSeconds(ReleaseInterval2);
            waitForTipsShowTime = new WaitForSeconds(TipsShowTime);
        }

        protected override void ReleaseEnemyEvent()
        {
            StartCoroutine(nameof(ReleaseEnemy));
        }

        protected override void RunAfterEnemysDied()
        {

        }

        IEnumerator ReleaseEnemy()
        {
            var nullObj = new GameObject();
            liveEnemyList.Add(nullObj);//防止提前结束

            yield return StartCoroutine(ShowTipsSeconds(tage1Tips));
            //1
            ReleaseEnemy(Enemy01, EnemyGenerationPosition1.transform.position);
            yield return waitForReleaseInterval1;
            ReleaseEnemy(Enemy01, EnemyGenerationPosition3.transform.position);
            yield return waitForReleaseInterval1;
            ReleaseEnemy(Enemy02, EnemyGenerationPosition3.transform.position);
            yield return waitForReleaseInterval1;
            ReleaseEnemy(Enemy03, EnemyGenerationPosition1.transform.position);
            yield return waitForReleaseInterval1;
            var obj1 = ReleaseEnemy(Enemy03, EnemyGenerationPosition3.transform.position);
            yield return waitForReleaseInterval1;
            //2
            while (obj1.activeSelf == true)
            {
                yield return null;
            }
            yield return StartCoroutine(ShowTipsSeconds(tage2Tips));
            GameObject obj = null;
            GameObject GenerationPos = null;
            var startTime = Time.time;
            while (Time.time - startTime < Level0Tage2Duration)
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
                    var Enemy03Count = CheckLiveListTheEnemyCount(Enemy03.name);
                    if (Enemy03Count >= 1)
                        obj = Enemy01;
                    else
                        obj = Enemy03;
                }
                if (random == 1 || random == 3)
                    random = Random.Range(1, 3);
                if (random == 1)
                    GenerationPos = EnemyGenerationPosition1;
                else if (random == 2)
                    GenerationPos = EnemyGenerationPosition3;
                //liveEnemyList.Add(PoolManager.Instance.Release(obj, GenerationPos.transform.position));
                yield return waitForReleaseInterval2;
                ReleaseEnemy(obj, GenerationPos.transform.position);
            }

            liveEnemyList.Remove(nullObj);//去除空对象
        }

        IEnumerator ShowTipsSeconds(string tips)
        {
            Tips.enabled = true;
            Tips.text = tips;
            yield return waitForTipsShowTime;
            Tips.enabled = false;
        }
    }
}