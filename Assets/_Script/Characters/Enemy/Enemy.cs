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
        [SerializeField] Collider2D AttackCol;
        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] float disappearTime;

        private bool HasAttacked = false;
        private string PlayerTag = "Player";

        protected virtual void OnEnable()
        {
            health = maxHealth;
            AttackCol.enabled = true;

            var color = spriteRenderer.color;
            color.a = 1f;
            spriteRenderer.color = color;
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
                    {
                        playerHitted.Broadcast(PlayerHitted.Hitted_Right, contactPoint);
                        //PlayerHittedEffect_Right.Instance.Effect_Right(contactPoint);
                    }
                    else
                    {
                        playerHitted.Broadcast(PlayerHitted.Hitted_Left, contactPoint);
                        //PlayerHittedEffect_Left.Instance.Effect_Left(contactPoint);
                    }

                    playerProperty.Hitted(1);

                    //逐渐消失
                    StartCoroutine(nameof(Disappear));
                }
            }
        }

        IEnumerator Disappear()
        {
            //AttackCol.enabled = false;
            float t = 0f;
            var color = spriteRenderer.color;
            while (t < 1f)
            {
                t += Time.deltaTime / disappearTime;
                color.a = Mathf.Lerp(1f, 0f, t);
                spriteRenderer.color = color;
                yield return null;
            }
            gameObject.SetActive(false);
        }
    }
}