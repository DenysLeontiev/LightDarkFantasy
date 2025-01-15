using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : PlayerBase
{
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private float attackRadius = 1f;
    [SerializeField] private Transform attackPoint;

    public override void Attack()
    {
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius);

        foreach (Collider2D collider in collider2DArray)
        {
            if(collider != playerCollider2D && collider.TryGetComponent<Health>(out Health health))
            {
                health.TakeDamage(damageAmount);
                return;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
