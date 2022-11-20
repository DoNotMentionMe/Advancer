using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public enum DynamicChangeDirection
    {
        Horizontal,
        Vertical
    }

    public enum RotationDirection
    {
        Clockwise,//顺时针
        Anticlockwise//逆时针
    }
    /// <summary>
    /// 控制角色横纵循环动态变化，Juice！！
    /// </summary>
    public class CharacterDynamicController : MonoBehaviour
    {

        [SerializeField] Transform mTransform;
        [Header("拉扯参数")]
        [SerializeField] bool DontChangeScale = false;//置true时将无法进行动态拉扯
        [SerializeField] float PullDifference;
        [SerializeField] float ChangePeriod;
        [Space]
        [Header("旋转参数")]
        [SerializeField] float RotateSpeed;
        [Space]
        [Header("缩小")]
        [SerializeField] float ScaleSmallTime;

        private Coroutine DynamicChangeCoroutine;
        private Coroutine RotationCoroutine;
        private Coroutine ScaleSmallCoroutine;

        private void OnDisable()
        {
            if (!DontChangeScale)
                StopDynamicChange();//先关掉循环变化的协程
            StopRotation();
            StopAllCoroutines();
            if (!DontChangeScale)
                mTransform.localScale = Vector3.one;
            mTransform.rotation = Quaternion.Euler(0, 0, 0);
            ScaleSmallCoroutine = null;
        }

        #region 循环动态拉扯
        /// <summary>
        /// 动态拉扯变化
        /// </summary>
        public void StartDynamicChange()
        {
            if (DynamicChangeCoroutine == null && !DontChangeScale)
                DynamicChangeCoroutine = StartCoroutine(nameof(DynamicChange));
        }

        public void StopDynamicChange()
        {
            //停止协程
            if (DynamicChangeCoroutine != null && !DontChangeScale)
            {
                StopCoroutine(DynamicChangeCoroutine);
                DynamicChangeCoroutine = null;
                //Scale恢复原状
                mTransform.localScale = Vector3.one;
            }
        }

        IEnumerator DynamicChange()
        {
            var changeSpeedPerFrame = PullDifference * 2 / ChangePeriod * Time.deltaTime;
            while (true)
            {
                mTransform.localScale = Vector3.one;
                //横向拉扯
                while (mTransform.localScale.x < 1 + PullDifference)
                {
                    var localScale = mTransform.localScale;
                    localScale.x += changeSpeedPerFrame;
                    localScale.y -= changeSpeedPerFrame;
                    mTransform.localScale = localScale;
                    yield return null;
                }
                mTransform.localScale = Vector3.one;
                //纵向拉扯
                while (mTransform.localScale.y < 1 + PullDifference)
                {
                    var localScale = mTransform.localScale;
                    localScale.x -= changeSpeedPerFrame;
                    localScale.y += changeSpeedPerFrame;
                    mTransform.localScale = localScale;
                    yield return null;
                }
            }
        }
        #endregion

        #region 单次动态拉扯

        /// <summary>
        /// 进行单次拉扯，可选方向
        /// </summary>
        public void StartDynamicChange(DynamicChangeDirection direction, float pullDifference, float changePeriod, System.Action action)
        {
            if (direction == DynamicChangeDirection.Horizontal)
            {
                StartCoroutine(DynamicHorizontalWithCount(pullDifference, changePeriod, action));
            }
            else if (direction == DynamicChangeDirection.Vertical)
            {
                StartCoroutine(DynamicVerticalWithCount(pullDifference, changePeriod, action));
            }
        }

        IEnumerator DynamicHorizontalWithCount(float pullDifference, float changePeriod, System.Action action)
        {
            var changeSpeedPerFrame = pullDifference / changePeriod * Time.deltaTime;
            mTransform.localScale = Vector3.one;
            //横向拉扯
            while (mTransform.localScale.x < 1 + pullDifference)
            {
                var localScale = mTransform.localScale;
                localScale.x += changeSpeedPerFrame;
                localScale.y -= changeSpeedPerFrame;
                mTransform.localScale = localScale;
                yield return null;
            }
            action?.Invoke();
        }

        IEnumerator DynamicVerticalWithCount(float pullDifference, float changePeriod, System.Action action)
        {
            var changeSpeedPerFrame = pullDifference / changePeriod * Time.deltaTime;
            mTransform.localScale = Vector3.one;
            //纵向拉扯
            while (mTransform.localScale.y < 1 + pullDifference)
            {
                var localScale = mTransform.localScale;
                localScale.x -= changeSpeedPerFrame;
                localScale.y += changeSpeedPerFrame;
                mTransform.localScale = localScale;
                yield return null;
            }
            action?.Invoke();
        }

        #endregion

        #region 旋转

        public void StartRotation(RotationDirection direction)
        {
            if (RotationCoroutine == null)
                RotationCoroutine = StartCoroutine(Rotation(direction));
        }

        public void StartRotation(RotationDirection direction, float count)
        {
            if (RotationCoroutine == null)
                RotationCoroutine = StartCoroutine(RotationCount(direction, count));
        }

        public void StartRotationWithSpeed(RotationDirection direction, float RotationSpeed)
        {
            if (RotationCoroutine == null)
                RotationCoroutine = StartCoroutine(RotationSetSpeed(direction, RotationSpeed));
        }


        public void StopRotation()
        {
            if (RotationCoroutine != null)
                StopCoroutine(RotationCoroutine);
            RotationCoroutine = null;

            mTransform.rotation = Quaternion.Euler(0, 0, 0);
        }

        IEnumerator Rotation(RotationDirection direction)
        {
            int value = 0;
            if (direction == RotationDirection.Clockwise)
            {
                value = -1;
            }
            else if (direction == RotationDirection.Anticlockwise)
            {
                value = 1;
            }

            //mTransform.rotation = Quaternion.Euler(0, 0, 0);
            while (true)
            {
                mTransform.Rotate(Vector3.forward, value * RotateSpeed * Time.deltaTime);

                yield return null;
            }
        }

        IEnumerator RotationSetSpeed(RotationDirection direction, float RotationSpeed)
        {
            int value = 0;
            if (direction == RotationDirection.Clockwise)
            {
                value = -1;
            }
            else if (direction == RotationDirection.Anticlockwise)
            {
                value = 1;
            }

            //mTransform.rotation = Quaternion.Euler(0, 0, 0);
            while (true)
            {
                mTransform.Rotate(Vector3.forward, value * RotationSpeed * Time.deltaTime);

                yield return null;
            }
        }

        IEnumerator RotationCount(RotationDirection direction, float count)
        {
            int value = 0;
            if (direction == RotationDirection.Clockwise)
            {
                value = -1;
            }
            else if (direction == RotationDirection.Anticlockwise)
            {
                value = 1;
            }
            float t = count * (360 / RotateSpeed);

            while (t > 0)
            {
                mTransform.Rotate(Vector3.forward, value * RotateSpeed * Time.deltaTime);
                t -= Time.deltaTime;
                yield return null;
            }
            mTransform.rotation = Quaternion.Euler(0, 0, 0);
            RotationCoroutine = null;
        }

        #endregion

        #region 缩小

        public void StartScaleSmall()
        {
            if (ScaleSmallCoroutine == null)
                ScaleSmallCoroutine = StartCoroutine(nameof(ScaleSmall));
        }

        IEnumerator ScaleSmall()
        {
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime / ScaleSmallTime;
                var scale = mTransform.localScale;
                var scaleX = scale.x;
                var scaelY = scale.y;
                scale.x = Mathf.Lerp(scaleX, 0f, t);
                scale.y = Mathf.Lerp(scaelY, 0f, t);
                mTransform.localScale = scale;
                yield return null;
            }
            ScaleSmallCoroutine = null;
        }

        #endregion

        public void ResetLocalScale()
        {
            mTransform.localScale = Vector3.one;
        }
    }
}
