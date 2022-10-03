using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class ThunderBall : MonoBehaviour
    {
        [SerializeField] float attack = 1f;
        [SerializeField] float moveSpeed = 3f;

        private string PlayerTag = "Player";
        private string PlayerAttackTag = "PlayerAttack";
        private Rigidbody2D mRigidbody2D;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag.Equals(PlayerTag))
            {
                if (col.gameObject.TryGetComponent<PlayerProperty>(out PlayerProperty playerProperty))
                {
                    playerProperty.Hitted(attack);
                    gameObject.SetActive(false);
                }
            }
            if (col.tag.Equals(PlayerAttackTag))
            {
                gameObject.SetActive(false);
            }
        }

        private void Awake()
        {
            mRigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            mRigidbody2D.velocity = Vector2.down * moveSpeed;
        }

        private void OnDestroy()
        {
            mRigidbody2D = null;
        }
    }
}