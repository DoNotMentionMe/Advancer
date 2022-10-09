using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class BOSS01Weapon : MonoBehaviour
    {
        public enum WeaponType
        {
            Normal, Up, Left
        }
        [SerializeField] WeaponType weaponType = WeaponType.Normal;
        [SerializeField] VoidEventChannel attackHit;
        [SerializeField] VoidEventChannel Fail;
        [SerializeField] FloatEventChannel bossTageChange;
        [SerializeField] float attack = 1f;
        [SerializeField] float UpMoveSpeed;
        [SerializeField] float LeftMoveSpeed;

        private float initalUpMoveSpeed;
        private float initalLeftMoveSpeed;
        private string PlayerTag = "Player";
        private string PlayerAttackTag = "PlayerAttack";
        private Collider2D mCollider2D;
        private Transform mTransform;
        private Quaternion leftQuaternion;
        private Quaternion rightQuaternion;
        private Quaternion initalRotation;
        private Vector3 initalPosition;

        private void Awake()
        {
            mCollider2D = GetComponent<Collider2D>();
            mTransform = transform;
            leftQuaternion = mTransform.localRotation;
            rightQuaternion = leftQuaternion * new Quaternion(0, 0, 180, 0);
            bossTageChange.AddListener(TageChange);
            initalUpMoveSpeed = 8;
            initalLeftMoveSpeed = 8;
            initalRotation = mTransform.rotation;
            initalPosition = mTransform.position;
            Fail.AddListener(() =>
            {
                if (weaponType != WeaponType.Normal)
                    gameObject.SetActive(false);
            });
        }

        private void OnEnable()
        {
            if (weaponType != WeaponType.Normal)
            {
                mCollider2D.enabled = true;
            }
            if (weaponType == WeaponType.Left)
            {
                if (mTransform.position.x < 0)
                    mTransform.localRotation = leftQuaternion;
                else
                    mTransform.localRotation = rightQuaternion;
            }
        }

        private void OnDisable()
        {
            if (weaponType == WeaponType.Normal)
            {
                mTransform.position = initalPosition;
                mTransform.rotation = initalRotation;
            }
        }

        private void OnDestroy()
        {
            mCollider2D = null;
            mTransform = null;
            bossTageChange.RemoveListenner(TageChange);
        }

        private void Update()
        {
            if (weaponType == WeaponType.Up)
                mTransform.position += Vector3.down * UpMoveSpeed * Time.deltaTime;
            if (weaponType == WeaponType.Left)
            {
                if (mTransform.position.x < 0)
                    mTransform.position += Vector3.right * LeftMoveSpeed * Time.deltaTime;
                else
                    mTransform.position += Vector3.left * LeftMoveSpeed * Time.deltaTime;
            }
        }

        private void TageChange(float tage)
        {
            if (tage == 1)
            {
                UpMoveSpeed = initalUpMoveSpeed;
                LeftMoveSpeed = initalLeftMoveSpeed;
            }
            if (tage == 2)
            {
                UpMoveSpeed = initalUpMoveSpeed * 1.43f;
                LeftMoveSpeed = initalLeftMoveSpeed * 1.43f;
            }
            else if (tage == 3)
            {
                UpMoveSpeed = initalUpMoveSpeed * 1.8f;
                LeftMoveSpeed = initalUpMoveSpeed * 1.8f;
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag.Equals(PlayerAttackTag))//被命中
            {
                attackHit.Broadcast();
                mCollider2D.enabled = false;
                if (weaponType != WeaponType.Normal)
                {
                    gameObject.SetActive(false);//暂时为直接关闭
                }
            }
            if (col.tag.Equals(PlayerTag))
            {
                if (col.gameObject.TryGetComponent<PlayerProperty>(out PlayerProperty playerProperty))
                {
                    playerProperty.Hitted(attack);
                    mCollider2D.enabled = false;
                    if (weaponType != WeaponType.Normal)
                    {
                        gameObject.SetActive(false);
                    }
                }
            }
        }

    }
}