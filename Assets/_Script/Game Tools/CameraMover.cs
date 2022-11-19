using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class CameraMover : MonoBehaviour
    {
        [SerializeField] Transform mCameraTransform;
        [SerializeField] VoidEventChannel UpAttack;
        [SerializeField] float MoveYTime;
        [SerializeField] float MoveYDistance;

        private Vector3 CameraInitalPosition = new Vector3(0, 0, -10);
        private Coroutine MoveYCoroutine;

        private void Awake()
        {
            Application.targetFrameRate = 120;
            UpAttack.AddListener(() =>
            {
                StartMoveY();
            });
        }

        private void StartMoveY()
        {
            if (MoveYCoroutine != null)
            {
                StopCoroutine(MoveYCoroutine);
                mCameraTransform.position = CameraInitalPosition;
            }
            MoveYCoroutine = StartCoroutine(nameof(MoveY));
        }

        IEnumerator MoveY()
        {
            float t = 0;
            while (t < MoveYTime)
            {
                mCameraTransform.Translate(Vector3.down * MoveYDistance / MoveYTime * Time.deltaTime);
                t += Time.deltaTime;
                yield return null;
            }
            t = 0;
            while (t < MoveYTime)
            {
                mCameraTransform.Translate(Vector3.up * MoveYDistance / MoveYTime * Time.deltaTime);
                t += Time.deltaTime;
                yield return null;
            }

            mCameraTransform.position = CameraInitalPosition;
            MoveYCoroutine = null;
        }

        private void OnDisable()
        {
            if (MoveYCoroutine != null)
            {
                StopCoroutine(MoveYCoroutine);
                mCameraTransform.position = CameraInitalPosition;
            }
        }
    }
}
