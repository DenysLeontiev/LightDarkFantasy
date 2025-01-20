using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public bool IsDead { get; private set; } = false;

    public event EventHandler OnDamageTaken;
    public event EventHandler OnDied;

    [SerializeField] private int maxHealthAmount = 3;

    private int currentHealthAmount;

    private Collider2D entityCollider2D;

    private void Start()
    {
        entityCollider2D = GetComponent<Collider2D>();

        currentHealthAmount = maxHealthAmount;
    }

    public void TakeDamage(int damageAmount = 1)
    {
        currentHealthAmount -= damageAmount;
        currentHealthAmount = Mathf.Clamp(currentHealthAmount, 0, maxHealthAmount);

        OnDamageTaken?.Invoke(this, EventArgs.Empty);

        if (currentHealthAmount <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDied?.Invoke(this, EventArgs.Empty);
        entityCollider2D.enabled = false;
        IsDead = true;
    }
}
