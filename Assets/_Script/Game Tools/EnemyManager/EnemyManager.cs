using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public enum GenerationObj
    {
        Enemy01, Enemy02, Enemy03
    }
    public enum GenerationPos
    {
        Left, Up, Right
    }
    /// <summary>
    /// 管理敌人生成、管理关卡（未实施）
    /// </summary>
    public class EnemyManager : MonoBehaviour
    {
        [Header("===== EnemyPrefabs =====")]
        [SerializeField] GameObject Enemy01;
        [SerializeField] GameObject Enemy02;
        [SerializeField] GameObject Enemy03;
        [Space]

        [SerializeField] List<EnemyGenerationInformation> Level1 = new List<EnemyGenerationInformation>();
        [SerializeField] GameObject EnemyGenerationPosition1;
        [SerializeField] GameObject EnemyGenerationPosition2;
        [SerializeField] GameObject EnemyGenerationPosition3;

        private float LevelStartTime;

        IEnumerator LevelProcessing(List<EnemyGenerationInformation> level)
        {
            LevelStartTime = Time.time;
            int currentOrder = 0;
            while (level.Count > currentOrder)
            {
                var generationTime = level[currentOrder].GenrationTimePoint;
                while (generationTime > (Time.time - LevelStartTime))
                {
                    yield return null;
                }
                RelaseEnemy(level[currentOrder]);
                currentOrder++;
            }
        }

        private void Awake()
        {
        }

        private void OnEnable()
        {
            StartCoroutine(LevelProcessing(Level1));
        }

        private void RelaseEnemy(EnemyGenerationInformation information)
        {
            GameObject obj = null;
            GameObject GenerationPos = null;
            if (information.GenerationObj == GenerationObj.Enemy01)
                obj = Enemy01;
            else if (information.GenerationObj == GenerationObj.Enemy02)
                obj = Enemy02;
            else if (information.GenerationObj == GenerationObj.Enemy03)
                obj = Enemy03;

            if (information.GenerationPosition == Adv.GenerationPos.Left)
                GenerationPos = EnemyGenerationPosition1;
            else if (information.GenerationPosition == Adv.GenerationPos.Up)
                GenerationPos = EnemyGenerationPosition2;
            else if (information.GenerationPosition == Adv.GenerationPos.Right)
                GenerationPos = EnemyGenerationPosition3;

            PoolManager.Instance.Release(obj, GenerationPos.transform.position);
            //Debug.Log("释放" + obj.name + "于" + GenerationPos.transform.position + "位置");
        }

    }
}