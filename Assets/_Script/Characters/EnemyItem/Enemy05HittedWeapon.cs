using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class Enemy05HittedWeapon : MonoBehaviour
    {
        [SerializeField] CharacterDynamicController animController;
        [SerializeField] RotationDirection rotationDirection;
        [SerializeField] float DisappearTime;
        [SerializeField] Vector2 flyForce;
        [SerializeField] Rigidbody2D mRigidbody2D;
        [SerializeField] SpriteRenderer spriteRenderer;

        private void OnEnable()
        {
            StartCoroutine(nameof(Disappear));
        }

        IEnumerator Disappear()
        {
            animController.StartRotation(rotationDirection);
            mRigidbody2D.velocity = flyForce;

            float t = 0f;
            var color = spriteRenderer.color;
            while (t < 1f)
            {
                t += Time.deltaTime / DisappearTime;
                color.a = Mathf.Lerp(1f, 0.5f, t);
                spriteRenderer.color = color;
                yield return null;
            }

            animController.StopRotation();
            gameObject.SetActive(false);
        }
    }
}
