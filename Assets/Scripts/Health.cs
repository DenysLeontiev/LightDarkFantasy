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

    private Collider2D collider2D;

    private void Start()
    {
        collider2D = GetComponent<Collider2D>();
        currentHealthAmount = maxHealthAmount;
    }

    public void TakeDamage(int damageAmount = 1)
    {
        currentHealthAmount -= damageAmount;
        currentHealthAmount = Mathf.Clamp(currentHealthAmount, 0, maxHealthAmount);

        OnDamageTaken?.Invoke(this, EventArgs.Empty);

        Debug.Log(transform.name);

        if (currentHealthAmount <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDied?.Invoke(this, EventArgs.Empty);
        collider2D.enabled = false;
        IsDead = true;
    }
}
