using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] protected float maxHealth = 5;
        [SerializeField] protected float health;
        [SerializeField] PlayerHittedEventChannel playerHitted;

        private bool HasAttacked = false;
        private string PlayerTag = "Player";

        protected virtual void OnEnable()
        {
            health = maxHealth;
        }

        protected virtual void OnDisable()
        {
            HasAttacked = false;
        }

        public virtual void Hitted(float damage)
        {
            health -= damage;
            if (health <= 0)
            {
                Died();
            }
        }

        protected virtual void Died()
        {
            health = 0;
            //attackHit.RemoveListenner(Hited);
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!HasAttacked && col.tag.Equals(PlayerTag))
            {
                HasAttacked = true;
                if (col.gameObject.TryGetComponent<PlayerProperty>(out PlayerProperty playerProperty))
                {
                    var contactPoint = col.ClosestPoint(transform.position);
                    if (contactPoint.x > 0)
                        playerHitted.Broadcast(PlayerHitted.Hitted_Right);
                    else
                        playerHitted.Broadcast(PlayerHitted.Hitted_Left);
                    playerProperty.Hitted(1);
                }
            }
        }
    }
}