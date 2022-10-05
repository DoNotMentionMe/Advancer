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
        [SerializeField] float attack = 1f;
        [SerializeField] float UpMoveSpeed;
        [SerializeField] float LeftMoveSpeed;

        private string PlayerTag = "Player";
        private string PlayerAttackTag = "PlayerAttack";
        private Collider2D mCollider2D;
        private Transform mTransform;


        private void Awake()
        {
            mCollider2D = GetComponent<Collider2D>();
            mTransform = transform;
        }

        private void OnDestroy()
        {
            mCollider2D = null;
            mTransform = null;
        }

        private void Update()
        {
            if (weaponType == WeaponType.Up)
                mTransform.position += Vector3.down * UpMoveSpeed * Time.deltaTime;
            if (weaponType == WeaponType.Left)
                mTransform.position += Vector3.right * mTransform.localScale.x * LeftMoveSpeed * Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag.Equals(PlayerAttackTag))//被命中
            {
                attackHit.Broadcast();
                mCollider2D.enabled = false;
                if (weaponType != WeaponType.Normal)
                {
                    gameObject.SetActive(false);
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