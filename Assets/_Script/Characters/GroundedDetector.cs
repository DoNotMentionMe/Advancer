using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adv
{
    public class GroundedDetector : MonoBehaviour
    {
        [SerializeField] float detectionRadius = 0.1f;
        [SerializeField] LayerMask groundLayer;

        Collider2D[] collider2Ds = new Collider2D[1];

        public bool IsGrounded => Physics2D.OverlapCircleNonAlloc(transform.position, detectionRadius, collider2Ds, groundLayer) != 0;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
    }
}