using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Mage : PlayerBase
{
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private GameObject projectilePrefab; 

    public override void Attack()
    {
        GameObject projectileInstance = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.identity);
        SpriteRenderer projectileSpriteRenderer = projectileInstance.GetComponent<SpriteRenderer>();
        Projectile mageProjectile = projectileInstance.GetComponent<Projectile>();

        bool shouldFlipSpriteOnX = IsFacingRight == false;

        mageProjectile.SetIsMovingRight(IsFacingRight);

        projectileSpriteRenderer.flipX = shouldFlipSpriteOnX;
    }
}
