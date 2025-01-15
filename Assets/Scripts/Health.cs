using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event EventHandler OnDamageTaken;

    [SerializeField] private int maxHealthAmount = 3;

    private int currentHealthAmount;

    private void Start()
    {
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
        Destroy(gameObject);
    }
}
