using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] protected float maxHealth = 5;
        [SerializeField] protected float health;

        private string PlayerTag = "Player";

        protected virtual void OnEnable()
        {
            health = maxHealth;
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
            if (col.tag.Equals(PlayerTag))
            {
                if (col.gameObject.TryGetComponent<PlayerProperty>(out PlayerProperty playerProperty))
                {
                    playerProperty.Hitted(1);
                }
            }
        }
    }
}