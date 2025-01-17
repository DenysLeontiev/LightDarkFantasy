using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class MageProjectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    private bool isMovingRight = true;

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
        Destroy(this.gameObject);
    }
}
