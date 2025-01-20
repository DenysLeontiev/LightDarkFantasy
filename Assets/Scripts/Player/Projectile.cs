using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int projectileDamage = 1;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float destroyTimeAfterInitialization = 10f; // if it goest beyond the map, destroy it after destroyTimeAfterInitialization seconds
    private bool isMovingRight = true;

    private void Start()
    {
        Destroy(this.gameObject, destroyTimeAfterInitialization);
    }

    private void Update()
    {
        Vector3 dir = isMovingRight ? transform.right : transform.right * (-1f);
        transform.position += moveSpeed * Time.deltaTime * dir;
    }

    public void SetIsMovingRight(bool moveState)
    {
        isMovingRight = moveState;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Health targetHealth = collision.transform.GetComponent<Health>();

        if(targetHealth != null)
        {
            targetHealth.TakeDamage(projectileDamage);
        }

        Destroy(this.gameObject);
    }

}
