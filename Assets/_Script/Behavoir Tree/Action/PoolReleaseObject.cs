using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Adv
{
    [TaskDescription("对象池释放对象，默认位置为自身位置")]
    public class PoolReleaseObject : Action
    {
        [SerializeField] bool SyncLocalScale = true;
        [SerializeField] SharedGameObject releasePrefab;
        [SerializeField] SharedVector3 releasePositionOffset;

        public override TaskStatus OnUpdate()
        {
            var offset = releasePositionOffset.Value;
            offset.x *= transform.localScale.x;
            if (SyncLocalScale)
                PoolManager.Instance.Release(releasePrefab.Value, transform.position + offset, Quaternion.identity, transform.localScale);
            else
                PoolManager.Instance.Release(releasePrefab.Value, transform.position + offset, Quaternion.identity);
            return TaskStatus.Success;
        }
    }
}